using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementBasis : MonoBehaviour
{
    public bool isSandBag;
    public int playerID;
    [HideInInspector] public bool isFinished;

    [Header("Stick Controlls")]
    public InputReader input;
    [HideInInspector] public Vector2 Axis;
    [HideInInspector] public bool isTouchingWall;

    [Header("Tapping Register")]
    public float joystickThresholdMin;
    public float joystickThresholdMax;

    [Header("Tapping Horizontal")]
    public float requiredStickTimeX;
    float framesHeld, prevThreshold;

    [Header("Movement Horizontal")]
    [Header("Speed")]
    public float walkSpeed;
    public float runSpeed;
    [HideInInspector] public float speed;
    [HideInInspector] public float platforMoving;
    [HideInInspector] public float finalspeed;
    bool isRunning;

    [Header("Dash")]
    public float dashSpeed;
    public float dashDuration;
    [HideInInspector] public bool isDashing;
    float dashTimeLeft, dashDirection;

    [Header("Traction")]
    public float tractionValue;
    [HideInInspector] public bool tractionBool, isChangingDirTraction;
    float tractionAxis, tractionFrames;

    [Header("Aerial Speed")]
    public float airFastSpeed;
    public float airSlowSpeed;
    bool speedAirOneTime;

    [Header("Movement Vertical")]
    [Header("Force")]
    public float speedShortHop;
    public float speedFullHop;
    [HideInInspector] public float verticalSpeed;
    float sTop;
    [Header("Time")]
    public float maxFramesSwitchJump; 
    public float maxFramesJump;
    float counterSwitchJump, counterJump;
    [HideInInspector] public bool isJumping;
    bool jOneTime, isDJumping, canDJump;
    [HideInInspector] public bool djOneTime;

    [Header("Gravity")]
    public float weight;
    float gravity;
    public float maxGravity;

    [Header("Fast Fall and Fall from platform")]
    public float requiredStickTimeY;
    float framesHeldY;
    [HideInInspector] public bool isFastFall;
    [HideInInspector] public bool canFallPlatform;
    bool isDownAxisY;

    [Header("Crouch")]
    [HideInInspector] public bool isCrouching;
    public bool isBusy;

    [Header("Knockback")]
    public float dragBase;
    [HideInInspector] public bool isHitted, knockbackBool, damagedOneTime;
    [HideInInspector] public float launchSpeed, launchAngle, direction;
    float angleRadY, angleRadX;
    [HideInInspector] public int damage, percentage;
    [HideInInspector] public Vector3 knockbackSpeed;

    [HideInInspector] public CollisionBox cb;
    [HideInInspector] public AudioPlayerController audio;
    HitBoxesController hit;

    private void Awake()
    {
        joystickThresholdMin = 0.2f;
        joystickThresholdMax = 0.8f;
    }

    // Start is called before the first frame update
    void Start()
    {
        input.MoveEvent += HandleMove;
        input.JumpEvent += HandleJump;
        input.JumpCancelEvent += HandleCancelJump;

        cb = GetComponent<CollisionBox>();
        audio = GetComponentInChildren<AudioPlayerController>();
        hit = GetComponentInChildren<HitBoxesController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished)
            Movement();
    }

    // Input Readers
    void HandleMove(Vector2 dir, int eventPlayerID)
    {
        if (eventPlayerID == playerID) Axis = dir;
    }

    void HandleJump(int eventPlayerID)
    {
        if (eventPlayerID == playerID)
        {
            if (!hit.isN && !hit.isF && !hit.isU && !hit.isD
            && !hit.isFSmash && !hit.isUSmash && !hit.isDSmash && !isBusy)
            {
                if (cb.isGrounded && !isJumping)
                {
                    isJumping = true;

                    if (!hit.isD && !hit.isDSmash) hit.CancelAttackForFallAnim();
                }

                if (canDJump) isDJumping = true;
            }
        }
    }

    void HandleCancelJump(int eventPlayerID)
    {
        if (eventPlayerID == playerID)
        {
            isJumping = false;
            isDJumping = false;
        }
    }

    // Logic Movement
    void HorzitonalMovement()
    {
        if (cb.isGrounded && !isCrouching &&
            !hit.isNAir && !hit.isDAir && !hit.isFAir && !hit.isBAir && !hit.isUAir) // Grounded
        {
            if (Mathf.Abs(Axis.x) > joystickThresholdMin && !tractionBool
                && Mathf.Abs(Axis.y) < joystickThresholdMin
                && !hit.isF && !hit.isU && !hit.isD
                && !hit.isFSmash && !hit.isUSmash && !hit.isDSmash && !isBusy)
            {
                if (!isRunning) framesHeld++;

                if (Mathf.Abs(Axis.x) >= joystickThresholdMax)
                {
                    if (framesHeld <= requiredStickTimeX)
                    {
                        if (!isDashing) // Start Dash
                        {
                            isDashing = true;
                            dashTimeLeft = dashDuration;
                            dashDirection = Axis.x;
                        }
                    }
                    else if (isDashing && Mathf.Sign(Axis.x) != Mathf.Sign(dashDirection)) // Change direction dash
                    {
                        isDashing = true;
                        dashTimeLeft = dashDuration;
                        dashDirection = Axis.x;
                    }

                    if (isRunning) speed = runSpeed;
                    else speed = walkSpeed;
                }
                else speed = walkSpeed;
            }
            else // Traction + Stop
            {
                if (isRunning) tractionBool = true;
                else if (speed == walkSpeed || isDashing)
                {
                    speed = 0;
                    isDashing = false;
                }

                if (speed > 0 && tractionBool) speed -= tractionValue;
                else if (speed <= 0 && tractionBool)
                {
                    speed = 0;
                    tractionFrames++;

                    if (tractionFrames >= 3)
                    {
                        tractionBool = false;
                        isChangingDirTraction = false;
                        tractionFrames = 0;
                    }
                }

                isRunning = false;
                framesHeld = 0;
            }

            if ((prevThreshold > joystickThresholdMin && Axis.x < -joystickThresholdMin ||
                prevThreshold < -joystickThresholdMin && Axis.x > joystickThresholdMin)
                && !isDashing)
            // Corrector para cambio de lado (reset a la variable contador)
            {
                framesHeld = 0;
                if (isRunning) // para la animacion quizas se debá de fusionar en una o mirar una alternativa.
                {
                    isChangingDirTraction = true;
                    tractionBool = true;
                }
            }

            if (Mathf.Abs(Axis.x) > joystickThresholdMin) prevThreshold = Axis.x;
            else prevThreshold = 0;

            speedAirOneTime = false;
        }
        else if (!cb.isGrounded) // Aerial
        {
            if (Mathf.Abs(Axis.x) > joystickThresholdMin && !speedAirOneTime)
            {
                if ((speed == walkSpeed || speed == runSpeed || isDashing) && isJumping && !isDJumping) speed = airFastSpeed;
                else speed = airSlowSpeed;

                speedAirOneTime = true;
            }
        }
        //}
    }

    void Crouch()
    {
        if (GetComponent<Rigidbody>().velocity == new Vector3(0, 0, 0) &&
            Axis.y <= -joystickThresholdMin && cb.isGrounded &&
            !tractionBool && !isDashing && speed == 0 &&
            !hit.isN && !hit.isF && !hit.isU && !hit.isNAir &&
            !hit.isFAir && !hit.isBAir && !hit.isDAir && !hit.isUAir &&
            !hit.isFSmash && !hit.isUSmash && !hit.isDSmash &&
            !isBusy)
        {
            isCrouching = true;
        }
        else
            isCrouching = false;
            //if (!hit.isD) isCrouching = false;
    }
    
    void Jump()
    {
        if (!cb.isGrounded)
        {
            canDJump = true;
        }

        if (!canDJump)
        {
            if (isJumping && cb.isGrounded) counterSwitchJump++;

            if (counterSwitchJump > 0) counterJump++;

            if (counterJump >= maxFramesJump)
            {
                if (counterSwitchJump >= maxFramesSwitchJump && cb.isGrounded && verticalSpeed == 0) sTop = speedFullHop;
                else if (counterSwitchJump < maxFramesSwitchJump && cb.isGrounded && verticalSpeed == 0) sTop = speedShortHop;

                if (!jOneTime)
                {
                    jOneTime = true;
                    audio.Jump();
                }
            }
        }
        else if (canDJump)
        {
            if (isDJumping && !djOneTime
                && !hit.isNAir && !hit.isFAir && !hit.isBAir && !hit.isDAir && !hit.isUAir)
            {
                verticalSpeed = 0;
                gravity = 0;
                sTop = speedFullHop;
                djOneTime = true;
                speedAirOneTime = false;

                isFastFall = false;
                framesHeldY = 0;

                audio.Jump();

                if (hit.isNAir || hit.isFAir || hit.isBAir || hit.isDAir || hit.isUAir)
                    hit.CancelAerialAttackAnim();
            }
        }

        if (cb.isGrounded && verticalSpeed < 0)
        {
            sTop = 0;
            verticalSpeed = 0;
            canDJump = false;
            isDJumping = false;
            djOneTime = false;

            if (counterJump >= maxFramesJump)
            {
                counterJump = 0;
                counterSwitchJump = 0;
            }

            if (isJumping) isJumping = false;
        }
    }

    void Gravity()
    { 
        if (!cb.isGrounded)
        {
            gravity += weight * 9.81f * Time.deltaTime;

            if (!hit.canAir) hit.canAir = true;

            if (hit.isF || hit.isU || hit.isD || 
                !hit.isFSmash || !hit.isUSmash || !hit.isDSmash) hit.CancelAttackForFallAnim();
        }
        else
        {
            gravity = 0;

            if (hit.canAir) hit.canAir = false;
        }
    }

    void FastFall()
    {
        if (!isSandBag)
        {
            if (!cb.isGrounded && verticalSpeed < 0)
            {
                if (Axis.y < -joystickThresholdMin)
                {
                    framesHeldY++;

                    if (Axis.y <= -joystickThresholdMax)
                    {
                        if (framesHeldY <= requiredStickTimeY && !isDownAxisY)
                        {
                            isFastFall = true;
                        }
                    }
                }
                else if (!isFastFall) framesHeldY = 0;
            }
        }
    }

   void FallPlatform() // ATENCION por alguna razon de las razones, si se realiza un ataque cancelado por tocar una plataforma, este fall estará jodido y consecuente el resto.
   {
        if (!isSandBag)
        {
            if (Axis.y < -joystickThresholdMin)
            {
                if (Axis.y <= -joystickThresholdMax)
                {
                    if (cb.raycastHitPlatform)
                    {
                        if ((!cb.isGrounded && verticalSpeed < 0) ||
                            cb.isGrounded)
                        {
                            if (cb.isGrounded) isDownAxisY = true;

                            canFallPlatform = true;
                            cb.isGrounded = false;
                            cb.raycastHitPlatform = false;
                        }
                    }
                }
            }
            else
            {
                isDownAxisY = false;

                if (!cb.isTouchingPlatform) 
                    canFallPlatform = false;
            }
        }
    }

    void Knockback()
    {
        if (knockbackBool)
        {
            float launchAngleX = launchAngle;
    
            if (direction == -1) launchAngleX -= 180;
    
            angleRadY = launchAngle * Mathf.Deg2Rad;
    
            angleRadX = launchAngleX * Mathf.Deg2Rad;
    
            if (!damagedOneTime)
            {
                percentage += damage;
    
                if (percentage >= 999) percentage = 999;
    
                GetComponent<DamagePlayer>().IsDamaged(percentage);
    
                damagedOneTime = true;
            }
    
            knockbackBool = false;
    
            knockbackSpeed = new Vector3(Mathf.Cos(angleRadX) * launchSpeed * percentage, Mathf.Sin(angleRadY) * launchSpeed * percentage, 0);
        }
    
        Vector3 dragForce = dragBase * knockbackSpeed;
        Vector3 gravityKnockback = new Vector3(1, 1, 0) * 9.81f;
    
        knockbackSpeed = knockbackSpeed - (dragForce + gravityKnockback);
    
        GetComponent<Rigidbody>().AddForce(knockbackSpeed * 0.1f, ForceMode.Impulse);
    
        Debug.Log(GetComponent<Rigidbody>().velocity);
    
        float knockbackDuration = (knockbackSpeed.magnitude * 0.1f) + 0.5f;
       
        if (knockbackDuration >= 5) knockbackDuration = 5;
    
        StartCoroutine(DisableMovementForKnockback(knockbackDuration));
    }

    IEnumerator DisableMovementForKnockback(float duration)
    {
        yield return new WaitForSeconds(duration);
    
        damagedOneTime = false;
        isHitted = false;
        knockbackSpeed = Vector3.zero;
    }

    public void ResetJump()
    {
        // Jump Reset
        sTop = 0;
        verticalSpeed = 0;
        canDJump = false;
        jOneTime = false;
        isDJumping = false;
        djOneTime = false;
        counterJump = 0;
        counterSwitchJump = 0;
        isJumping = false;
    }

    public void AfterLanding()
    {
        // Run reset
        isDashing = false;
        tractionBool = false;
        isRunning = false;
        isChangingDirTraction = false;
        speed = 0;
        tractionFrames = 0;
        framesHeld = 10;

        if (Mathf.Abs(Axis.x) > joystickThresholdMin)
        {
            speed = walkSpeed;
        }

        ResetJump();

        // Fast fall
        isFastFall = false;
        framesHeldY = 0;

        // Fall Platform
        canFallPlatform = false;
        isDownAxisY = false;

        hit.CancelAerialAttackAnim();
    }

    void Movement()
    {
        if (!hit.isN) HorzitonalMovement();
        Crouch();
        if (!hit.isN && !hit.isF && !hit.isU && !hit.isD
            && !hit.isFSmash && !hit.isUSmash && !hit.isDSmash) Jump();
        if (!hit.isN) Gravity();
        if (!hit.isN && !hit.isF && !hit.isU && !hit.isD
            && !hit.isFSmash && !hit.isUSmash && !hit.isDSmash) FallPlatform();
        if (!hit.isN && !hit.isF && !hit.isU && !hit.isD
            && !hit.isFSmash && !hit.isUSmash && !hit.isDSmash) FastFall();

        verticalSpeed = sTop - gravity;

        if (verticalSpeed <= -maxGravity && !isFastFall) verticalSpeed = -maxGravity;
        else if (isFastFall && !cb.isGrounded) verticalSpeed = -maxGravity - 5;

        //if (cb.isGrounded && isFastFall && !cb.isTouchingPlatform)
        //{
        //    isFastFall = false;
        //    verticalSpeed = 0;
        //}

        if (cb.isGrounded)
        {
            if (!tractionBool && isDashing)
            {
                if (dashTimeLeft > 0)
                {
                    // Solo mover si Axis.x no es 0
                    if (Mathf.Abs(Axis.x) > 0)
                    {
                        finalspeed = Mathf.Round(dashDirection) * dashSpeed;
                    }
                    dashTimeLeft -= Time.deltaTime;
                }
                else
                {
                    isDashing = false;
                    isRunning = true;
                }
            }
            else if (!tractionBool && !isDashing)
            {
                if (Mathf.Abs(Axis.x) >= joystickThresholdMax)
                {
                    tractionAxis = Axis.x;
                    finalspeed = Mathf.Round(Axis.x) * speed;
                }
                else finalspeed = Axis.x * speed;
            }
            else if (tractionBool && !isDashing) finalspeed = Mathf.Round(tractionAxis) * speed;
        }
        else if (!cb.isGrounded)
        {
            finalspeed = Axis.x * speed * 0.8f;
        }

        if (isTouchingWall && !isHitted) finalspeed = 0;
        // De momento no se toca esto.
        //else if (isTouchingWall && isHitted) GetComponent<Rigidbody>().AddForce(new Vector3(-1 * GetComponent<Rigidbody>().velocity.x, 0, 0), ForceMode.VelocityChange);

        GetComponent<Rigidbody>().velocity = new Vector3(finalspeed + platforMoving, verticalSpeed, 0);

        if (isHitted)
        {
            Knockback();

            if (cb.isGrounded)
            {
                isHitted = false;

                knockbackSpeed = Vector3.zero;
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
        }
    }
}

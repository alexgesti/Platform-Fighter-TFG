using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementBasis : MonoBehaviour
{
    public bool isSandBag;

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
    bool isJumping, isDJumping, canDJump;
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
    bool isCrouching;

    [HideInInspector] public CollisionBox cb;

    // Start is called before the first frame update
    void Start()
    {
        input.MoveEvent += HandleMove;
        input.JumpEvent += HandleJump;
        input.JumpCancelEvent += HandleCancelJump;

        cb = GetComponent<CollisionBox>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    // Input Readers
    void HandleMove(Vector2 dir)
    {
        Axis = dir;
    }

    void HandleJump()
    {
        if (cb.isGrounded && !isJumping) isJumping = true;

        if (canDJump) isDJumping = true;
    }

    void HandleCancelJump()
    {
        isJumping = false;
        isDJumping = false;
    }

    // Logic Movement
    void HorzitonalMovement()
    {
        //if (!isCrouching)
        //{
            if (cb.isGrounded) // Grounded
            {
                if (Mathf.Abs(Axis.x) > joystickThresholdMin && !tractionBool)
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
        if (Axis.y < -0.5f) isCrouching = true;
        else isCrouching = false;
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
            }
        }
        else if (canDJump)
        {
            if (isDJumping && !djOneTime)
            {
                verticalSpeed = 0;
                gravity = 0;
                sTop = speedFullHop;
                djOneTime = true;
                speedAirOneTime = false;

                isFastFall = false;
                framesHeldY = 0;
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
            if (gravity >= maxGravity && !isFastFall) gravity = maxGravity;
            else if (isFastFall) gravity = maxGravity + 5;
        }
        else gravity = 0;
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

   void FallPlatform()
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

    public void AfterLanding()
    {
        // Run reset
        isDashing = false;
        tractionBool = false;
        isRunning = false;
        isChangingDirTraction = false;
        tractionFrames = 0;
        framesHeld = 10;

        if (Mathf.Abs(Axis.x) > joystickThresholdMin)
        {
            speed = walkSpeed;
        }

        // Jump Reset
        sTop = 0;
        verticalSpeed = 0;
        canDJump = false;
        isDJumping = false;
        djOneTime = false;
        counterJump = 0;
        counterSwitchJump = 0;
        isJumping = false;

        // Fast fall
        isFastFall = false;
        framesHeldY = 0;

        // Fall Platform
        canFallPlatform = false;
        isDownAxisY = false;
    }

    void Movement()
    {
        HorzitonalMovement();
        Crouch();
        Jump();
        Gravity();
        FallPlatform();
        FastFall();
        
        verticalSpeed = sTop - gravity;

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
            finalspeed = Axis.x * speed;
        }

        GetComponent<Rigidbody>().velocity = new Vector3(finalspeed, verticalSpeed, 0);
        //Vector3 movement = new Vector3(finalspeed * Time.deltaTime, verticalSpeed * Time.deltaTime, 0);
        //
        //transform.Translate(movement);
    }
}

using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [Header("Stick Controlls (Pro Controller For Now)")]
    public InputReader input;
    [HideInInspector] public Vector2 Axis;

    [Header("Movement Horizontal")]
    public float walkSpeed;
    public float runSpeed;
    [HideInInspector] public float speed;

    [Header("Movement Vertical (Jump)")]
    public float weight;
    public float speedShortHop;
    public float speedFullHop;
    [HideInInspector] public float verticalSpeed, jFMFSTFHCounter, jFMFSJCounter, sTop;
    public float jumpFramesMaxForSwitchToFullHop, jumpFramesMaxForStartJump;
    float gravity;
    public float maxGravity;
    [HideInInspector] public bool isJumping, isInTheAirUp, isFastFall;
    bool dontJump;

    [Header("Movement Vertical (Double Jump)")]
    [HideInInspector] public bool djOneTime;
    bool canDoubleJump, isDJumping;

    // Jump conditions
    [HideInInspector] public bool pCanTraspass;

    [Header("RaycastPosition")]
    public Vector3 directionC;
    public float maxDistanceC;
    public static float maxDCCopy;
    [HideInInspector] public bool itTraspass;
    public bool raycastHitGround;
    GameObject obj;

    [Header("RaycastLedge")]
    public Vector3 directionS;
    public float maxDistanceS;

    [Header("Tapping")]
    float finalspeed;
    public float joystickThresholdMin;
    public float joystickThresholdMax;
    float framesHeld, prevThreshold;
    bool isRunning;
    public float requiredStickTimeX, requiredStickSpeedY;
    float stickExitTimeY, resultStickSpeedY;
    [HideInInspector] public bool DownAxisIsActive;

    [Header("HorizontalMovementValues")]
    public float tractionValue;
    [HideInInspector] public bool tractionBool, isChangingDirTraction;
    float tractionAxis, framesForTraction;
    // Hacer dash

    [HideInInspector] public EnvironmentCollisionBox ecb;

    void Start()
    {
        input.MoveEvent += HandleMove;
        input.JumpStartEvent += HandleStartJump;
        input.JumpEvent += HandleJump;
        input.JumpCancelEvent += HandleCancelJump;
        
        ecb = GetComponentInChildren<EnvironmentCollisionBox>();

        maxDCCopy = maxDistanceC;
        raycastHitGround = true;
    }
     
    private void Update()
    {
        Movement();
    }

    void LateUpdate()
    {
        PositionCorrection();
    }

    void HandleMove(Vector2 dir)
    {
        Axis = dir;
    }

    void Move()
    {
        //// Movement Horizontal (Basic) + Tapping.
        if (Mathf.Abs(Axis.x) > joystickThresholdMin && !tractionBool)
        {
            if (!isRunning) framesHeld++;

            if (Mathf.Abs(Axis.x) >= joystickThresholdMax)
            {
                if (framesHeld <= requiredStickTimeX) isRunning = true;
                else isRunning = false;

                if (isRunning) speed = runSpeed;
                else 
                {
                    speed = walkSpeed;
                }
            }
            else
            {
                speed = walkSpeed;
            }
        }
        else
        {
            if (isRunning)
            {
                tractionBool = true;
            }
            else if (speed == walkSpeed) speed = 0;
            
            if (speed > 0 && tractionBool)
            {
                speed -= tractionValue;
            }
            else if (speed <= 0 && tractionBool)
            {
                speed = 0;
                framesForTraction++;
                if (framesForTraction >= 3)
                {
                    tractionBool = false;
                    isChangingDirTraction = false;
                    framesForTraction = 0;
                }
            }

            isRunning = false;
            framesHeld = 0;
        }

        if (prevThreshold > joystickThresholdMin && Axis.x < -joystickThresholdMin || 
            prevThreshold < -joystickThresholdMin && Axis.x > joystickThresholdMin) 
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

        // Not falling when traction (stopping)
        StopOnLedge();

        // Stop when hit a wall
        if (ecb.isCollidingW && ecb.isRaycastHitW) // Revisar
        {
            speed = 0;
            if (ecb.wallR) transform.position = new Vector3 (ecb.objwall.position.x - ((ecb.objwall.localScale.x / 2) * 17) - (transform.localScale.x / 2), transform.position.y, transform.position.z);
            else if (!ecb.wallR) transform.position = new Vector3(ecb.objwall.position.x + ((ecb.objwall.localScale.x / 2) * 17) + (transform.localScale.x / 2), transform.position.y, transform.position.z);
        }
    }

    void HandleStartJump()
    {
        //isStartJumping = true;
    }

    void HandleJump()
    {
        if (ecb.isGrounded && !isJumping) 
            isJumping = true;

        if (canDoubleJump) isDJumping = true;
    }

    void HandleCancelJump()
    {
        isJumping = false;
        isDJumping = false;
    }

    void Jump()
    {
        // Movement Vertical (Jump)
        if (!ecb.isGrounded)
        {
            canDoubleJump = true;
            jFMFSTFHCounter = 0;
            jFMFSJCounter = 0;
        }

        if (!canDoubleJump && !dontJump) // Normal Jump
        {
            if (isJumping && ecb.isGrounded)
            {
                jFMFSTFHCounter++;
            }

            if (jFMFSTFHCounter > 0) jFMFSJCounter++;

            if (jFMFSJCounter >= jumpFramesMaxForStartJump)
            {
                if (jFMFSTFHCounter >= jumpFramesMaxForSwitchToFullHop && ecb.isGrounded && verticalSpeed == 0) // Full Jump
                {
                    sTop = speedFullHop;
                }
                else if (jFMFSTFHCounter < jumpFramesMaxForSwitchToFullHop && ecb.isGrounded && verticalSpeed == 0) // Short Jump
                {
                    sTop = speedShortHop;
                }

                dontJump = true; // Corrector. Aún no estoy seguro, pero había un bug raro que al no saltar y luego doble saltar y tocar platafroma traspasable se podía realizar otro salto (what).
                isInTheAirUp = true; // Corrector para las plataformas. Acción MUY especifica. Mirar en "EnvironmentCollisionBox" para tener más info.
            }
        }
        else if (canDoubleJump) // Double Jump
        { 
            if (isDJumping && !djOneTime)
            {
                verticalSpeed = 0;
                gravity = 0;
                sTop = speedFullHop;
                djOneTime = true;
                dontJump = true;
            }
        }

        // Reset
        if (verticalSpeed < 0) isInTheAirUp = false;

        if (ecb.isGrounded && verticalSpeed < 0) 
        {
            sTop = 0;
            verticalSpeed = 0.0f;
            gravity = 0;
            canDoubleJump = false;
            isDJumping = false;
            djOneTime = false;
            dontJump = false;
            if (!ecb.thatsAPlatform) isFastFall = false;

            if (jFMFSJCounter >= jumpFramesMaxForStartJump)
            {
                jFMFSTFHCounter = 0;
                jFMFSJCounter = 0;
            }

            if (isJumping)
            {
                isJumping = false;
            }
        }

        // Gravity/Fall
        if (!ecb.isGrounded)
        {
            gravity += weight * 9.81f * Time.deltaTime;
            if (gravity >= maxGravity) gravity = maxGravity;
            
            if (isFastFall) gravity = maxGravity + 5;

            if (!isJumping && gravity < speedFullHop && !dontJump)
            {
                sTop = speedFullHop;
                gravity = speedFullHop;
            }
        }
        else gravity = 0; // Frenado para cuando unicamente esta en el suelo.

        verticalSpeed = sTop - gravity;

        //Debug.Log("Grounded " + ecb.isGrounded);
        //Debug.Log("Bool " + isJumping);
        //Debug.Log("TimePress " + jbCounter);
        //Debug.Log("TimeFrames " + jfCounter);
        //Debug.Log("Speed " + verticalSpeed);
        //Debug.Log("G " + gravity);

        // Fall from platform
        if (ecb.isGrounded && ecb.canTraspassP)
        {
            if (Axis.y < -joystickThresholdMin)
            {
                if (Axis.y <= -joystickThresholdMax)
                {
                    if (resultStickSpeedY == 0) resultStickSpeedY = Mathf.Abs((joystickThresholdMax - joystickThresholdMin) / (Time.time - stickExitTimeY));
                    if (requiredStickSpeedY >= resultStickSpeedY && !djOneTime)
                    {
                        ecb.isGoingToTraspassP = true;
                        ecb.isGrounded = false;
                        DownAxisIsActive = true;
                    }
                }
            }
            else resultStickSpeedY = 0;
        }

        if (ecb.isGoingToTraspassP && djOneTime && verticalSpeed < 0) // Corrección platafroma con doble salto.
        {
            ecb.isGoingToTraspassP = false;
            ecb.canTraspassP = true;
        }
        
        if (isFastFall && ecb.thatsAPlatform) // Hace que no se pare en seco en la plataforma (¿?, hay que comprobarlo)
        {
            ecb.isGoingToTraspassP = true;
            ecb.canTraspassP = false;
        }

        // Fast fall
        if (!ecb.isGrounded && verticalSpeed < 0)
        {
            if (Axis.y < -joystickThresholdMin)
            {
                if (Axis.y <= -joystickThresholdMax)
                {
                    if (resultStickSpeedY == 0) resultStickSpeedY = Mathf.Abs((joystickThresholdMax - joystickThresholdMin) / (Time.time - stickExitTimeY));
                    if (requiredStickSpeedY >= resultStickSpeedY && !DownAxisIsActive)
                    {
                        isFastFall = true;
                    }
                }
            }
            else DownAxisIsActive = false;
        }
    }

    void Movement()
    {
        Move();
        Jump();

        if (!tractionBool) // Hay que poner que no sea justo en el mismo momento de correr. 
        {
            if (Mathf.Abs(Axis.x) >= joystickThresholdMax) tractionAxis = Axis.x;
            finalspeed = Axis.x * speed;
        }
        else if (tractionBool && ecb.isGrounded && !isJumping)
        {
            finalspeed = tractionAxis * speed;
        }

        Vector3 movement = new Vector3(finalspeed * Time.deltaTime, verticalSpeed * Time.deltaTime, 0);

        //Debug.Log(tractionBool);
        //Debug.Log("Axis traction: " + tractionAxis);
        //Debug.Log("Stick speed: " + resultStickSpeed + " m/s");
        //Debug.Log("Axis X: " + Axis.x);
        //Debug.Log("Speed: " + speed + " m/s");
        //Debug.Log("Actual movement speed: " + movement + " m/s");

        transform.Translate(movement);
    }

    void PositionCorrection()
    {
        // Raycast part
        RaycastHit hit;

        if (Physics.Raycast(transform.position - new Vector3(0, transform.localScale.y / 2, 0), directionC, out hit, maxDistanceC))
        {
            if (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "PlatformF")
            {
                if (!pCanTraspass)
                {
                    itTraspass = true;
                    raycastHitGround = true;
                    obj = hit.collider.gameObject;
                }
                if (isFastFall) raycastHitGround = true;
            }
        }
        else raycastHitGround = false;

        Debug.DrawRay(transform.position - new Vector3(0, transform.localScale.y / 2, 0), directionC * maxDistanceC, Color.green);
    
        // Position correction
        if (!isJumping && ecb.isGrounded && itTraspass)
        {
            transform.position =
            new Vector3(transform.position.x,
            obj.transform.position.y + obj.transform.localScale.y / 2 + transform.localScale.y,
            transform.position.z);

            itTraspass = false;
        }
    }

    void StopOnLedge()
    {
        if (Axis.x > 0) directionS = new Vector3(0.1f, -1, 0);
        else if (Axis.x < 0) directionS = new Vector3(-0.1f, -1, 0);

        //Raycast part
        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionS, out hit, maxDistanceS))
        {
            if (hit.collider.gameObject.tag == "Stop")
            {
                if (tractionBool && ecb.isGrounded) speed = 0;
            }
        }

        Debug.DrawRay(transform.position, directionS * maxDistanceS, Color.red);
    }
}

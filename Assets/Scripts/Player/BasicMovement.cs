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
    [HideInInspector] public float verticalSpeed;
    float jFMFSTFHCounter, jFMFSJCounter, sTop;
    public float jumpFramesMaxForSwitchToFullHop, jumpFramesMaxForStartJump;
    float gravity;
    [HideInInspector] public bool isJumping, isInTheAirUp;
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
    public bool itTraspass;
    GameObject obj;

    [Header("RaycastLedge")]
    public Vector3 directionS;
    public float maxDistanceS;
    bool cantFall;

    [Header("Tapping")]
    public float joystickThresholdMin;
    public float joystickThresholdMax;
    float framesHeld;
    bool isRunning, wasWalking;
    public float requiredStickTimeX, requiredStickSpeedY;
    float stickExitTimeX, resultStickSpeedX, stickExitTimeY, resultStickSpeedY;

    [HideInInspector] public EnvironmentCollisionBox ecb;

    void Start()
    {
        input.MoveEvent += HandleMove;
        input.JumpStartEvent += HandleStartJump;
        input.JumpEvent += HandleJump;
        input.JumpCancelEvent += HandleCancelJump;
        
        ecb = GetComponentInChildren<EnvironmentCollisionBox>();
        //jumpForceConst = jumpForce;

        maxDCCopy = maxDistanceC;
    }
     
    private void Update()
    {
        StopOnLedge();
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

    void Move() // No acaba de funcionar cuando el jugador coge de andar (al max) y pasa al correr
                // (parece que debería de detectar el frame medio todo el rato pero hay frames que se salta)
    {
        Debug.Log(Axis.x);

        //// Movement Horizontal (Basic) + Tapping. Semi resuelto, hay aún hay que ajustar un poco para que acabe de funcionar.
        if (Mathf.Abs(Axis.x) > joystickThresholdMin)
        {
            if (!isRunning) framesHeld++;

            //if (stickExitTimeX == 0) stickExitTimeX = Time.time;

            if (Mathf.Abs(Axis.x) >= joystickThresholdMax)
            {
                if (framesHeld <= requiredStickTimeX) isRunning = true;
                else isRunning = false;

                if (isRunning) speed = runSpeed;
                else
                {
                    speed = walkSpeed;
                    wasWalking = true;
                }
            }
            else
            {
                speed = walkSpeed;
            }
        }
        else
        {
            speed = 0;
            isRunning = false;
            framesHeld = 0;
        }

        if (Mathf.Abs(Axis.x) < joystickThresholdMax && wasWalking) // Funciona solo para umbral maximo, menor no acaba de funcionar. Revisar.
        {
            framesHeld = 0;
            wasWalking = false;
        }

        // Not falling when Walking
        //if (cantFall && speed == walkSpeed && ecb.isGrounded) speed = 0;
        //else if (cantFall && (speed == runSpeed || !ecb.isGrounded)) cantFall = false;

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
        if (!ecb.isGrounded) canDoubleJump = true; 

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

            if (jFMFSJCounter >= jumpFramesMaxForStartJump)
            {
                jFMFSTFHCounter = 0;
                jFMFSJCounter = 0;
                if (isJumping)
                {
                    isJumping = false;
                }
            }
        }

        // Gravity/Fall
        if (!ecb.isGrounded) gravity += weight * 9.81f * Time.deltaTime; 
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
                    }
                }
            }
            else
            {
                resultStickSpeedY = 0;
            }
        }

        if (ecb.isGoingToTraspassP && djOneTime && verticalSpeed < 0) // Corrección platafroma con doble salto.
        {
            ecb.isGoingToTraspassP = false;
            ecb.canTraspassP = true;
        }
    }

    void Movement()
    {
        Move();
        Jump();

        Vector3 movement = new Vector3(Axis.x * speed * Time.deltaTime, verticalSpeed * Time.deltaTime, 0);

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

        if (Physics.Raycast(transform.position - new Vector3 (0, transform.localScale.y / 2, 0), directionC, out hit, maxDistanceC))
        {
            if (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "PlatformF")
            {
                if (!pCanTraspass)
                {
                    itTraspass = true;
                    obj = hit.collider.gameObject;
                }
            }
        }

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
                if (speed == walkSpeed && ecb.isGrounded) cantFall = true;
            }
            else cantFall = false;
        }

        Debug.DrawRay(transform.position, directionS * maxDistanceS, Color.red);
    }
}

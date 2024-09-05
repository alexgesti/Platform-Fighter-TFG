using ProBuilder2.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class HitBoxesController : MonoBehaviour
{
    [HideInInspector] public List<BoxCollider> hitBoxes = new List<BoxCollider>();
    [HideInInspector] public List<Transform> bonesRef = new List<Transform>();
   
    [Header("Controller")]
    [HideInInspector] public bool canAir;
    [HideInInspector] public bool isN, isF, isD, isU, isNAir, isFAir, isBAir, isDAir, isUAir;
    [HideInInspector] public bool isN2, isN3, isNRepeat;
    [HideInInspector] public int neutralIndex;
    [HideInInspector] public bool isFSmash, isDSmash, isUSmash, canSmash;
    [HideInInspector] public float framesHeld, framesMaxCharge, multiplySmashAttack;
    public float requiredStickTimeX, requiredStickTimeY, maxSmashTime;

    MovementBasis player;

    // Start is called before the first frame update
    void Awake()
    {
        BoxCollider[] childBox = GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider box in childBox)
        {
            if (box.gameObject.layer == LayerMask.NameToLayer("HitBoxes"))
            {
                box.gameObject.AddComponent<HitBoxesState>();
                hitBoxes.Add(box);
            }
        }

        Transform[] childBones = GetComponentsInChildren<Transform>();

        foreach (Transform bones in childBones)
        {
            if (bones.gameObject.CompareTag("PlayerSqueleton"))
            {
                bonesRef.Add(bones.transform);
            }
        }

        foreach (Transform reference in bonesRef)
        {
            foreach (BoxCollider state in hitBoxes)
            {
                if (reference.gameObject.name == state.gameObject.name.Replace("_Box", ""))
                {
                    state.gameObject.GetComponent<HitBoxesState>().reference = reference;
                    state.gameObject.GetComponent<HitBoxesState>().father = this.transform;
                }
            }
        }
    }

    private void Start()
    {
        player = GetComponentInParent<MovementBasis>();

        if (!player.isSandBag)
        {
            player.input.AttackStartEvent += HandleAttackStart;
            player.input.AttackCancelEvent += HandleAttackCancel;
        }
    }

    private void Update()
    {
        //if (attackButton) GetComponentInParent<MovementBasis>().ResetJump();

        HandleNeutralAttack();
        HandleSmashAttacks();
        SaveEnemyBox();
    }

    private void LateUpdate()
    {
        HandleAttackState();
    }

    void HandleAttackStart(int eventPlayerID)
    {
        if (eventPlayerID == player.playerID)
        {
            if (player.cb.isGrounded &&
                !player.isJumping)
            {
                if (!isDSmash && !isFSmash && !isUSmash && !canSmash && !player.isBusy)
                {
                    if (!isF && !isD && !isU)
                    {
                        if (GetComponentInParent<Rigidbody>().velocity == new Vector3(0, 0, 0) &&
                            player.Axis.y < player.joystickThresholdMin &&
                            player.Axis.y > -player.joystickThresholdMin)
                        {
                            isN = true;
                            isNRepeat = true;
                        }

                        if (isN && neutralIndex == 1) //ATENCIÓN. Si se puede poner que si se mantiene pulsado repita el jab1 todo el rato (si se puede, lo he probado y no acaba de funcionar).
                            isN2 = true;

                        if (isN && neutralIndex >= 2)
                            isN3 = true;
                    }

                    if (!isN && !isU && !isD)
                    {
                        if (GetComponentInParent<Rigidbody>().velocity.x != 0 &&
                            GetComponentInParent<Rigidbody>().velocity.y == 0 &&
                            player.Axis.y < player.joystickThresholdMin &&
                            player.Axis.y > -player.joystickThresholdMin)
                            isF = true;
                    }

                    if (!isN && !isF && !isD)
                    {
                        if (GetComponentInParent<Rigidbody>().velocity.y == 0 &&
                            player.Axis.y >= player.joystickThresholdMin)
                            isU = true;
                    }

                    if (!isN && !isF && !isU)
                    {
                        if (player.isCrouching)
                            isD = true;
                    }
                }

                // Smash Attacks
                if (!isF && !isD && !isU && !isN)
                {
                    if (!isDSmash && !isFSmash)
                        if (canSmash && player.Axis.y >= player.joystickThresholdMax)
                            isUSmash = true;

                    if (!isUSmash && !isFSmash)
                        if (canSmash && player.Axis.y <= -player.joystickThresholdMax)
                            isDSmash = true;

                    if (!isUSmash && !isDSmash)
                        if (canSmash && Mathf.Abs(player.Axis.x) >= player.joystickThresholdMin)
                            isFSmash = true;
                }
            }
            else if (!player.cb.isGrounded && canAir)
            {
                if (!isDAir && !isFAir && !isBAir && !isUAir)
                    if (player.Axis.x == 0 && player.Axis.y == 0)
                        isNAir = true;

                if (!isNAir && !isFAir && !isBAir && !isUAir)
                    if (player.Axis.y <= -player.joystickThresholdMin)
                        isDAir = true;

                if (!isNAir && !isFAir && !isBAir && !isDAir)
                    if (player.Axis.y >= player.joystickThresholdMin)
                        isUAir = true;

                if (!isNAir && !isUAir && !isBAir && !isDAir)
                    if ((transform.localEulerAngles.y == 90 && player.Axis.x >= player.joystickThresholdMin)
                        || (transform.localEulerAngles.y == 270 && player.Axis.x <= -player.joystickThresholdMin))
                        isFAir = true;

                if (!isNAir && !isUAir && !isFAir && !isDAir)
                    if ((transform.localEulerAngles.y == 90 && player.Axis.x <= -player.joystickThresholdMin)
                        || (transform.localEulerAngles.y == 270 && player.Axis.x >= player.joystickThresholdMin))
                        isBAir = true;
            }
        }
    }

    void HandleNeutralAttack()
    {
        foreach (BoxCollider state in hitBoxes)
        {
            if (isN) neutralIndex = state.GetComponent<HitBoxesState>().indexNeutral;
            else if (!isN && state.GetComponent<HitBoxesState>().indexNeutral != 0) state.GetComponent<HitBoxesState>().indexNeutral = 0;
        }
    }

    void HandleAttackState()
    {
        if (isN || isF || isU)
        {
            isD = false;
            player.isCrouching = false;
        }

        if (player.isCrouching)
        {
            isN = false;
            isF = false;
            isU = false;
        }
        else
            isD = false;
    }

    void HandleAttackCancel(int eventPlayerID)
    {
        if (eventPlayerID == player.playerID)
        {
            if (isNRepeat) isNRepeat = false;

            if (isUSmash) isUSmash = false;
            else if (isDSmash) isDSmash = false;
            else if (isFSmash) isFSmash = false;
        }
    }

    void HandleSmashAttacks()
    {
        if (Mathf.Abs(player.Axis.y) >= player.joystickThresholdMin)
        {
            if (!canSmash) framesHeld++;

            if (Mathf.Abs(player.Axis.y) >= player.joystickThresholdMax && 
                framesHeld <= requiredStickTimeY && !canSmash)
                canSmash = true;
        }

        if (Mathf.Abs(player.Axis.x) >= player.joystickThresholdMin)
        {
            if (!canSmash) framesHeld++;

            if (Mathf.Abs(player.Axis.x) >= player.joystickThresholdMax &&
                framesHeld <= requiredStickTimeX && !canSmash)
                canSmash = true;
        }

        if (canSmash && (isUSmash || isDSmash || isFSmash))
        {
            framesMaxCharge++;

            multiplySmashAttack = framesMaxCharge;

            if (framesMaxCharge >= maxSmashTime)
            {
                isUSmash = false;
                isDSmash = false;
                isFSmash = false;
                canSmash = false;
            }
        }
        else if (canSmash && !isUSmash && !isDSmash && !isFSmash)
        {
            if (framesHeld < requiredStickTimeY + 5) framesHeld++;
            else
                canSmash = false;
        }
        else
        {
            if (Mathf.Abs(player.Axis.y) < player.joystickThresholdMin &&
                Mathf.Abs(player.Axis.x) < player.joystickThresholdMin) 
                framesHeld = 0;

            framesMaxCharge = 0;
        }
    }

    void CancelAttackAnim()
    {
        isN = false;
        isN2 = false;
        isN3 = false;
        neutralIndex = 0;
        isF = false;
        isD = false;
        isU = false;
        isNAir = false;
        isFAir = false;
        isBAir = false;
        isDAir = false;
        isUAir = false;
        isFSmash = false;
        isDSmash = false;
        isUSmash = false;
        player.isBusy = false;
    }

    public void CancelAttackForFallAnim()
    {
        if (isN || isF || isD || isU)
            GetComponent<Animator>().SetTrigger("cancelAnimationAttackForFall");

        isN = false;
        isN2 = false;
        isN3 = false;
        neutralIndex = 0;
        isF = false;
        isD = false;
        isU = false;
        isFSmash = false;
        isDSmash = false;
        isUSmash = false;
    }

    public void CancelAerialAttackAnim()
    {
        if (isNAir || isFAir || isBAir || isDAir || isUAir)
            GetComponent<Animator>().SetTrigger("cancelAnimationAttackAir");

        isNAir = false;
        isFAir = false;
        isBAir = false;
        isDAir = false;
        isUAir = false;
    }

    void CancelAllHitboxesActives()
    {
        foreach (BoxCollider state in hitBoxes)
        {
            if (state.gameObject.GetComponent<HitBoxesState>().indexState == 1)
            {
                state.gameObject.GetComponent<HitBoxesState>().Vulnerable();
                state.gameObject.GetComponent<HitBoxesState>().scale = Vector3.zero;
                state.gameObject.GetComponent<HitBoxesState>().launchAngle = 0;
                state.gameObject.GetComponent<HitBoxesState>().launchSpeed = 0;
                state.gameObject.GetComponent<HitBoxesState>().damage = 0;
            }
        }
    }

    void FinishAnimBeforeCrouch(int i)
    {
        if (i == 1) player.isBusy = true;
        else if (i == 0)
        {
            player.isBusy = false;
            multiplySmashAttack = 0;
        }
    }

    void SaveEnemyBox()
    {
        foreach (BoxCollider state in hitBoxes)
        {
            if (state.GetComponent<HitBoxesState>().enemyCollider != null)
            {
                Transform enemyParent = state.GetComponent<HitBoxesState>().enemyCollider.transform;
                MovementBasis enemyState = null; 

                while (enemyParent != null)
                {
                    enemyState = enemyParent.GetComponent<MovementBasis>();

                    if (enemyState != null)
                    {
                        break;
                    }

                    enemyParent = enemyParent.parent;
                }

                if (state != null)
                {
                    if (!enemyState.isHitted)
                    {
                        enemyState.launchSpeed = state.GetComponent<HitBoxesState>().launchSpeed + ((framesMaxCharge / 10) * 2);
                        enemyState.launchAngle = state.GetComponent<HitBoxesState>().launchAngle;
                        enemyState.damage = state.GetComponent<HitBoxesState>().damage + (((int)framesMaxCharge / 10) * 2);
                        enemyState.isHitted = true;
                        enemyState.knockbackBool = true;
                        if (transform.eulerAngles.y < 180) enemyState.direction = 1;
                        else enemyState.direction = -1;
                    }
                }
            }
        }
    }
}

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
    public InputReader input;
    [HideInInspector] public bool canAir;
    [HideInInspector] public bool isN, isF, isD, isU, isNAir, isFAir, isDAir, isUAir;
    [HideInInspector] public bool isN2, isN3, isNRepeat;
    [HideInInspector] public int neutralIndex;

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
            input.AttackStartEvent += HandleAttackStart;
            input.AttackCancelEvent += HandleAttackCancel;
        }
    }

    private void Update()
    {
        //if (attackButton) GetComponentInParent<MovementBasis>().ResetJump();

        HandleNeutralAttack();
        SaveEnemyBox();
    }

    private void LateUpdate()
    {
        HandleAttackState();
    }

    void HandleAttackStart()
    {
        if (player.cb.isGrounded &&
            !player.isJumping)
        {
            if (!isF && !isD && !isU)
            {
                if (GetComponentInParent<Rigidbody>().velocity == new Vector3(0, 0, 0) &&
                    player.Axis.y < player.joystickThresholdMin &&
                    player.Axis.y > -player.joystickThresholdMin)
                {
                    isN = true;
                    isNRepeat = true;
                    //player.isBusy = false;
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
                    player.Axis.y > -player.joystickThresholdMin) // ATENCIÓN. Probar con los mandos.
                    isF = true;
            }

            if (!isN && !isF && !isD)
            {
                if (GetComponentInParent<Rigidbody>().velocity.y == 0 &&
                    player.Axis.y >= player.joystickThresholdMin) // ATENCIÓN. Probar con los mandos.
                    isU = true;
            }

            if (!isN && !isF && !isU)
            {
                if (player.isCrouching)
                    isD = true;
            }
        }
        else if (!player.cb.isGrounded && canAir)
        {
            if (player.Axis.x == 0 && player.Axis.y == 0)
                isNAir = true;
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

    void HandleAttackCancel()
    {
        if (isNRepeat) isNRepeat = false;
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
        isDAir = false;
        isUAir = false;
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
    }

    public void CancelAerialAttackAnim()
    {
        if (isNAir || isFAir || isDAir || isUAir)
            GetComponent<Animator>().SetTrigger("cancelAnimationAttackAir");

        isNAir = false;
        isFAir = false;
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
                    enemyState.knockbackBool = true;
                    enemyState.launchSpeed = state.GetComponent<HitBoxesState>().launchSpeed;
                    enemyState.launchAngle = state.GetComponent<HitBoxesState>().launchAngle;
                    enemyState.damage = state.GetComponent<HitBoxesState>().damage;
                    enemyState.isHitted = true;

                    if (transform.eulerAngles.y < 180) enemyState.direction = 1;
                    else enemyState.direction = -1;
                }
            }
        }
    }
}

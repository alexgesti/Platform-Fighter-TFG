using ProBuilder2.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxesController : MonoBehaviour
{
    [HideInInspector] public List<BoxCollider> hitBoxes = new List<BoxCollider>();
    [HideInInspector] public List<Transform> bonesRef = new List<Transform>();
   
    [Header("Controller")]
    public InputReader input;
    [HideInInspector] public bool attackButton;

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
        if (!GetComponentInParent<MovementBasis>().isSandBag)
        {
            input.AttackStartEvent += HandleAttackStart;
            input.AttackCancelEvent += HandleAttackCancel;
        }
    }

    private void Update()
    {
        if (attackButton) GetComponentInParent<MovementBasis>().ResetJump();

        SaveEnemyBox();
    }

    void HandleAttackStart()
    {
        if (GetComponentInParent<MovementBasis>().cb.isGrounded &&
            !GetComponentInParent<MovementBasis>().isJumping &&
            GetComponentInParent<Rigidbody>().velocity == new Vector3(0, 0, 0)) // Jab
        {
            attackButton = true;
        }
        else attackButton = false;
    }

    void HandleAttackCancel()
    {
        if (GetComponentInParent<MovementBasis>().isJumping) attackButton = false;
    }

    void CancelAttackAnim()
    {
        attackButton = false;
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

                    if (transform.eulerAngles.y < 180) enemyState.direction = 1;
                    else enemyState.direction = -1;
                }
            }
        }
    }
}

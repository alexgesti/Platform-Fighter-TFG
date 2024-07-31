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
                }
            }
        }
    }

    private void Start()
    {
        input.AttackStartEvent += HandleAttackStart;
        input.AttackCancelEvent += HandleAttackCancel;
    }

    private void Update()
    {
        if (attackButton) GetComponentInParent<MovementBasis>().ResetJump(); // Corrige pulsar puño y saltar, pero no al reves (practicamente al mismo tiempo).
    }

    void HandleAttackStart()
    {
        if (GetComponentInParent<MovementBasis>().cb.isGrounded && 
            !GetComponentInParent<MovementBasis>().isJumping)
        {
            attackButton = true;
        }
    }

    void HandleAttackCancel()
    {
        //attackButton = false;
    }

    void CancelAttackAnim()
    {
        attackButton = false;
    }
}

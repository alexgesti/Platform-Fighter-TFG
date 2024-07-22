using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxesState : MonoBehaviour
{
    [HideInInspector] public int indexState; // 0 = Vulnerable; 1 = DamageAction; -1 = Invulnerable
    [HideInInspector] public Vector3 scale;
    [HideInInspector] public Transform reference;
    [HideInInspector] public Color color; // Yellow = Vulnerable; Red = DamageAction; Green = Invulnerable

    BoxCollider bCollider;
    MeshRenderer rend;
    MeshFilter filter;

    private void Start()
    {
        indexState = 0;

        transform.position = reference.position;
        transform.rotation = reference.rotation;
        transform.localScale = reference.localScale + scale;

        color = Color.yellow;

        bCollider = GetComponent<BoxCollider>();
        rend = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();

        AdjustMeshToBoxCollider();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localPosition = reference.localPosition;
        transform.localRotation = reference.localRotation;
        transform.localScale = reference.localScale + scale;
    }

    void AdjustMeshToBoxCollider()
    {
        filter.transform.localScale = bCollider.size;
        filter.transform.localPosition = bCollider.center;
    }
}

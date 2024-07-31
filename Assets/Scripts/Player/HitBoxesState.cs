using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxesState : MonoBehaviour
{
    [HideInInspector] public int indexState; 
    Vector3 ogScale;
    [HideInInspector] public Vector3 scale;
    [HideInInspector] public Transform reference;
    [HideInInspector] public MeshRenderer mesh;

    private void Start()
    {
        indexState = 0;

        ogScale = transform.localScale;

        transform.localScale = ogScale;

        mesh = GetComponent<MeshRenderer>();
        mesh.material.color = new Color(1, 0.92f, 0.016f, 0.6f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (reference.localScale.x > 1 ||
            reference.localScale.y > 1 ||
            reference.localScale.z > 1 ) 
            transform.localScale = scale + ogScale;
        else transform.localScale = ogScale;
    }

    public void Vulnerable()
    {
        indexState = 0;

        mesh.material.color = new Color(1, 0.92f, 0.016f, 0.6f);
    }

    public void DamageAction()
    {
        indexState = 1;

        mesh.material.color = new Color(1, 0, 0, 0.6f);
    }

    public void Invulnerable()
    {
        indexState = 2;

        mesh.material.color = new Color(0, 1, 0, 0.6f);
    }
}

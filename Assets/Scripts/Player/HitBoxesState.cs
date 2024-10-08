using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxesState : MonoBehaviour
{
    [HideInInspector] public int indexState;
    [HideInInspector] public BoxCollider enemyCollider;
    Vector3 ogScale;
    [HideInInspector] public Vector3 scale;
    [HideInInspector] public Transform reference;
    [HideInInspector] public Transform father;
    [HideInInspector] public MeshRenderer mesh;

    [HideInInspector] public float launchSpeed, launchAngle;
    [HideInInspector] public int damage;

    [HideInInspector] public int indexNeutral;

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
        if (!GetComponentInParent<MovementBasis>().isSandBag)
        {
            if (reference.localScale.x > ogScale.x ||
                reference.localScale.y > ogScale.y ||
                reference.localScale.z > ogScale.z)
                transform.localScale = scale + ogScale;
            else transform.localScale = ogScale;
        }

        CheckCollision();
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

    void CheckCollision()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        Collider[] hitColliders = Physics.OverlapBox(box.transform.TransformPoint(box.center), 
            box.size / 2, box.transform.rotation, LayerMask.GetMask("HitBoxes"));

        // Test
        bool collisionFound = false;

        foreach (BoxCollider hitCollider in hitColliders)
        {
            HitBoxesState hitBoxState = hitCollider.GetComponent<HitBoxesState>();

            if (hitCollider != box && hitBoxState.father != father)
            {
                if (indexState == 1 && hitBoxState.indexState == 0)
                {
                    enemyCollider = hitCollider;
                    collisionFound = true;
                    break;
                }
            }
        }

        if (!collisionFound) enemyCollider = null;

        //bool noCollisions = true;

        //foreach (BoxCollider hitCollider in hitColliders)
        //{
        //    if (hitCollider != box && hitCollider.GetComponent<HitBoxesState>().father != father)
        //    {
        //        if (indexState == 1 && hitCollider.GetComponent<HitBoxesState>().indexState == 0)
        //        {
        //            noCollisions = false;
        //            enemyCollider = hitCollider;
        //        }
        //    }
        //}
        //
        //if (noCollisions && enemyCollider != null)
        //{
        //    enemyCollider = null;
        //}
    }
}

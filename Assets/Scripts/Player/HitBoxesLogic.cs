using ProBuilder2.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HitBoxesLogic : MonoBehaviour
{
    [HideInInspector] public List<BoxCollider> hitBoxes = new List<BoxCollider>();
    [HideInInspector] public List<Transform> bonesRef = new List<Transform>();
    int indexState = 0; 

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
                if (reference.gameObject.name ==  state.gameObject.name.Replace("_Box", ""))
                {
                    state.gameObject.GetComponent<HitBoxesState>().reference = reference;
                }
            }
        }

        //BoxCollider[] childBox = GetComponentsInChildren<BoxCollider>();
        //
        //foreach (BoxCollider collider in childBox)
        //{
        //    if (collider.gameObject.layer == LayerMask.NameToLayer("HitBoxes"))
        //    {
        //        hitBoxes.Add(collider);
        //    }
        //}
        //
        //MeshRenderer[] childRend = GetComponentsInChildren<MeshRenderer>();
        //
        //foreach (MeshRenderer rend in childRend)
        //{
        //    if (rend.gameObject.layer == LayerMask.NameToLayer("HitBoxes"))
        //    {
        //        hitBVis.Add(rend);
        //
        //        string baseName = rend.gameObject.name.Replace("_Vis", "");
        //
        //        foreach (BoxCollider box in hitBoxes)
        //        {
        //            if (box.gameObject.name == baseName)
        //            {
        //                rend.transform.localPosition = box.center;
        //                rend.transform.localScale = box.size;
        //                break;
        //            }
        //        }
        //
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Jab1(int i)
    {
        if (i == 1) Debug.Log("Active");
        else if (i == 0) Debug.Log("Deactived");
    }

    void Jab2(int i)
    {
        if (i == 1) Debug.Log("Active");
        else if (i == 0) Debug.Log("Deactived");
    }
}

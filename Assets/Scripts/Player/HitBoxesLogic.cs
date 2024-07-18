using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxesLogic : MonoBehaviour
{
    [HideInInspector] public List<BoxCollider> hitBoxes = new List<BoxCollider>();
    [HideInInspector] public List<MeshRenderer> hitBVis = new List<MeshRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider[] childBox = GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider collider in childBox)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("HitBoxes"))
                hitBoxes.Add(collider);
        }

        MeshRenderer[] childRend = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer rend in childRend)
        {
            if (rend.gameObject.layer == LayerMask.NameToLayer("HitBoxes"))
            {
                hitBVis.Add(rend);

                string baseName = rend.gameObject.name.Replace("_Vis", "");

                foreach (BoxCollider box in hitBoxes)
                {
                    if (box.gameObject.name == baseName)
                    {
                        rend.transform.localPosition = box.center;
                        rend.transform.localScale = box.size;
                        break;
                    }
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

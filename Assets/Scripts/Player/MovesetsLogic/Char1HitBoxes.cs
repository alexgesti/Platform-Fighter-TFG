using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char1HitBoxes : MonoBehaviour
{
    HitBoxesController state;

    private void Start()
    {
        state = GetComponent<HitBoxesController>();
    }

    void Jab1(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "H_L_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.15f, 0.15f, 0.15f);
                }
                else if (i == 0)
                {
                    hit.GetComponent<HitBoxesState>().Vulnerable();
                    hit.GetComponent<HitBoxesState>().scale = Vector3.zero;
                }
            }
        }
    }

    void Jab2(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "H_R_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.15f, 0.15f, 0.15f);
                }
                else if (i == 0)
                {
                    hit.GetComponent<HitBoxesState>().Vulnerable();
                    hit.GetComponent<HitBoxesState>().scale = Vector3.zero;
                }
            }
        }
    }
}

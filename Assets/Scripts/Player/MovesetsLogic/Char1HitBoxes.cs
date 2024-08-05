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
                    hit.GetComponent<HitBoxesState>().launchAngle = 45;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 20;
                    hit.GetComponent<HitBoxesState>().damage = 10;
                }
                else if (i == 0)
                {
                    hit.GetComponent<HitBoxesState>().Vulnerable();
                    hit.GetComponent<HitBoxesState>().scale = Vector3.zero;
                    hit.GetComponent<HitBoxesState>().launchAngle = 0;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 0;
                    hit.GetComponent<HitBoxesState>().damage = 0;
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
                    hit.GetComponent<HitBoxesState>().launchAngle = 45;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 20;
                    hit.GetComponent<HitBoxesState>().damage = 10;
                }
                else if (i == 0)
                {
                    hit.GetComponent<HitBoxesState>().Vulnerable();
                    hit.GetComponent<HitBoxesState>().scale = Vector3.zero;
                    hit.GetComponent<HitBoxesState>().launchAngle = 0;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 0;
                    hit.GetComponent<HitBoxesState>().damage = 0;
                }
            }
        }
    }
}

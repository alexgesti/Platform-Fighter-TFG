using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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
            if (hit.name == "H_R_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.15f, 0.15f, 0.15f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 5;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 5;
                    hit.GetComponent<HitBoxesState>().damage = 3;
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

            if (hit.name == "A_R_2_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.1f, 0.1f, 0.1f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 2;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 1;
                    hit.GetComponent<HitBoxesState>().damage = 2;
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

            if (hit.name == "A_R_1_Box")
            {
                if (i == 1) 
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.1f, 0.1f, 0.1f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 2;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 1;
                    hit.GetComponent<HitBoxesState>().damage = 2;
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
            if (hit.name == "H_L_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.15f, 0.15f, 0.15f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 5;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 5;
                    hit.GetComponent<HitBoxesState>().damage = 3;
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

            if (hit.name == "A_L_2_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.1f, 0.1f, 0.1f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 2;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 1;
                    hit.GetComponent<HitBoxesState>().damage = 2;
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

            if (hit.name == "A_L_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.1f, 0.1f, 0.1f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 2;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 1;
                    hit.GetComponent<HitBoxesState>().damage = 2;
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

    void Jab3(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "H_R_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.35f, 0.35f, 0.35f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 25;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 20;
                    hit.GetComponent<HitBoxesState>().damage = 6;
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

            if (hit.name == "A_R_2_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.15f, 0.15f, 0.15f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 13;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 10;
                    hit.GetComponent<HitBoxesState>().damage = 3;
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

            if (hit.name == "A_R_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.15f, 0.15f, 0.15f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 13;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 10;
                    hit.GetComponent<HitBoxesState>().damage = 3;
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

    void NextJab(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            hit.GetComponent<HitBoxesState>().indexNeutral = i;
        }
    }

    void Forward(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "L_L_3_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.45f, 0.45f, 0.45f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 20;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 40;
                    hit.GetComponent<HitBoxesState>().damage = 6;
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

            if (hit.name == "L_L_2_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.20f, 0.20f, 0.20f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 20;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 20;
                    hit.GetComponent<HitBoxesState>().damage = 4;
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

            if (hit.name == "L_L_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.1f, 0.1f, 0.1f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 10;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 10;
                    hit.GetComponent<HitBoxesState>().damage = 1;
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

    void Up(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "H_R_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.45f, 0.45f, 0.45f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 80;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 20;
                    hit.GetComponent<HitBoxesState>().damage = 4;
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

            if (hit.name == "A_R_2_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.2f, 0.2f, 0.2f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 25;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 5;
                    hit.GetComponent<HitBoxesState>().damage = 2;
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

            if (hit.name == "A_R_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.2f, 0.2f, 0.2f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 25;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 5;
                    hit.GetComponent<HitBoxesState>().damage = 2;
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

    void Down(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "H_R_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.35f, 0.35f, 0.35f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 25;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 15;
                    hit.GetComponent<HitBoxesState>().damage = 4;
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

            if (hit.name == "A_R_2_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.1f, 0.1f, 0.1f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 10;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 5;
                    hit.GetComponent<HitBoxesState>().damage = 2;
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

            if (hit.name == "A_R_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.1f, 0.1f, 0.1f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 10;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 5;
                    hit.GetComponent<HitBoxesState>().damage = 2;
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

    void NAir(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "L_R_3_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.25f, 0.25f, 0.25f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 45;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 15;
                    hit.GetComponent<HitBoxesState>().damage = 7;
                }
                else if (i == 0)
                {
                    hit.GetComponent<HitBoxesState>().Vulnerable();
                    hit.GetComponent<HitBoxesState>().scale = Vector3.zero;
                    hit.GetComponent<HitBoxesState>().launchAngle = 0;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 0;

                }
            }

            if (hit.name == "L_R_2_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.2f, 0.2f, 0.2f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 35;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 7;
                    hit.GetComponent<HitBoxesState>().damage = 3;
                }
                else if (i == 0)
                {
                    hit.GetComponent<HitBoxesState>().Vulnerable();
                    hit.GetComponent<HitBoxesState>().scale = Vector3.zero;
                    hit.GetComponent<HitBoxesState>().launchAngle = 0;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 0;

                }
            }

            if (hit.name == "L_R_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.15f, 0.15f, 0.15f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 35;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 7;
                    hit.GetComponent<HitBoxesState>().damage = 3;
                }
                else if (i == 0)
                {
                    hit.GetComponent<HitBoxesState>().Vulnerable();
                    hit.GetComponent<HitBoxesState>().scale = Vector3.zero;
                    hit.GetComponent<HitBoxesState>().launchAngle = 0;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 0;

                }
            }
        }
    }

    void DAir(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "L_L_3_Box" || hit.name == "L_R_3_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.45f, 0.45f, 0.45f);
                    hit.GetComponent<HitBoxesState>().launchAngle = -90;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 50;
                    hit.GetComponent<HitBoxesState>().damage = 8;
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

            if (hit.name == "L_L_2_Box" || hit.name == "L_R_2_Box" ||
                hit.name == "L_L_1_Box" || hit.name == "L_R_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.2f, 0.2f, 0.2f);
                    hit.GetComponent<HitBoxesState>().launchAngle = -90;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 15;
                    hit.GetComponent<HitBoxesState>().damage = 4;
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

            if (hit.name == "B_1_Box" || hit.name == "B_2_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.2f, 0.2f, 0.2f);
                    hit.GetComponent<HitBoxesState>().launchAngle = -45;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 10;
                    hit.GetComponent<HitBoxesState>().damage = 4;
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

    void FAir(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "H_L_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.45f, 0.45f, 0.45f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 35;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 35;
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

            if (hit.name == "A_L_2_Box" || hit.name == "A_L_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.2f, 0.2f, 0.2f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 15;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 20;
                    hit.GetComponent<HitBoxesState>().damage = 5;
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

    void BAir(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "L_L_3_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.45f, 0.45f, 0.45f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 150;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 11;
                    hit.GetComponent<HitBoxesState>().damage = 7;
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

            if (hit.name == "L_L_2_Box" || hit.name == "L_L_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.2f, 0.2f, 0.2f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 160;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 6;
                    hit.GetComponent<HitBoxesState>().damage = 3;
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

    void UAir(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "L_L_3_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.45f, 0.45f, 0.45f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 90;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 10;
                    hit.GetComponent<HitBoxesState>().damage = 8;
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

            if (hit.name == "L_L_2_Box" || hit.name == "L_L_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.2f, 0.2f, 0.2f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 75;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 5;
                    hit.GetComponent<HitBoxesState>().damage = 4;
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

    void USmash(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "H_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.65f, 0.65f, 0.65f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 90;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 45;
                    hit.GetComponent<HitBoxesState>().damage = 14;
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

            if (hit.name == "B_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.35f, 0.35f, 0.35f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 90;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 20;
                    hit.GetComponent<HitBoxesState>().damage = 6;
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

    void DSmash(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "L_L_3_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.55f, 0.55f, 0.55f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 135;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 40;
                    hit.GetComponent<HitBoxesState>().damage = 12;
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

            if (hit.name == "L_L_2_Box" || hit.name == "L_L_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.35f, 0.35f, 0.35f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 135;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 17;
                    hit.GetComponent<HitBoxesState>().damage = 6;
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

            if (hit.name == "L_R_3_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.55f, 0.55f, 0.55f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 45;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 40;
                    hit.GetComponent<HitBoxesState>().damage = 12;
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

            if (hit.name == "L_R_2_Box" || hit.name == "L_R_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.35f, 0.35f, 0.35f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 45;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 17;
                    hit.GetComponent<HitBoxesState>().damage = 6;
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

    void FSmash(int i)
    {
        foreach (BoxCollider hit in state.hitBoxes)
        {
            if (hit.name == "H_L_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.65f, 0.65f, 0.65f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 35;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 50;
                    hit.GetComponent<HitBoxesState>().damage = 13;
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

            if (hit.name == "A_L_2_Box" || hit.name == "A_L_1_Box")
            {
                if (i == 1)
                {
                    hit.GetComponent<HitBoxesState>().DamageAction();
                    hit.GetComponent<HitBoxesState>().scale = new Vector3(0.15f, 0.15f, 0.15f);
                    hit.GetComponent<HitBoxesState>().launchAngle = 35;
                    hit.GetComponent<HitBoxesState>().launchSpeed = 23;
                    hit.GetComponent<HitBoxesState>().damage = 6;
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

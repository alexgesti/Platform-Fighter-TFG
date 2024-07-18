using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetLogic : MonoBehaviour
{
    public MovementBasis player;
    public GameObject[] targets;
    [HideInInspector] public int deactived;

    public Text counter, counterB;

    public Text chrono, chronoB;
    float time;
    public bool chronoActive;

    // Start is called before the first frame update
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Length == deactived)
        {
            player.isFinished = true;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.Axis.x = 0;
            chronoActive = false;
        }

        if (player.isFinished && targets.Length != deactived) chronoActive = false;

        counter.text = "x " + (targets.Length - deactived); // Se queja de estos dos pero funciona ingame (?????)
        counterB.text = "x " + (targets.Length - deactived);

        if (chronoActive) 
        {
            time += Time.deltaTime;

            float minutos = Mathf.FloorToInt(time / 60);
            float segundos = Mathf.FloorToInt(time % 60);
            float milisegundos = (time % 1) * 1000;

            chrono.text = string.Format("{0:00} : {1:00} : {2:000}", minutos, segundos, milisegundos);
            chronoB.text = string.Format(" {0:00} : {1:00} : {2:000}", minutos, segundos, milisegundos);
        }
    }
}

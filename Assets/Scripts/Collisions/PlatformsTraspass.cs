using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsTraspass : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Layer1" && 
            other.transform.parent.GetComponent<BasicMovement>().verticalSpeed > 0)
        {
            other.transform.parent.GetComponent<BasicMovement>().pCanTraspass = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Layer1")
        {
            other.transform.parent.GetComponent<BasicMovement>().pCanTraspass = false;
        }
    }
}

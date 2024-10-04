using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementDebug : MonoBehaviour
{
    Camera cam;

    public Transform target;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = false;

        if (target != null)
            offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Variables para el movimiento en los ejes X y Z
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) moveDirection += Vector3.up;         // Arriba
            if (Input.GetKey(KeyCode.S)) moveDirection += Vector3.down;       // Abajo
            if (Input.GetKey(KeyCode.A)) moveDirection += Vector3.left;       // Izquierda
            if (Input.GetKey(KeyCode.D)) moveDirection += Vector3.right;      // Derecha
            if (Input.GetKey(KeyCode.Q)) moveDirection += Vector3.forward;    // Forward
            if (Input.GetKey(KeyCode.E)) moveDirection += Vector3.back;       // Back

            Vector3 rotationDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.I)) rotationDirection += Vector3.left;   // Rotar hacia arriba
            if (Input.GetKey(KeyCode.K)) rotationDirection += Vector3.right;  // Rotar hacia abajo
            if (Input.GetKey(KeyCode.J)) rotationDirection += Vector3.down;   // Rotar a la izquierda
            if (Input.GetKey(KeyCode.L)) rotationDirection += Vector3.up;     // Rotar a la derecha

            offset = Quaternion.Euler(rotationDirection * 100 * Time.deltaTime) * offset;
            cam.transform.Translate(moveDirection * 100 * Time.deltaTime, Space.World);

            // Siempre mirar hacia el objetivo
            cam.transform.LookAt(target);
        }
    }
}

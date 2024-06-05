using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class EnvironmentCollisionBox : MonoBehaviour
{
    [HideInInspector] public static Vector3[] layer1 = new Vector3[7]; // Principal detector

    [HideInInspector] public bool isGrounded;

    [HideInInspector] public bool canTraspassP;
    [HideInInspector] public bool isGoingToTraspassP;
    [HideInInspector] public bool thatsAPlatform;

    [HideInInspector] public bool isCollidingW;
    [HideInInspector] public bool isRaycastHitW;
    [HideInInspector] public Transform objwall = null;
    [HideInInspector] public bool wallR;

    public Vector3 direction;
    public float maxDistance;

    void Start()
    {
        CreatingLayer1();
    }

    private void Update()
    {
        if (transform.parent.gameObject.GetComponent<BasicMovement>().directionS.x > 0) direction = new Vector3(0.75f, 0, 0);
        else if (transform.parent.gameObject.GetComponent<BasicMovement>().directionS.x < 0) direction = new Vector3(-0.75f, 0, 0);

        for (int i = 0; i < 6; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(new Vector3(transform.position.x,
                transform.position.y - (i * (transform.localScale.y / 2)),
                transform.position.z), direction, out hit, maxDistance))
            {
                if (hit.collider.gameObject.tag == "Wall_R" ||
                    hit.collider.gameObject.tag == "Wall_L") isRaycastHitW = true;
            }
            else isRaycastHitW = false;

            Debug.DrawRay(new Vector3(transform.position.x,
                transform.position.y - (i * (transform.localScale.y / 5)),
                transform.position.z), direction * maxDistance, Color.green);
        }
    }

    void CreatingLayer1()
    {
        if (layer1 != null && layer1.Length >= 3)
        {
            // Creacion de los puntos y mesh.
            layer1[0] = transform.localPosition + new Vector3(0, transform.localScale.y, 0);
            layer1[1] = transform.localPosition + new Vector3(transform.localScale.x / 2, 0, 0);
            layer1[2] = transform.localPosition + new Vector3(0, 0, transform.localScale.z / 2);
            layer1[3] = transform.localPosition - new Vector3(transform.localScale.x / 2, 0, 0);
            layer1[4] = transform.localPosition - new Vector3(0, transform.localScale.y, 0);
            layer1[6] = transform.localPosition - new Vector3(0, 0, transform.localScale.z / 2);

            Mesh mesh = new Mesh();
            mesh.vertices = layer1;

            // Triangular la Mesh (necesario para el MeshCollider)
            int[] triangles = new int[3 * (layer1.Length - 2)];
            for (int i = 0, j = 1, k = 2; k < layer1.Length; i++, j++, k++)
            {
                triangles[3 * i] = 0;
                triangles[3 * i + 1] = j;
                triangles[3 * i + 2] = k;
            }
            mesh.triangles = triangles;

            // Asignar el Mesh al MeshFilter
            GetComponent<MeshFilter>().mesh = mesh;

            // Configurar el MeshCollider
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;
            meshCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor" 
            && !transform.parent.GetComponent<BasicMovement>().pCanTraspass)
        {
            isGrounded = true;
            isGoingToTraspassP = false;
            if (transform.parent.GetComponent<BasicMovement>().sTop == 0)
            {
                transform.parent.GetComponent<BasicMovement>().isJumping = false;
                transform.parent.GetComponent<BasicMovement>().jFMFSJCounter = 0;
                transform.parent.GetComponent<BasicMovement>().jFMFSTFHCounter = 0;
            }
        }

        if (other.gameObject.tag == "PlatformF" && !transform.parent.GetComponent<BasicMovement>().isInTheAirUp
            && !transform.parent.GetComponent<BasicMovement>().pCanTraspass && !isGoingToTraspassP)
        {
            isGrounded = true;
            canTraspassP = true;
            isGoingToTraspassP = false;
        }

        if (other.gameObject.tag == "PlatformF" && !transform.parent.GetComponent<BasicMovement>().DownAxisIsActive)
        {
            thatsAPlatform = true;
        }

        if (other.gameObject.tag == "Wall_R" || other.gameObject.tag == "Wall_L")
        {
            isCollidingW = true;
            objwall = other.gameObject.GetComponent<Transform>();
        }

        if (other.gameObject.tag == "Wall_R") wallR = true; 
        else if (other.gameObject.tag == "Wall_L") wallR = false; 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }

        if (other.gameObject.tag == "PlatformF")
        {
            isGrounded = false;
            canTraspassP = false;
            thatsAPlatform = false;
        }

        if (other.gameObject.tag == "Wall_R" || other.gameObject.tag == "Wall_L")
        {
            isCollidingW = false; 
            objwall = null;
        }
    }
}

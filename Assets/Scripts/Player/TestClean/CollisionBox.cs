using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBox : MonoBehaviour
{
    [HideInInspector] public static Vector3[] layer = new Vector3[7]; // Principal detector
    MeshCollider meshCollider;

    [HideInInspector] public bool isGrounded;
    MovementBasis player;

    [Header("RaycastPosition")]
    public Vector3 directionC;
    //[HideInInspector] public bool itTraspass;
    [HideInInspector] public static float maxDistance;
    public bool raycastHitFloor, raycastHitPlatform, itsTagged;
    Transform rayHitObj;
    public Transform baseRayHitObj;

    // Start is called before the first frame update
    void Start()
    {
        CreatingCollision();

        player = GetComponent<MovementBasis>();
        maxDistance = 5;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastGround();

        //if (!raycastHitGround) isGrounded = false;
    }

    void CreatingCollision()
    {
        if (layer != null && layer.Length >= 3)
        {
            // Creacion de los puntos y mesh.
            layer[0] = new Vector3(0, transform.localScale.y, 0);
            layer[1] = new Vector3(transform.localScale.x / 2, 0, 0);
            layer[2] = new Vector3(0, 0, transform.localScale.z / 2);
            layer[3] = new Vector3(-transform.localScale.x / 2, 0, 0);
            layer[4] = new Vector3(0, -transform.localScale.y, 0);
            layer[6] = new Vector3(0, 0, -transform.localScale.z / 2);

            Mesh mesh = new Mesh();
            mesh.vertices = layer;

            // Triangular la Mesh (necesario para el MeshCollider)
            int[] triangles = new int[3 * (layer.Length - 2)];
            for (int i = 0, j = 1, k = 2; k < layer.Length; i++, j++, k++)
            {
                triangles[3 * i] = 0;
                triangles[3 * i + 1] = j;
                triangles[3 * i + 2] = k;
            }
            mesh.triangles = triangles;

            // Asignar el Mesh al MeshFilter
            GetComponent<MeshFilter>().mesh = mesh;

            // Configurar el MeshCollider
            meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;
            meshCollider.isTrigger = true;
        }
    }

    void RaycastGround()
    {
        RaycastHit hit;

        if (rayHitObj != null) maxDistance = transform.position.y - 
                (rayHitObj.position.y + rayHitObj.localScale.y / 2);

        if (Physics.Raycast(transform.position - new Vector3(0, transform.localScale.y / 2, 0), directionC, out hit, maxDistance))
        {
            if (hit.collider)
            {
                if (hit.collider.gameObject.tag == "PlatformF"
                    || hit.collider.gameObject.tag == "Floor")
                {
                    rayHitObj = hit.collider.transform;
                }
            }

            if (hit.collider.gameObject.tag == "Floor")
            {
                raycastHitFloor = true;
                raycastHitPlatform = false;
            }

            if (hit.collider.gameObject.tag == "PlatformF")
            {
                raycastHitPlatform = true;
                raycastHitFloor = false;
            }

            if (hit.collider.gameObject.tag != "PlatformF" && hit.collider.gameObject.tag != "Floor")
            {
                raycastHitPlatform = false;
                raycastHitFloor = false;
            }

            //Debug.Log(hit.collider);
        }
        else
        {
            if (!isGrounded) rayHitObj = baseRayHitObj;
            raycastHitPlatform = false;
            raycastHitFloor = false;
        }

        Debug.DrawRay(transform.position - new Vector3(0, transform.localScale.y / 2, 0), directionC * maxDistance, Color.red); //probar las colisiones de caida.raycast bien? o es colision ?
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor" && raycastHitFloor)
        {
            isGrounded = true;

            // Pos correction
            transform.position =
            new Vector3(transform.position.x,
            other.transform.position.y + other.transform.localScale.y / 2 + transform.localScale.y,
            transform.position.z);

            player.AfterLanding();
        }

        if (other.gameObject.tag == "PlatformF" && raycastHitPlatform && player.verticalSpeed <= 0)
        {
            isGrounded = true;

            // Pos correction
            transform.position =
            new Vector3(transform.position.x,
            other.transform.position.y + other.transform.localScale.y / 2 + transform.localScale.y,
            transform.position.z);

            player.AfterLanding();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Floor" && raycastHitFloor)
        {
            isGrounded = true;

            // Pos correction
            transform.position =
            new Vector3(transform.position.x,
            other.transform.position.y + other.transform.localScale.y / 2 + transform.localScale.y,
            transform.position.z);
        }

        if (other.gameObject.tag == "PlatformF" && raycastHitPlatform && player.verticalSpeed <= 0)
        {
            isGrounded = true;

            // Pos correction
            transform.position =
            new Vector3(transform.position.x,
            other.transform.position.y + other.transform.localScale.y / 2 + transform.localScale.y,
            transform.position.z);
        }
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
        }
    }
}

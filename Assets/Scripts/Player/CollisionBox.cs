using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBox : MonoBehaviour
{
    [HideInInspector] public static Vector3[] layer = new Vector3[7]; // Principal detector
    MeshCollider meshCollider;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isTouchingPlatform;
    MovementBasis player;

    [Header("RaycastPosition")]
    public Vector3 directionC;
    [HideInInspector] public static float maxDistance;
    [HideInInspector] public bool raycastHitFloor;
    [HideInInspector] public bool raycastHitPlatform;
    Collider raycastHitCollider;

    [Header("RaycastWall")]
    public Vector3[] originRaycasts;
    public float maxDistanceW;



    // Start is called before the first frame update
    void Start()
    {
        CreatingCollision();

        player = GetComponent<MovementBasis>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastGround();
        if (player.isTouchingWall) RaycastHorizontal();
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
            //meshCollider.isTrigger = true;
        }
    }

    void RaycastGround()
    {
        RaycastHit hit;

        maxDistance = Mathf.Infinity;
        
        if (Physics.Raycast(transform.position - new Vector3(0, transform.localScale.y / 2, 0), directionC, out hit, maxDistance))
        {
            if (hit.collider.gameObject != gameObject)
            {
                float distance = Vector3.Distance(transform.position - new Vector3(0, transform.localScale.y / 2, 0), hit.point);
                if (distance < maxDistance) maxDistance = distance;

                raycastHitCollider = hit.collider;

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
            }
        }
        else
        {
            raycastHitFloor = false;
            raycastHitPlatform = false;
            raycastHitCollider = null;
        }
        
        Debug.DrawRay(transform.position - new Vector3(0, transform.localScale.y / 2, 0), directionC * maxDistance, Color.red);
    }

    void RaycastHorizontal()
    {
        player.isTouchingWall = false;

        foreach (Vector3 origin in originRaycasts)
        {
            RaycastHit hitW;

            if (Physics.Raycast(origin + transform.position, new Vector3(Mathf.Sign(player.Axis.x) * Mathf.Ceil(Mathf.Abs(player.Axis.x)), 0, 0), out hitW, maxDistanceW))
            {
                if (hitW.collider.gameObject != gameObject)
                {
                    if (hitW.collider.gameObject.tag == "Floor")
                    {
                        player.isTouchingWall = true;
                    }
                }
            }

            Debug.DrawRay(origin + transform.position, new Vector3(Mathf.Sign(player.Axis.x) * Mathf.Ceil(Mathf.Abs(player.Axis.x)), 0, 0) * maxDistanceW, Color.red);
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Floor" && raycastHitFloor
            && other.collider == raycastHitCollider)
        {
            isGrounded = true;
            CorrectionPositionVertical(other.collider);
            player.AfterLanding();
        }

        if (other.gameObject.tag == "PlatformF" && raycastHitPlatform 
            && player.verticalSpeed <= 0 && !player.canFallPlatform
            && other.collider == raycastHitCollider)
        {
            isTouchingPlatform = true;
            isGrounded = true;
            CorrectionPositionVertical(other.collider);
            player.AfterLanding();
        }

        if (other.gameObject.tag == "Floor" && other.collider != raycastHitCollider)
            CorrectionPositionHorizontal(other.collider);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Floor" && raycastHitFloor
            && other.collider == raycastHitCollider)
        {
            isGrounded = true;
            CorrectionPositionVertical(other.collider);
        }

        if (other.gameObject.tag == "PlatformF" && raycastHitPlatform 
            && player.verticalSpeed <= 0 && !player.canFallPlatform
            && other.collider == raycastHitCollider)
        {
            isTouchingPlatform = true;
            isGrounded = true;
            CorrectionPositionVertical(other.collider);
            if (player.isFastFall) player.AfterLanding();
        }

        if (other.gameObject.tag == "Floor" && other.collider != raycastHitCollider)
            CorrectionPositionHorizontal(other.collider);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }

        if (other.gameObject.tag == "PlatformF")
        {
            isTouchingPlatform = false;

            isGrounded = false;
        }
    }

    void CorrectionPositionVertical(Collider other)
    {
        transform.position =
        new Vector3(transform.position.x,
        other.transform.position.y + other.transform.localScale.y / 2 + transform.localScale.y,
        transform.position.z);
    }

    void CorrectionPositionHorizontal(Collider other)
    {
        Vector3 contactPoint = other.ClosestPoint(transform.position);
        float playerWidth = transform.localScale.x / 2;

        if (contactPoint.x >= transform.position.x)
        {
            transform.position = new Vector3(contactPoint.x - playerWidth - 0.01f, transform.position.y, transform.position.z);
            player.isTouchingWall = true;
        }
        else if (contactPoint.x <= transform.position.x)
        {
            transform.position = new Vector3(contactPoint.x + playerWidth + 0.01f, transform.position.y, transform.position.z);
            player.isTouchingWall = true;
        }
    }
}

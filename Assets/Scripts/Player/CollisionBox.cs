using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBox : MonoBehaviour
{
    [HideInInspector] public static Vector3[] layer = new Vector3[10]; // Principal detector
    MeshCollider meshCollider;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isTouchingPlatform;
    bool isFalling;
    MovementBasis player;

    [Header("RaycastPosition")]
    public Vector3 directionC;
    [HideInInspector] public static float maxDistance;
    [HideInInspector] public bool raycastHitFloor;
    [HideInInspector] public bool raycastHitPlatform;
    Collider raycastHitCollider;
    //public List<Collider> groundColliders = new List<Collider>(); // Intento de correccion de tocar varios suelos (no funciona).

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
        TraspassController();
        //RaycastAligment();

        //Debug.Log(player.speed);
        //Debug.Log("IsGrounded: " + isGrounded);
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
            layer[5] = new Vector3(0, 0, -transform.localScale.z / 2);
            layer[6] = new Vector3(transform.localScale.x / 3, transform.localScale.y / 1.5f, 0);
            layer[7] = new Vector3(-transform.localScale.x / 3, transform.localScale.y / 1.5f, 0);
            layer[8] = new Vector3(0, transform.localScale.y / 1.5f, transform.localScale.z / 3);
            layer[9] = new Vector3(0, transform.localScale.y / 1.5f, -transform.localScale.z / 3);
            //layer[10] = new Vector3(transform.localScale.x / 3, -transform.localScale.y, 0);
            //layer[11] = new Vector3(-transform.localScale.x / 3, -transform.localScale.y, 0);
            //layer[12] = new Vector3(0, -transform.localScale.y, transform.localScale.z / 3);
            //layer[13] = new Vector3(0, -transform.localScale.y, -transform.localScale.z / 3);

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
        }
    }

    void RaycastGround()
    {
        RaycastHit hit;

        maxDistance = Mathf.Infinity;
        
        if (Physics.Raycast(transform.position - new Vector3(0, transform.localScale.y / 2, 0), directionC, out hit, maxDistance))
        {
            if (hit.collider.gameObject != gameObject && hit.collider.gameObject.layer != LayerMask.NameToLayer("HitBoxes"))
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
                if (hitW.collider.gameObject != gameObject && hitW.collider.gameObject.layer != LayerMask.NameToLayer("HitBoxes"))
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

    //void RaycastAligment() // Intento de correccion de rampas (no funciona).
    //{
    //    RaycastHit info;
    //
    //    if (Physics.Raycast(transform.position, Vector3.down, out info))
    //    {
    //        Quaternion rotationRef = Quaternion.FromToRotation(Vector3.up, info.normal);
    //        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rotationRef.eulerAngles.z);
    //    }
    //}

    void TraspassController()
    {
        if (raycastHitPlatform)
        {
            if (isFalling)
            {
                meshCollider.enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Floor" && raycastHitFloor
            && other.collider == raycastHitCollider)
        {
            isGrounded = true;
            player.AfterLanding();
            player.audio.IsGround();
        }

        if (other.gameObject.tag == "PlatformF" && raycastHitPlatform 
            && player.verticalSpeed <= 0 && !player.canFallPlatform
            && other.collider == raycastHitCollider)
        {
            isTouchingPlatform = true;
            isGrounded = true;
            player.AfterLanding();
            player.audio.IsGround();
        }

        //if ((other.gameObject.tag == "Floor" || other.gameObject.tag == "PlatformF") && !groundColliders.Contains(other.collider))
        //    groundColliders.Add(other.collider);

        if (other.gameObject.tag == "Floor" && other.collider != raycastHitCollider)
            RaycastHorizontal();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Floor" && raycastHitFloor
            && other.collider == raycastHitCollider)
        {
            isGrounded = true;

            if (player.isFastFall)
            {
                player.AfterLanding();
                player.audio.IsGround();
            }
        }

        if (other.gameObject.tag == "PlatformF" && raycastHitPlatform 
            && player.verticalSpeed <= 0 && !player.canFallPlatform
            && other.collider == raycastHitCollider)
        {
            isTouchingPlatform = true;
            isGrounded = true;
        }

        //if ((other.gameObject.tag == "Floor" || other.gameObject.tag == "PlatformF") && !groundColliders.Contains(other.collider))
        //    groundColliders.Add(other.collider);

        if (other.gameObject.tag == "Floor" && other.collider != raycastHitCollider)
            RaycastHorizontal();
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            //groundColliders.Remove(other.collider);

            //if (groundColliders.Count == 0) 
                isGrounded = false;

            if (player.isTouchingWall) player.isTouchingWall = false;
        }

        if (other.gameObject.tag == "PlatformF")
        {
            //groundColliders.Remove(other.collider);

            //if (groundColliders.Count == 0)
            //{
                isTouchingPlatform = false;

                isGrounded = false;
            //}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor" && raycastHitFloor
            && other == raycastHitCollider)
        {
            Physics.IgnoreCollision(meshCollider, other, false);
        }
        
        if (other.gameObject.tag == "PlatformF" && raycastHitPlatform
            && player.verticalSpeed <= 0 && !player.canFallPlatform
            && other == raycastHitCollider)
        {
            Physics.IgnoreCollision(meshCollider, other, false);
        }
        else if (other.gameObject.tag == "PlatformF" && player.canFallPlatform) isFalling = true;
        else
        {
            if (other.gameObject.tag == "PlatformF") Physics.IgnoreCollision(meshCollider, other, true);
        }

        if (other.gameObject.tag == "Player" && !isGrounded) Physics.IgnoreCollision(meshCollider, other, true);
        else if (other.gameObject.tag == "Player" && isGrounded) Physics.IgnoreCollision(meshCollider, other, false);

        // Exclusive for Break the targets
        if (other.gameObject.tag == "Target")
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            other.gameObject.GetComponent<AudioSource>().Play();
            GameObject.Find("Targets").GetComponent<TargetLogic>().deactived++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Floor" && raycastHitFloor
            && other == raycastHitCollider)
        {
            Physics.IgnoreCollision(meshCollider, other, false);
        }
        
        if (other.gameObject.tag == "PlatformF" && raycastHitPlatform
            && player.verticalSpeed <= 0 && !player.canFallPlatform
            && other == raycastHitCollider)
        {
            Physics.IgnoreCollision(meshCollider, other, false);
        }
        else if (other.gameObject.tag == "PlatformF" && player.canFallPlatform) isFalling = true;
        else
        {
            if (other.gameObject.tag == "PlatformF") Physics.IgnoreCollision(meshCollider, other, true);
        }

        if (other.gameObject.tag == "Player" && !isGrounded) Physics.IgnoreCollision(meshCollider, other, true);
        else if (other.gameObject.tag == "Player" && isGrounded) Physics.IgnoreCollision(meshCollider, other, false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlatformF" || other.gameObject.tag == "Floor")
        {
            Physics.IgnoreCollision(meshCollider, other, false);

            if (isFalling)
            {
                meshCollider.enabled = true;

                isTouchingPlatform = false;
                isFalling = false;
            }
        }
    }
}
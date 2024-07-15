using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlaltform : MonoBehaviour
{
    public float speed;
    public float moveDistance;

    Vector3 startPos;
    bool movingForward;
    Rigidbody rb;

    MovementBasis player;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = movingForward ? Vector3.right : Vector3.left;
        Vector3 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;

        if (player != null && player.cb.isGrounded) player.platforMoving = (newPosition.x - rb.position.x) / Time.fixedDeltaTime;
        
        rb.MovePosition(newPosition);

        if (Vector3.Distance(startPos, transform.position) >= moveDistance)
        {
            movingForward = !movingForward;
            startPos = transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            player = collision.transform.GetComponent<MovementBasis>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            player.platforMoving = 0;

            player = null;
        }
    }
}

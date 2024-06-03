using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    BasicMovement player;
    Animator anim;
    public GameObject dJump;
    bool dOneTime;
    float speedW;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent<BasicMovement>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Axis.x > 0) transform.localEulerAngles = new Vector3(0, 90, 0);
        else if (player.Axis.x < 0) transform.localEulerAngles = new Vector3(0, -90, 0);

        if (player.speed == player.runSpeed && player.ecb.isGrounded) // Run
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", true);
        }
        else if (player.speed == player.walkSpeed && player.ecb.isGrounded) // Walk
        {
            anim.SetBool("isWalk", true);
            anim.SetBool("isRun", false);

            // Animation Speed Walk
            speedW = Mathf.Abs(player.Axis.x) * 3;
            anim.SetFloat("speedW", speedW);
        }
        else if (player.speed == 0 && player.ecb.isGrounded) // Idle after Run
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
        }
        else if (player.verticalSpeed > 0) // Jump
        {
            anim.SetBool("isJump", true);
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
        }
        else if (player.verticalSpeed < 0 && !player.ecb.isGrounded) // Fall 
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", true);
        }

        if (player.ecb.isGrounded) // Idle after Fall
        {
            anim.SetBool("isFall", false);
        }

        if (player.djOneTime && !dOneTime) // Double Jump (Effect)
        {
            dJump.transform.position = player.transform.position - new Vector3(0, 1, 0);
            dJump.GetComponent<Animator>().SetBool("dJump", true);
            dOneTime = true;
        }
        else if (!player.djOneTime) // After Double Jump (Effect)
        {
            dJump.transform.position = new Vector3(0, -10, 0);
            dJump.GetComponent<Animator>().SetBool("dJump", false);
            dOneTime = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    //BasicMovement player;
    MovementBasis player;
    Animator anim;
    public GameObject dJump;
    public GameObject fastFall;
    public ParticleSystem particle;
    bool djOneTime, fOneTime, dOneTime;
    float speedW;

    // Start is called before the first frame update
    void Start()
    {
        //player = gameObject.GetComponentInParent<BasicMovement>();
        player = gameObject.GetComponentInParent<MovementBasis>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Axis.x > player.joystickThresholdMin && player.cb.isGrounded)
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
            particle.transform.localRotation = Quaternion.Euler(-10, -90, 0);
        }
        else if (player.Axis.x < -player.joystickThresholdMin && player.cb.isGrounded)
        {
            transform.localEulerAngles = new Vector3(0, -90, 0);
            particle.transform.localRotation = Quaternion.Euler(-10, 90, 0);
        }

        if (player.isDashing && !player.tractionBool && player.cb.isGrounded) // Dash before Run
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
            anim.SetBool("isGonnaRun", true);

            if (!dOneTime)
            {
                particle.Play();
                particle.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z);
                dOneTime = true;
            }
        }
        else if (player.speed == player.runSpeed && player.cb.isGrounded) // Run
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", true);
            anim.SetBool("isTraction", false);
            anim.SetBool("isDirectionChanged", false);
            anim.SetBool("isGonnaRun", false);
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", false);

            particle.transform.position = new Vector3(0, -10, 0);
        }
        else if (player.speed == player.walkSpeed && player.cb.isGrounded) // Walk
        {
            anim.SetBool("isWalk", true);
            anim.SetBool("isRun", false);
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", false);

            // Animation Speed Walk
            speedW = Mathf.Abs(player.Axis.x) * 3;
            anim.SetFloat("speedW", speedW);
        }
        else if (player.speed == 0 && !player.tractionBool && player.cb.isGrounded) // Idle -> Apaga igualmente a isRun y isWalk
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", false);
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
            anim.SetBool("isTraction", false);
            anim.SetBool("isDirectionChanged", false);
            anim.SetBool("isGonnaRun", false);

            dOneTime = false;
            particle.transform.position = new Vector3(0, -10, 0);
        }
        else if (player.verticalSpeed > 0 && !player.cb.isGrounded) // Jump
        {
            anim.SetBool("isJump", true);
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
            anim.SetBool("isTraction", false);
            anim.SetBool("isDirectionChanged", false);
            anim.SetBool("isGonnaRun", false);
        }
        else if (player.verticalSpeed < 0 && !player.cb.isGrounded) // Fall 
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", true);
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
            anim.SetBool("isTraction", false);
            anim.SetBool("isDirectionChanged", false);
            anim.SetBool("isGonnaRun", false);
        }
        else if (player.tractionBool && player.isChangingDirTraction) // Change direction (traction)
        {
            anim.SetBool("isTraction", false);
            anim.SetBool("isDirectionChanged", true);
            anim.SetBool("isJump", false);
            anim.SetBool("isRun", true);
        }
        else if (player.tractionBool && !player.isChangingDirTraction) // Traction stop
        {
            anim.SetBool("isTraction", true);
            anim.SetBool("isDirectionChanged", false);
            anim.SetBool("isJump", false);
            anim.SetBool("isRun", false);
        }

        if (player.cb.isGrounded) // Idle after Fall
        {
            anim.SetBool("isFall", false);
        }
        
        if (player.djOneTime && !djOneTime) // Double Jump (Effect)
        {
            dJump.transform.position = player.transform.position - new Vector3(0, 1, 0);
            dJump.GetComponent<Animator>().SetBool("dJump", true);
            djOneTime = true;
        }
        else if (!player.djOneTime) // After Double Jump (Effect)
        {
            dJump.transform.position = new Vector3(0, -10, 0);
            dJump.GetComponent<Animator>().SetBool("dJump", false);
            djOneTime = false;
        }

        if (player.isFastFall && !fOneTime) // Fast Fall
        {
            fastFall.transform.position = player.transform.position + new Vector3(0, 0.2f, -0.2f);
            fastFall.GetComponent<Animator>().SetBool("fastfall", true);
            fOneTime = true;
        }
        else if (fastFall.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("spark_signal") &&
            fastFall.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !player.isFastFall)
        {
            fastFall.transform.position = new Vector3(0, -10, 0);
            fastFall.GetComponent<Animator>().SetBool("fastfall", false);
            fOneTime = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    //BasicMovement player;
    MovementBasis player;
    HitBoxesController hit;
    Animator anim;
    public GameObject dJump;
    public GameObject fastFall;
    public ParticleSystem particle;
    bool djOneTime, fOneTime, dOneTime;
    float speedW;

    // Color blink
    List<Renderer> renderModel = new List<Renderer>();
    bool isBlinking;

    // Start is called before the first frame update
    void Start()
    {
        //player = gameObject.GetComponentInParent<BasicMovement>();
        player = gameObject.GetComponentInParent<MovementBasis>();
        hit = GetComponent<HitBoxesController>();
        anim = GetComponent<Animator>();

        isBlinking = false;

        Renderer[] childModel = GetComponentsInChildren<Renderer>();

        foreach (Renderer model in childModel)
        {
            if (model.gameObject.CompareTag("PlayerModel"))
                renderModel.Add(model);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit.isN && !hit.isF && !hit.isD && !hit.isU
            && !hit.isFSmash && !hit.isDSmash && !hit.isUSmash
            && !player.isCrouching && !player.isBusy)
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
        }

        // Movement
        if (!hit.isN && !hit.isF && !hit.isD && !hit.isU && 
            !hit.isNAir && !hit.isFAir && !hit.isBAir && !hit.isDAir && !hit.isUAir)
        {
            if (player.isDashing && !player.tractionBool && player.cb.isGrounded && !player.isCrouching) // Dash before Run
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
            else if (player.speed == player.runSpeed && player.cb.isGrounded && !player.isCrouching) // Run
            {
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", true);
                anim.SetBool("isTraction", false);
                anim.SetBool("isDirectionChanged", false);
                anim.SetBool("isGonnaRun", false);
                anim.SetBool("isJump", false);
                anim.SetBool("isFall", false);
                anim.SetBool("isCrouch", false);

                particle.transform.position = new Vector3(0, -10, 0);
            }
            else if (player.speed == player.walkSpeed && player.cb.isGrounded && !player.isCrouching) // Walk
            {
                anim.SetBool("isWalk", true);
                anim.SetBool("isRun", false);
                anim.SetBool("isJump", false);
                anim.SetBool("isFall", false);
                anim.SetBool("isCrouch", false);

                // Animation Speed Walk
                speedW = Mathf.Abs(player.Axis.x) * 3;
                anim.SetFloat("speedW", speedW);
            }
            else if (player.speed == 0 && !player.tractionBool && player.cb.isGrounded && !player.isCrouching) // Idle -> Apaga igualmente a isRun y isWalk
            {
                anim.SetBool("isJump", false);
                anim.SetBool("isFall", false);
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", false);
                anim.SetBool("isTraction", false);
                anim.SetBool("isDirectionChanged", false);
                anim.SetBool("isGonnaRun", false);
                anim.SetBool("isCrouch", false);

                dOneTime = false;
                particle.transform.position = new Vector3(0, -10, 0);
            }
            else if (player.verticalSpeed > 0) // Jump. ATENCION Le he sacado el !isGrounded. Puede que sea necesario volverlo a poner.
            {
                anim.SetBool("isJump", true);
                anim.SetBool("isCrouch", false); 
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", false);
                anim.SetBool("isTraction", false);
                anim.SetBool("isDirectionChanged", false);
                anim.SetBool("isGonnaRun", false);

                dOneTime = false;
            }
            else if (player.verticalSpeed < 0 && !player.cb.isGrounded) // Fall 
            {
                anim.SetBool("isJump", false);
                anim.SetBool("isFall", true);
                anim.SetBool("isCrouch", false);
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", false);
                anim.SetBool("isTraction", false);
                anim.SetBool("isDirectionChanged", false);
                anim.SetBool("isGonnaRun", false);

                dOneTime = false;
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
            else if (player.isCrouching) // Crouch
            {
                anim.SetBool("isCrouch", true);
                anim.SetBool("isJump", false);
                anim.SetBool("isFall", false);
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", false);
                anim.SetBool("isTraction", false);
                anim.SetBool("isDirectionChanged", false);
                anim.SetBool("isGonnaRun", false);
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
        }

        if (fastFall.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("spark_signal") &&
            fastFall.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !player.isFastFall)
        {
            fastFall.transform.position = new Vector3(0, -10, 0);
            fastFall.GetComponent<Animator>().SetBool("fastfall", false);
            fOneTime = false;
        }

        // Attacks
        if (hit.isN)
        {
            anim.SetBool("isGonnaN", true);

            if (hit.isNRepeat) anim.SetBool("isGonnaNLoop", true);
            else anim.SetBool("isGonnaNLoop", false);

            if (hit.isN2) anim.SetBool("isGonnaN2", true);
            if (hit.isN3) anim.SetBool("isGonnaN3", true);
        }
        else if (hit.isF)
        {
            anim.SetBool("isGonnaF", true);
        }
        else if (hit.isU)
        {
            anim.SetBool("isGonnaU", true);
        }
        else if (hit.isD)
        {
            anim.SetBool("isGonnaD", true);
        }
        else if (hit.isNAir) // Aerial
        {
            anim.SetBool("isGonnaNAir", true);
        }
        else if (hit.isDAir) // Aerial
        {
            anim.SetBool("isGonnaDAir", true);
        }
        else if (hit.isUAir) // Aerial
        {
            anim.SetBool("isGonnaUAir", true);
        }
        else if (hit.isFAir) // Aerial
        {
            anim.SetBool("isGonnaFAir", true);
        }
        else if (hit.isBAir) // Aerial
        {
            anim.SetBool("isGonnaBAir", true);
        }
        else if (hit.isUSmash)
        {
            anim.SetBool("isGonnaSmash", true);
            anim.SetBool("isGonnaUSmash", true);

            BlinkLogic();
        }
        else if (hit.isDSmash)
        {
            anim.SetBool("isGonnaSmash", true);
            anim.SetBool("isGonnaDSmash", true);

            BlinkLogic();
        }
        else if (hit.isFSmash)
        {
            anim.SetBool("isGonnaSmash", true);
            anim.SetBool("isGonnaFSmash", true);

            BlinkLogic();
        }

        if (hit.isN || hit.isF || hit.isD || hit.isU ||
            hit.isNAir || hit.isBAir || hit.isFAir || hit.isDAir || hit.isUAir ||
            hit.canSmash || hit.isUSmash || hit.isFSmash || hit.isDSmash)
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
            if (hit.isN || hit.isF || hit.isD || hit.isU) anim.SetBool("isFall", false);
            anim.SetBool("isTraction", false);
            anim.SetBool("isDirectionChanged", false);
            anim.SetBool("isGonnaRun", false);
            if ((hit.isN || hit.isF || hit.isU) && !hit.isD) anim.SetBool("isCrouch", false);
        }
        else
        {
            anim.SetBool("isGonnaN", false);
            anim.SetBool("isGonnaF", false);
            anim.SetBool("isGonnaD", false);
            anim.SetBool("isGonnaU", false);
            anim.SetBool("isGonnaNAir", false);
            anim.SetBool("isGonnaFAir", false);
            anim.SetBool("isGonnaBAir", false);
            anim.SetBool("isGonnaDAir", false);
            anim.SetBool("isGonnaUAir", false);
            anim.SetBool("isGonnaN2", false);
            anim.SetBool("isGonnaN3", false);
            anim.SetBool("isGonnaNLoop", false);
            anim.SetBool("isGonnaSmash", false);
            anim.SetBool("isGonnaFSmash", false);
            anim.SetBool("isGonnaDSmash", false);
            anim.SetBool("isGonnaUSmash", false);
            if (player.cb.isGrounded) anim.SetBool("isFall", false);
        }
    }

    void BlinkLogic()
    {
        float timeRatio = GetComponent<HitBoxesController>().framesHeld / 
            GetComponent<HitBoxesController>().framesMaxCharge;

        float blinkSpeed = Mathf.Lerp(1, 10, timeRatio);

        if (!isBlinking)
            StartCoroutine(BlinkCharacter(blinkSpeed));
    }

    IEnumerator BlinkCharacter(float blinkSpeed)
    {
        isBlinking = true;

        while (GetComponent<HitBoxesController>().framesHeld <
            GetComponent<HitBoxesController>().framesMaxCharge)
        {
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1);

            foreach (Renderer model in renderModel)
            {
                model.material.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 0, 0.25f), t);
            }

            yield return null;
        }

        foreach (Renderer model in renderModel)
        {
            model.material.color = new Color(1, 1, 1, 1);
        }

        isBlinking = false;
    }
}

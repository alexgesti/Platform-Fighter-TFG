using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DamagePlayer : MonoBehaviour
{
    public bool isTargets;

    Transform player;
    [HideInInspector] public bool ko;

    // Text
    public Text p1Text;
    public Animator p1Anim;
    public Text p1bText;
    public Animator p1bAnim;
    public Color startColor;
    public Color finalColor;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Transform>();
    }

    private void Update()
    {
        if (ko && isTargets)
        {
            p1Text.text = "";
            p1bText.text = "";
        }

        if (this.p1Anim.GetCurrentAnimatorStateInfo(0).IsName("DamagedVibration")) p1Anim.SetBool("hit", false);
        if (this.p1bAnim.GetCurrentAnimatorStateInfo(0).IsName("DamagedVibration")) p1bAnim.SetBool("hit", false);
    }

    public void IsDamaged(int damaged)
    {
        if (!isTargets) // Comparador no funciona. Also hay que resetear el damage en movementbasis.- 12/08 -> no se que se refiere. pese que colisiona con las cosas el rigidbody, no se a que se refiere esto
        {
            p1Anim.SetBool("hit", true);
            p1bAnim.SetBool("hit", true);
            if (damaged < 200) p1Text.color = p1Text.color - (new Color(0.002843135f, 0.005f, 0.005f, 0f) * GetComponent<MovementBasis>().damage);
            else p1Text.color = finalColor;

            p1Text.text = damaged + "%";
            p1bText.text = damaged + "%";
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Blastline")
        {
            ko = true;
            player.GetComponent<MovementBasis>().audio.Death();

            GetComponent<MovementBasis>().percentage = 0;
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            p1Text.text = "0%";
            p1bText.text = "0%";
            p1Text.color = startColor;

            if (!isTargets) player.position = new Vector3(0, 13, 0);
            else
            {
                player.GetComponent<MovementBasis>().isFinished = true;
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}

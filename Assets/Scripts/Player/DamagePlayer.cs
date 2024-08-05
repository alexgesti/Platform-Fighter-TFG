using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePlayer : MonoBehaviour
{
    public bool isTargets;

    int damage, textDamage;

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

        damage = 0;
        textDamage = int.Parse(p1Text.text);
    }

    private void Update()
    {
        if (damage > textDamage && !isTargets) // Comparador no funciona. Also hay que resetear el damage en movementbasis.
        {
            p1Anim.SetBool("hit", true);
            p1bAnim.SetBool("hit", true);
            //damage += 10; 
            //if (damage >= 999) damage = 999;
            damage = GetComponent<MovementBasis>().percentage;

            if (damage < 200) p1Text.color = p1Text.color - new Color(0.037f, 0.085f, 0.085f, 0f);

            p1Text.text = damage + "%";
            p1bText.text = damage + "%";

            textDamage = int.Parse(p1Text.text);
        }
        
        if (ko && isTargets)
        {
            p1Text.text = "";
            p1bText.text = "";
        }

        if (this.p1Anim.GetCurrentAnimatorStateInfo(0).IsName("DamagedVibration")) p1Anim.SetBool("hit", false);
        if (this.p1bAnim.GetCurrentAnimatorStateInfo(0).IsName("DamagedVibration")) p1bAnim.SetBool("hit", false);

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Blastline")
        {
            ko = true;
            player.GetComponent<MovementBasis>().audio.Death();
            if (!isTargets) player.position = new Vector3(0, 13, 0);
            else
            {
                player.GetComponent<MovementBasis>().isFinished = true;
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}

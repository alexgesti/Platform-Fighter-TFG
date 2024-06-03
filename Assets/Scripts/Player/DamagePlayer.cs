using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePlayer : MonoBehaviour
{
    int damage;

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
        player = transform.parent.GetComponentInParent<Transform>();
    }

    private void Update()
    {
        if (ko)
        {
            p1Anim.SetBool("hit", true);
            p1bAnim.SetBool("hit", true);
            damage += 10;
            if (damage >= 999) damage = 999;

            if (damage < 200) p1Text.color = p1Text.color - new Color(0.037f, 0.085f, 0.085f, 0f);

            p1Text.text = damage + "%";
            p1bText.text = damage + "%";
            ko = false;
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
            player.position = new Vector3(0, 13, 0);
        }
    }
}
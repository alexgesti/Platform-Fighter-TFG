using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextManager : MonoBehaviour
{
    public GameObject ready, go, success, failure;
    public AudioClip[] clips;
    public MovementBasis player;
    public TargetLogic targets;
    public AudioSource musicBackground;

    float counter, counter2;
    bool oneTime1, oneTime2, isFinished;

    // Start is called before the first frame update
    void Start()
    {
        go.SetActive(false);
        success.SetActive(false);
        failure.SetActive(false);

        player.isFinished = true;
        targets.chronoActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished)
        {
            if (counter < 1.5f) counter += Time.deltaTime;
            else if (counter >= 1.5f)
            {
                ready.SetActive(false);
                go.SetActive(true);

                if (!oneTime2)
                {
                    GetComponent<AudioSource>().PlayOneShot(clips[1]);
                    oneTime2 = true;
                }

                if (counter2 < 1) counter2 += Time.deltaTime;
                else if (counter2 >= 1)
                {
                    go.SetActive(false);
                    player.isFinished = false;
                    targets.chronoActive = true;
                    isFinished = true;
                }
            }

            if (!oneTime1)
            {
                GetComponent<AudioSource>().PlayOneShot(clips[0]);
                oneTime1 = true;
            }
        }

        if (isFinished && player.isFinished)
        {
            if (targets.targets.Length == targets.deactived && !success.activeSelf)
            {
                success.SetActive(true);
                GetComponent<AudioSource>().PlayOneShot(clips[2]);
            }
            else if (targets.targets.Length != targets.deactived && !failure.activeSelf)
            {
                failure.SetActive(true);
                musicBackground.Stop();
                GetComponent<AudioSource>().clip = clips[3];
                GetComponent<AudioSource>().PlayDelayed(0.75f);
            }
        }
    }
}

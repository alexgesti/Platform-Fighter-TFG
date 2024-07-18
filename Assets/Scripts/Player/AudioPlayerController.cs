using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audios;
         
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Jump()
    {
        audioSource.PlayOneShot(audios[0]);
    }

    public void IsGround()
    {
        audioSource.PlayOneShot(audios[1]);
    }

    public void Death()
    {
        audioSource.PlayOneShot(audios[2]);
    }
}

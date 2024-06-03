using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    RawImage background;

    public float XSpeed;
    public float YSpeed;

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        background.uvRect = new Rect(background.uvRect.position + new Vector2(-XSpeed, -YSpeed) * Time.deltaTime, background.uvRect.size);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour
{
    public Text framerateInfo;

    bool stop;
    bool frameByFrame;
    public Text frameByFrameInfo;

    public Text playerPos;
    public Text collPos;

    public Transform player;
    public Transform collision;

    public LineRenderer lineCollision, lineRaycast;
    [HideInInspector] public static bool activeColl;

    public DamagePlayer dScript; 

    private void Awake()
    {
        Application.targetFrameRate = 60;

        dScript = GameObject.FindGameObjectWithTag("Player").GetComponent<DamagePlayer>(); //Add un "if exists p1"
    }

    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        frameByFrame = false;

        lineCollision.positionCount = 5;
        lineCollision.startWidth = 0.1f;
        lineCollision.endWidth = 0.1f;
        lineCollision.material = new Material(Shader.Find("Sprites/Default"));
        lineCollision.startColor = Color.blue;
        lineCollision.endColor = Color.blue;

        lineRaycast.positionCount = 2;
        lineRaycast.startWidth = 0.05f;
        lineRaycast.endWidth = 0.05f;
        lineRaycast.material = new Material(Shader.Find("Sprites/Default"));
        lineRaycast.startColor = Color.red;
        lineRaycast.endColor = Color.red;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Framerate
        float msec = Time.deltaTime * 1000.0f;
        float fps = 1.0f / Time.deltaTime;
        framerateInfo.text = string.Format("{0:0.0}ms; {1:0.}fps", msec, fps);

        // Avance frame a frame F1 && TAB
        if (Input.GetKeyDown(KeyCode.F1)) stop = !stop;
        if (Input.GetKeyDown(KeyCode.Tab) && stop) frameByFrame = !frameByFrame;

        if (stop)
        {
            Time.timeScale = 0;
            frameByFrameInfo.text = "Frame by Frame: enabled";
        }
        else
        {
            Time.timeScale = 1;
            frameByFrameInfo.text = "Frame by Frame: disabled";
        }

        if (frameByFrame)
        {
            Time.timeScale = 1;
            frameByFrame = false;
        }

        // Positions F2
        playerPos.text = string.Format("P: x:{0:0.000} y:{1:0.000} z:{2:0.000}", player.position.x, player.position.y, player.position.z);
        collPos.text = string.Format("C: x:{0:0.000} y:{1:0.000} z:{2:0.000}", collision.position.x, collision.position.y, collision.position.z);

        if (Input.GetKeyDown(KeyCode.F2)) activeColl = !activeColl;
        if (activeColl)
        {
            lineCollision.SetPosition(0, CollisionBox.layer[0] + player.position);
            lineCollision.SetPosition(1, CollisionBox.layer[1] + player.position);
            lineCollision.SetPosition(2, CollisionBox.layer[4] + player.position);
            lineCollision.SetPosition(3, CollisionBox.layer[3] + player.position);
            lineCollision.SetPosition(4, CollisionBox.layer[0] + player.position);

            lineRaycast.SetPosition(0, player.position - new Vector3(0, player.localScale.y / 2, 0)); 
            lineRaycast.SetPosition(1, player.position - new Vector3(0, CollisionBox.maxDistance, 0)); 
        }
        else
        {
            lineCollision.SetPosition(0, Vector3.zero);
            lineCollision.SetPosition(1, Vector3.zero);
            lineCollision.SetPosition(2, Vector3.zero);
            lineCollision.SetPosition(3, Vector3.zero);
            lineCollision.SetPosition(4, Vector3.zero);

            lineRaycast.SetPosition(0, Vector3.zero);
            lineRaycast.SetPosition(1, Vector3.zero);
        }

        // Add damage F3
        if (Input.GetKeyDown(KeyCode.F3)) dScript.ko = true;

        // Put player in air to base
        if (Input.GetKeyDown(KeyCode.F4)) dScript.transform.position = new Vector3(0, 10, 0);

        // Put player in air to platform
        if (Input.GetKeyDown(KeyCode.F5)) dScript.transform.position = new Vector3(4.45f, 15, 0);
    }
}

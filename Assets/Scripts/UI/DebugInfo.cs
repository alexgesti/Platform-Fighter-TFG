using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugInfo : MonoBehaviour
{
    public Text framerateInfo;

    [Header("Frame by frame")] 
    bool stop;
    bool frameByFrame;
    public Text frameByFrameInfo;

    [Header("Positions")]
    public Text playerPos;
    //public Text collPos;
    public Transform player;
    //public Transform collision;

    [Header("Dibujado de colisiones")]
    public List<GameObject> gameObjects;
    List<Material> originalMaterials;
    public Material collisionMaterial;
    //public LineRenderer lineCollision, lineRaycast;
    bool activeColl;

    public DamagePlayer dScript; 

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        frameByFrame = false;

        gameObjects = new List<GameObject>();

        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        gameObjects.AddRange(floors);

        GameObject[] platforms = GameObject.FindGameObjectsWithTag("PlatformF");
        gameObjects.AddRange(platforms);

        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> players = new List<GameObject>();
        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i].GetComponent<MeshCollider>() != null)
            {
                players.Add(allPlayers[i]);
            }
        }
        gameObjects.AddRange(players);


        originalMaterials = new List<Material>();

        foreach (GameObject obj in gameObjects)
        {
            if (obj.CompareTag("Floor") || obj.CompareTag("PlatformF"))
            {
                originalMaterials.Add(obj.GetComponent<MeshRenderer>().material);
            }

            if (obj.CompareTag("Player"))
            {
                obj.GetComponent<LineRenderer>().positionCount = 7;
                obj.GetComponent<LineRenderer>().startWidth = 0.1f;
                obj.GetComponent<LineRenderer>().endWidth = 0.1f;
                obj.GetComponent<LineRenderer>().material = new Material(Shader.Find("Sprites/Default"));
                obj.GetComponent<LineRenderer>().startColor = Color.blue;
                obj.GetComponent<LineRenderer>().endColor = Color.blue;
            }
        }
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

        playerPos.text = string.Format("P: x:{0:0.000} y:{1:0.000} z:{2:0.000}", player.position.x, player.position.y, player.position.z);

        // Collision Pos F2
        if (Input.GetKeyDown(KeyCode.F2)) activeColl = !activeColl;
        if (activeColl)
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj.CompareTag("Player"))
                {
                    obj.GetComponent<LineRenderer>().enabled = true;

                    obj.GetComponent<LineRenderer>().SetPosition(0, CollisionBox.layer[0] + obj.transform.position);
                    obj.GetComponent<LineRenderer>().SetPosition(1, CollisionBox.layer[6] + obj.transform.position);
                    obj.GetComponent<LineRenderer>().SetPosition(2, CollisionBox.layer[1] + obj.transform.position);
                    obj.GetComponent<LineRenderer>().SetPosition(3, CollisionBox.layer[4] + obj.transform.position);
                    obj.GetComponent<LineRenderer>().SetPosition(4, CollisionBox.layer[3] + obj.transform.position);
                    obj.GetComponent<LineRenderer>().SetPosition(5, CollisionBox.layer[7] + obj.transform.position);
                    obj.GetComponent<LineRenderer>().SetPosition(6, CollisionBox.layer[0] + obj.transform.position);
                }
            }
        }
        else
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj.CompareTag("Player"))
                {
                    obj.GetComponent<LineRenderer>().enabled = false;
                }
            }
        }

        // HitBoxes Player F3 // Se debe de acabar de arreglar que solo sea para cada boton, adem�s de que solo entre una vez en algunos de estos adem�s de que los renderer no se apagan (idk el porque).
        if (activeColl)
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj.CompareTag("Player"))
                {
                    foreach (MeshRenderer renderer in obj.GetComponentInChildren<HitBoxesLogic>().hitBVis)
                    {
                        renderer.enabled = true;
                    }
                }
            }
        }
        else
        {

            foreach (GameObject obj in gameObjects)
            {
                if (obj.CompareTag("Player"))
                {
                    foreach (MeshRenderer renderer in obj.GetComponentInChildren<HitBoxesLogic>().hitBVis)
                    {
                        renderer.enabled = false;
                    }
                }
            }
        }

        // No player (With HitBoxes) Tab
        if (activeColl)
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj.CompareTag("Player"))
                {
                    foreach (MeshRenderer model in obj.GetComponentsInChildren<MeshRenderer>())
                    {
                        if (model.tag == "PlayerModel")
                            model.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
        }
        else
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj.CompareTag("Player"))
                {
                    foreach (MeshRenderer model in obj.GetComponentsInChildren<MeshRenderer>())
                    {
                        if (model.tag == "PlayerModel")
                            model.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
        }

        // Scenary Collision F4
        if (activeColl)
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj.CompareTag("Floor") || obj.CompareTag("PlatformF"))
                {
                    obj.GetComponent<MeshRenderer>().material = collisionMaterial;
                }
            }
        }
        else
        {
            int indexG = 0;

            foreach (GameObject obj in gameObjects)
            {
                if (obj.CompareTag("Floor") || obj.CompareTag("PlatformF"))
                {
                    int indexM = 0;

                    foreach (Material returnMaterial in originalMaterials)
                    {
                        if (indexG == indexM) obj.GetComponent<MeshRenderer>().material = returnMaterial;

                        indexM++;
                    }

                }

                indexG++;
            }
        }

        // Add damage F5
        if (Input.GetKeyDown(KeyCode.F5)) dScript.ko = true;

        // Change Scene
        if (Input.GetKeyDown(KeyCode.F6)) SceneManager.LoadScene(0);

        if (Input.GetKeyDown(KeyCode.F7)) SceneManager.LoadScene(1);

        // Exit
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
}

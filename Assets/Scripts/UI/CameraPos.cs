using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    Camera cam;
    public Transform[] players;
    public Transform leftLimit, rightLimit, topLimit, bottomLimit;

    public float padding;
    public float minFOV;
    public float zoomSpeed;
    public float limitPaddingX;
    public float limitPaddingY;

    float leftLimitX;
    float rightLimitX;
    float topLimitY;
    float bottomLimitY;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = false;

        GameObject[] playersObjects = GameObject.FindGameObjectsWithTag("Player");
        players = new Transform[playersObjects.Length];
        for (int i = 0; i < playersObjects.Length; i++)
        {
            players[i] = playersObjects[i].transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLimits();
        Rect boundingBox = CalculatePlayersBox();
        transform.position = CalculateCamPos(boundingBox);
        cam.fieldOfView = CalculateFOVSize(boundingBox);
    }

    void UpdateLimits()
    {
        leftLimitX = leftLimit.position.x + leftLimit.localScale.x + limitPaddingX;
        rightLimitX = rightLimit.position.x - rightLimit.localScale.x - limitPaddingX;
        topLimitY = topLimit.position.y - topLimit.localScale.y - limitPaddingY;
        bottomLimitY = bottomLimit.position.y + bottomLimit.localScale.y + limitPaddingY;
    }

    Rect CalculatePlayersBox()
    {
        Vector2 min = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Vector2 max = new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);

        foreach (Transform player in players)
        {
            min.x = Mathf.Min(min.x, player.position.x);
            min.y = Mathf.Min(min.y, player.position.y);
            max.x = Mathf.Max(max.x, player.position.x);
            max.y = Mathf.Max(max.y, player.position.y);
        }

        return Rect.MinMaxRect(min.x - padding, max.y + padding, max.x + padding, min.y - padding);
    }

    Vector3 CalculateCamPos(Rect box)
    {
        Vector3 newPos = new Vector3(box.center.x, box.center.y, cam.transform.position.z);

        float halfVerticalSize = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2) * Mathf.Abs(newPos.z);
        float halfHorizontalSize = halfVerticalSize * cam.aspect;

        newPos.x = Mathf.Clamp(newPos.x, leftLimitX + halfHorizontalSize, rightLimitX - halfHorizontalSize);
        newPos.y = Mathf.Clamp(newPos.y, bottomLimitY + halfVerticalSize, topLimitY - halfVerticalSize);

        return newPos;
    }

    float CalculateFOVSize(Rect box)
    {
        // Dimensiones de la caja.
        float boxHeight = Mathf.Abs(box.height);
        float boxWidth = Mathf.Abs(box.width);

        // Calculo de FOV deseado.
        float distanceBox = Mathf.Abs(cam.transform.position.z);
        float fovVertical = 2 * Mathf.Atan(boxHeight / (2 * distanceBox)) * Mathf.Rad2Deg;
        float fovHorizontal =  2 * Mathf.Atan(boxWidth / (2 * (distanceBox / cam.aspect))) * Mathf.Rad2Deg;

        // Ajuste de la caja dentro de la cámara
        float desiredFOV = Mathf.Max(fovVertical, fovHorizontal);

        // Ajustar distancia dependiendo de las blastlines.
        float maxAllowedWidth = rightLimitX - leftLimitX - padding * 2;
        float maxAllowedHeight = topLimitY - bottomLimitY - padding * 2;

        float maxAllowedFOVHorizontal = 2 * Mathf.Atan(maxAllowedWidth / (2 * (distanceBox / cam.aspect))) * Mathf.Rad2Deg;
        float maxAllowedFOVVertical = 2 * Mathf.Atan(maxAllowedHeight / (2 * distanceBox)) * Mathf.Rad2Deg;

        return Mathf.Clamp(Mathf.Lerp(cam.fieldOfView, Mathf.Max(fovVertical, fovHorizontal), Time.deltaTime * zoomSpeed), minFOV, Mathf.Min(maxAllowedFOVHorizontal, maxAllowedFOVVertical));
    }
}

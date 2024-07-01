using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    Camera cam;
    public Transform[] players;
    public Transform leftLimit, rightLimit;

    public float padding;
    public float minFOV;
    public float maxFOV;
    public float zoomSpeed;
    public float limitPadding;

    float leftLimitX;
    float rightLimitX;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        leftLimitX = leftLimit.position.x + leftLimit.localScale.x + limitPadding;
        rightLimitX = rightLimit.position.x - rightLimit.localScale.x - limitPadding;

        Rect boundingBox = CalculatePlayersBox();
        transform.position = CalculateCamPos(boundingBox);
        cam.fieldOfView = CalculateFOVSize(boundingBox);
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
        return new Vector3(Mathf.Clamp(box.center.x, leftLimitX + cam.orthographicSize * cam.aspect, rightLimitX - cam.orthographicSize * cam.aspect), box.center.y, cam.transform.position.z);
    }

    float CalculateFOVSize(Rect box)
    {
        float boxHeight = Mathf.Abs(box.height);
        float boxWidth = Mathf.Abs(box.width);
        float distanceBox = Mathf.Abs(cam.transform.position.z);

        float fovVertical = 2 * Mathf.Atan(boxHeight / (2 * distanceBox)) * Mathf.Rad2Deg;
        float fovHorizontal =  2 * Mathf.Atan(boxWidth / (2 * (distanceBox / cam.aspect))) * Mathf.Rad2Deg;

        // Ajustar distancia dependiendo de las blastlines.
        float maxAllowedWidth = rightLimitX - leftLimitX - padding * 2;
        float maxAllowedFov = 2 * Mathf.Atan(maxAllowedWidth / (2 * (distanceBox / cam.aspect))) * Mathf.Rad2Deg;

        return Mathf.Clamp(Mathf.Lerp(cam.fieldOfView, Mathf.Max(fovVertical, fovHorizontal), Time.deltaTime * zoomSpeed), minFOV, Mathf.Min(maxFOV, maxAllowedFov));
    }
}

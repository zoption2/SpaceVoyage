using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    private Transform cameraTransform;
    private float paralaxSpeed = 0.9f;
    private float lastCameraX;
    private float lastCameraY;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        backgroundMove();
    }

    private void backgroundMove()
    {
        float deltaX = cameraTransform.position.x - lastCameraX;
        float deltaY = cameraTransform.position.y - lastCameraY;
        transform.position += (Vector3.right * deltaX * paralaxSpeed) + (Vector3.up * deltaY * paralaxSpeed);
        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;
    }
}

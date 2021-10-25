using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private static int RIGHT_MOUSE_BUTTON = 1;
    private static int CameraMoveButton = RIGHT_MOUSE_BUTTON;

    // private Vector3 mousePosition = new Vector3(0,0,0);
    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;
    private float smoothTime = 0.2f;


    [Header("Settings")]
    public Transform target;
    public bool invert = false;

    // State variables
    [Header("Yaw")] // X-axis
    public float xAngle = 0.0f;
    public float xSensitivity = 3.0f;

    [Header("Pitch")] // "Y"-axis
    public float yAngle = 20.0f;
    public float ySensitivity = 3.0f;
    public float minPitch = 10.0f;
    public float maxPitch = 90.0f;

    [Header("Zoom")]
    public float distance = 5.0f;
    public float zoomSpeed = 5.0f;
    public float minZoom = 1.0f;
    public float maxZoom = 1000.0f;

    void Update() {
        MoveCamera();
    }

    private void MoveCamera() {
        if (Input.GetMouseButton(CameraMoveButton)) {
            float mouseX = Input.GetAxis("Mouse X") * xSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * ySensitivity;

            xAngle += -mouseY;
            yAngle += mouseX;

            if (invert) xAngle *= -1;
        }

        xAngle = Mathf.Clamp(xAngle, minPitch, maxPitch);

        Vector3 nextRotation = new Vector3(xAngle, yAngle);
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;

        // Something similar to this can be used for zoom in and out
        distance += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, minZoom, maxZoom);

        transform.position = target.position - transform.forward * distance;
    }
}

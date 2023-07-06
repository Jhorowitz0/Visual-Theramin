using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera cam;
    private float cameraFov;
    private void OnEnable() {
        cameraFov = cam.fieldOfView;
    }

    private void OnDisable() {
        transform.localPosition = new Vector3(0,transform.localPosition.y,transform.localPosition.z);
    }

    private void Update() {
        cam.fieldOfView = cameraFov + transform.localPosition.x;
    }
}

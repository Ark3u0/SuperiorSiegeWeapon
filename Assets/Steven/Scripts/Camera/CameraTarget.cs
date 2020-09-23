using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [Range(-10f, 10f)]
    public float aboveTargetOffset = 1.0f;

    [Range(1f, 20f)]
    public float defaultZoom = 5f;

    [Range(10f, 20f)]
    public float maxZoomOut = 20f;

    [Range(1f, 10f)]
    public float minZoomIn = 3f;

    [Range(10f, 45f)]
    public float angleFromTarget = 30f;

    [Range(1.0f, 1000.0f)]
    public float rotationSpeed = 5.0f;

    [Range(1.0f, 1000.0f)]
    public float zoomSpeed = 5.0f;

    public bool canCameraZoom = true;

    public bool canCameraRotate = true;

    public bool canCameraLookAt = true;

    // We define zoom in/out constraints
    // look above offset

    void Awake() {
        transform.position = transform.position + Vector3.up * aboveTargetOffset;
    }

    public void UpdateTargetPosition(Vector3 position) {
        transform.position = position;
    }
}

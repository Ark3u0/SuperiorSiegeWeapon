using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOcclusionCollider : MonoBehaviour
{
    private CameraTarget currentTarget;

    void Update() {
        currentTarget = GetComponent<FollowCameraTarget>().cameraTarget;
        CapsuleCollider collider = GetComponent<CapsuleCollider>();

        float distanceBetweenCameraAndTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        collider.height = distanceBetweenCameraAndTarget;
        collider.center = new Vector3(0, 0, collider.height / 2);
    }

    void OnTriggerEnter(Collider other) {
        Renderer renderer = other.gameObject.GetComponent<Renderer>();
        // renderer.mat
        Debug.Log($"[Camera] Trigger Enter: {other.gameObject.name}");
    }

    void OnTriggerExit(Collider other) {
        Renderer renderer = other.gameObject.GetComponent<Renderer>();
        Debug.Log($"[Camera] Trigger Exit: {other.gameObject.name}");
    }
}

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

    void OnCollisionEnter(Collision other) {
        Debug.Log($"[Camera] Collision Enter: {other.gameObject.name}");
    }

    void OnCollisionExit(Collision other) {
        Debug.Log($"[Camera] Collision Exit: {other.gameObject.name}");
    }

    void OnCollisionStay(Collision other) {
        Debug.Log($"[Camera] Collision Stay: {other.gameObject.name}");
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log($"[Camera] Trigger Enter: {other.gameObject.name}");
    }

    void OnTriggerExit(Collider other) {
        Debug.Log($"[Camera] Trigger Exit: {other.gameObject.name}");
    }

    void OnTriggerStay(Collider other) {
        Debug.Log($"[Camera] Trigger Stay: {other.gameObject.name}");
    }
}

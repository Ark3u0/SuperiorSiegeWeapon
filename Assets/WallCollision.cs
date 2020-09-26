using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
  
    void OnCollisionEnter(Collision other) {
        Debug.Log($"[Wall] Collision Enter: {other.gameObject.name}");
    }

    void OnCollisionExit(Collision other) {
        Debug.Log($"[Wall] Collision Exit: {other.gameObject.name}");
    }

    void OnCollisionStay(Collision other) {
        Debug.Log($"[Wall] Collision Stay: {other.gameObject.name}");
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log($"[Wall] Trigger Enter: {other.gameObject.name}");
    }

    void OnTriggerExit(Collider other) {
        Debug.Log($"[Wall] Trigger Exit: {other.gameObject.name}");
    }

    void OnTriggerStay(Collider other) {
        Debug.Log($"[Wall] Trigger Stay: {other.gameObject.name}");
    }
}

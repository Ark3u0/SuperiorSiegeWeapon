using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // private Rigidbody rigidBody;

    void Start() 
    {
        // rigidBody = GetComponent<Rigidbody>();
    }

    public void PlaceForKick(Transform playerTransform) 
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // Stop movement of ball
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.5f, playerTransform.position.z) + playerTransform.forward * 0.5f;
    }
}

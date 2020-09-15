using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DribbleCollider : MonoBehaviour
{
    public CharacterController controller;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Debug.Log("BALL DRIBBLE COLLIDER HIT");
        }
    }
}

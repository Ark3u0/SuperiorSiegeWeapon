using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollisionCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9f)// 9 is the ball layer
        {
            Debug.Log("Ball in well");
            // renturn 
        }
        else
        {

            Debug.Log("Ball not in well");
        }
    }

}

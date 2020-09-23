using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCollider : MonoBehaviour
{
    public CharacterController controller;

    public void OnTriggerEnter(Collider other) {
        Ball ball = other.gameObject.GetComponent<Ball>();
        if (ball != null) {
            controller.GetComponent<PlayerController>().Ball(ball);
        }

        NpcController npc = other.gameObject.GetComponent<NpcController>();
        if (npc != null) {
            controller.GetComponent<PlayerController>().Npc(npc);     
        }
    }

    public void OnTriggerExit(Collider other) {
        Ball ball = other.gameObject.GetComponent<Ball>();
        if (ball != null) {
            controller.GetComponent<PlayerController>().NoBall();
        }

        NpcController npc = other.gameObject.GetComponent<NpcController>();
        if (npc != null) {
            controller.GetComponent<PlayerController>().NoNpc();     
        }
    }
}

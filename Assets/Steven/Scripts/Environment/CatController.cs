using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public CharacterController controller;
    public float GRAVITY = 9.81f;
    private bool gravityOn;

    public void Awake() {
        gravityOn = false;
    }

    public void Update() {
        Vector3 toMove = Vector3.zero;
        if (!controller.isGrounded && gravityOn) {
            toMove.y -= GRAVITY * Time.deltaTime;
        }

        controller.Move(toMove);
    }

    public void TurnOnGravity() {
        gravityOn = true;
    }

    public void TurnOffGravity() {
        gravityOn = true;
    }
}

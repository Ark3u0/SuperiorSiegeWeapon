using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// Reference: Brackey's - https://www.youtube.com/watch?v=4HpC--2iowE
public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    
    public PlayerInputActions controls;

    public Ball ball;

    public Transform cam;
    public float speed = 6.0f;
    public float gravity = 9.81f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    bool kicking = false;

    Vector2 movement;
    Vector3 heading;

    void Awake() {
        controls = new PlayerInputActions();

        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = ctx.ReadValue<Vector2>();
        // controls.Player.Kick.performed += Kick;
    }

    // void Kick()
    // {
    //     if (CanKick())
    //     {
    //         this.kicking = true;
    //         this.ball.Kick(heading);
    //     }
    // }

    void Update()
    {
        if (CanKick()) 
        {
            // TODO: Trigger kick animation
        }

        // if (!kicking) {
            HandleMovement();
        // }
    }

    public void HandleMovement() {
        Vector3 direction = new Vector3(movement.x, 0f, movement.y).normalized;
        Vector3 toMove = new Vector3(0, 0, 0);

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            toMove = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * speed * Time.deltaTime;
        }

        if (!controller.isGrounded) {
            toMove.y -= gravity * Time.deltaTime;
        }

        controller.Move(toMove);
    }

    public bool CanKick()
    {
        return this.ball != null;
    }

    public void Ball(Ball ball)
    {
        this.ball = ball;
    }

    public void NoBall() {
        this.ball = null;
    }

    void OnEnable() {
        controls.Player.Enable();
    }

    void OnDisable() {
        controls.Player.Disable();
    }
}

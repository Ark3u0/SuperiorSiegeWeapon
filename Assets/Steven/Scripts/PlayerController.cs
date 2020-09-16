using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// Reference: Brackey's - https://www.youtube.com/watch?v=4HpC--2iowE
public class PlayerController : MonoBehaviour
{
    public KickTrajectoryRenderer kickTrajectoryRenderer;

    public CharacterController controller;
    
    public PlayerInputActions controls;

    public Ball ball;

    public Transform cam;
    public float speed = 6.0f;
    public float gravity = 9.81f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float maxKickVelocity = 50f;
    public float minKickVelocity = 25f;

    public float maxKickAngle = 70f;

    public float minKickAngle = 30f;
    
    [Range(1f, 100f)]
    public float kickVelocitySensitivity = 100f;

     [Range(1f, 100f)]
    public float kickAngleSensitivity = 100f;

    [Range(1f, 100f)]
    public float kickRotationSensitivity = 100f;   

    float kickVelocity = 1f;

    float kickAngle = 30f;

    Vector3 kickHeading;

    bool aiming = false;

    Vector2 movement;

    Vector2 look;

    void Awake() {
        controls = new PlayerInputActions();

        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => look = ctx.ReadValue<Vector2>();
        controls.Player.Kick.started += AimKick;
        controls.Player.Kick.canceled += Kick;
    }

    void AimKick(InputAction.CallbackContext context)
    {        
        if (CanKick())
        {
            this.aiming = true;
            this.ball.PlaceForKick(transform);
            this.kickHeading = transform.forward;
        }
    }

    void Kick(InputAction.CallbackContext context) 
    {
        if (this.aiming) {
            this.aiming = false;
            this.kickTrajectoryRenderer.rendering = false;

            // Set velocity, angle, heading on ball to kick
            // unlock freeze constraints

            kickVelocity = 1f;
            kickAngle = 10f;
            kickHeading = transform.forward;
        }
    }

    void Update()
    {
        if (this.aiming) {
            HandleAiming();
        }

        if (!this.aiming) {
            HandleMovement();
        }
    }

    public void HandleAiming() {
        if (Mathf.Abs(movement.y) > .5) {
            kickVelocity = Mathf.Max(minKickVelocity, Mathf.Min(maxKickVelocity, kickVelocity + movement.y * 100f * kickVelocitySensitivity * Time.deltaTime));
        }
        
        if (Mathf.Abs(look.y) > .5) {
            kickAngle = Mathf.Max(minKickAngle, Mathf.Min(maxKickAngle, kickAngle + look.y * 100f * kickAngleSensitivity * Time.deltaTime));       
        }

        Quaternion rotateKickHeading = Quaternion.AngleAxis(movement.x * 100f * kickRotationSensitivity * Time.deltaTime, Vector3.up);

        kickHeading = (rotateKickHeading * kickHeading).normalized;

        kickTrajectoryRenderer.position = ball.transform.position;
        kickTrajectoryRenderer.heading = kickHeading;
        kickTrajectoryRenderer.velocity = kickVelocity;
        kickTrajectoryRenderer.angle = kickAngle;
        kickTrajectoryRenderer.rendering = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// Reference: Brackey's - https://www.youtube.com/watch?v=4HpC--2iowE
public class PlayerController : MonoBehaviour
{
    public CameraTarget cameraTarget;
    public CharacterController controller;
    public PlayerInputActions controls;
    public Ball ball;
    public NpcController npc;
    public Camera cam;
    public float speed = 6.0f;
    public float gravity = 9.81f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public PlayerState playerState;
    Vector2 movement;
    Vector2 look;

    float angleKick;

    void Awake() {
        controls = new PlayerInputActions();

        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.AngleKick.performed += ctx => angleKick = ctx.ReadValue<float>();
        controls.Player.AngleKick.canceled += ctx => angleKick = 0f;

        controls.Player.Kick.started += Aim;
        controls.Player.Kick.canceled += Kick;

        controls.Player.Interact.started += Talk;

        cam.GetComponent<FollowCameraTarget>().SetCameraTarget(cameraTarget);
    }

    void Aim(InputAction.CallbackContext context)
    {        
        if (CanAim())
        {
            playerState = PlayerState.AIMING;
            ball.PlaceForKick(transform);
        }
    }

    void Talk(InputAction.CallbackContext context)
    {
        if (playerState != PlayerState.TALKING && CanTalk())
        {
            playerState = PlayerState.TALKING;
            bool isEndOfConversation = npc.StartConversation(transform);
            if (isEndOfConversation) playerState = PlayerState.MOVING;
        } 
        else if (playerState == PlayerState.TALKING)
        {
            bool isEndOfConversation = npc.ContinueConversation();
            if (isEndOfConversation) playerState = PlayerState.MOVING;
        }

        
    }

    void Kick(InputAction.CallbackContext context) 
    {
        if (CanKick()) {
            playerState = PlayerState.MOVING;
            this.ball.Kick();
            cam.GetComponent<FollowCameraTarget>().SetCameraTarget(cameraTarget);
        }
    }

    void Update()
    {
        if (playerState == PlayerState.AIMING) HandleAiming();

        if (playerState == PlayerState.MOVING) HandleMovement();
    }

    public void HandleAiming() {
        Vector3 moveInput = new Vector3(movement.x, 0f, movement.y).normalized;
        float angleInput = angleKick;

        this.ball.Aim(moveInput, angleInput, cam);
    }

    public void HandleMovement() {
        Vector3 direction = new Vector3(movement.x, 0f, movement.y).normalized;
        Vector3 toMove = new Vector3(0, 0, 0);

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
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
        return ball != null && playerState == PlayerState.AIMING;
    }

    public bool CanAim()
    {
        return ball != null;
    }

    public bool CanTalk()
    {
        return npc != null;
    }

    public void Npc(NpcController npc)
    {
        this.npc = npc;
    }

    public void Ball(Ball ball)
    {
        this.ball = ball;
    }

    public void NoNpc() {
        if (playerState != PlayerState.TALKING) {
            this.npc = null;
        }
    }
 
    public void NoBall() {
        if (playerState != PlayerState.AIMING) {
            this.ball = null;
        }
    }

    void OnEnable() {
        controls.Player.Enable();
    }

    void OnDisable() {
        controls.Player.Disable();
    }

    public enum PlayerState {
        MOVING,
        TALKING,
        AIMING
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : Action
{
    private ActionStateMachine actions;
    private PlayerController player;
    private PlayerInputActions controls;
    private Vector2 movement;
    public float GRAVITY = 9.81f;
    private bool aimTriggered = false;
    private bool talkTriggered = false;
    private bool ballResetTriggerer = false;


    public PlayerMoving(PlayerController player, ActionStateMachine actions)
    {
        this.controls = new PlayerInputActions();
        this.actions = actions;
        this.player = player;

        controls.Player.Kick.started += ctx => aimTriggered = true;

        controls.Player.Move.started += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = ctx.ReadValue<Vector2>();

        controls.Player.Interact.started += ctx => talkTriggered = true;
        controls.Player.ResetBall.started += ctx => ballResetTriggerer = true;
    }

    public void PostAction()
    {
        controls.Player.Kick.Disable();
        controls.Player.Move.Disable();
        controls.Player.Interact.Disable();
        controls.Player.ResetBall.Disable();
    }

    public void PreAction(Dictionary<string, object> changeParams)
    {
        controls.Player.Kick.Enable();
        controls.Player.Move.Enable();
        controls.Player.Interact.Enable();
        controls.Player.ResetBall.Enable();
    }

    public bool CheckForActionChange()
    {
        if (aimTriggered && CanAim()) {
            actions.Change("aiming", new Dictionary<string, object>());
            return true;
        }
        if (talkTriggered && CanTalk()) {
            actions.Change("talking", new Dictionary<string, object>());
            return true;
        }
        if (ballResetTriggerer) {
            actions.Change("resetingBall", new Dictionary<string, object>());
            return true;
        }

        aimTriggered = false;
        talkTriggered = false;
        ballResetTriggerer = false;

        return false;
    }

    public void Update()
    {
        if (CheckForActionChange()) return;

        Vector3 direction = new Vector3(movement.x, 0f, movement.y).normalized;
        Vector3 toMove = new Vector3(0, 0, 0);

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + player.cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref player.turnSmoothVelocity, player.turnSmoothTime);
            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            toMove = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * player.speed * Time.deltaTime;
        }

        if (!player.controller.isGrounded) {
            toMove.y -= GRAVITY * Time.deltaTime;
        }

        player.controller.Move(toMove);
    }

    private bool CanAim()
    {
        return player.ball != null;
    }

    private bool CanTalk()
    {
        return player.npc != null;
    }

    public string Name()
    {
        return "moving";
    }
}

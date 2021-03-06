﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : Action
{
    public PlayerInputActions controls;
    private ActionStateMachine actions;
    private PlayerController player;
    private Camera cam;
    private InputBoxManager inputBoxManager;
    private Vector2 movement;
    private float angleKick;

    private bool kickTriggered = false;
    
    public PlayerAiming(PlayerController player, ActionStateMachine actions, InputBoxManager inputBoxManager, Camera cam) {
        this.controls = new PlayerInputActions();
        this.cam = cam;
        this.inputBoxManager = inputBoxManager;
        this.actions = actions;
        this.player = player;

        controls.Player.Kick.canceled += ctx => kickTriggered = true;

        controls.Player.Move.started += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = ctx.ReadValue<Vector2>();

        controls.Player.AngleKick.started += ctx => angleKick = ctx.ReadValue<float>();
        controls.Player.AngleKick.performed += ctx => angleKick = ctx.ReadValue<float>();
        controls.Player.AngleKick.canceled += ctx => angleKick = ctx.ReadValue<float>();

    }

    public void PostAction()
    {
        controls.Player.Move.Disable();
        controls.Player.Kick.Disable();
        controls.Player.AngleKick.Disable();
        inputBoxManager.HideInputs();
    }

   public void PreAction(Dictionary<string, object> changeParams)
    {
        player.SpriteAnimation().SetState(SpriteState.IDLE);

        controls.Player.Move.Enable();
        controls.Player.Kick.Enable();
        controls.Player.AngleKick.Enable();

        player.ball.PlaceForKick(player.transform);
        inputBoxManager.ShowInputs(new List<string> {
            "Kick",
            "Angle ↓",
            "Angle ↑",
            "Target"
        });
    }

    public void Update()
    {
        if (CheckForActionChange()) return;

        player.ball.Aim(
            new Vector3(movement.x, 0f, movement.y).normalized, 
            angleKick, 
            cam);
    }

    public bool CheckForActionChange()
    {
        if (kickTriggered && CanKick())
        {
            actions.Change("kicking", new Dictionary<string, object>());
            return true;
        }

        kickTriggered = false;

        return false;
    }

    private bool CanKick()
    {
        return player.ball != null;
    }

    public string Name()
    {
        return "aiming";
    }
}

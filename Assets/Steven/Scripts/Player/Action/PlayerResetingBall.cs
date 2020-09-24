﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerResetingBall : Action
{
    public PlayerInputActions controls;
    private ActionStateMachine actions;
    private PlayerController player;
    
    public PlayerResetingBall(PlayerController player, ActionStateMachine actions) {
        this.controls = new PlayerInputActions();
        this.actions = actions;
        this.player = player;
    }

    public void PostAction()
    {
    }

    public void PreAction(Dictionary<string, object> changeParams)
    {
        if (player.ballToReset != null)
        {
            player.ballToReset.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            player.ballToReset.transform.position = player.transform.position + new Vector3(0, 5, 0);
        }
    }

    public void Update()
    {
        if (CheckForActionChange()) return;
    }

    public bool CheckForActionChange()
    {
        actions.Change("moving", new Dictionary<string, object>());
        return true;
    }

    public string Name()
    {
        return "resetingBall";
    }
}

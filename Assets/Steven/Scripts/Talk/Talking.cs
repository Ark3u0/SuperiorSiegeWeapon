using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Talking : Action
{
    public PlayerController player;
    private ActionStateMachine actions;
    private PlayerInputActions controls;
    private Vector2 movement;
    private bool talkTriggered = false;
    private bool isConversationEnded = false;

    public Talking(PlayerController player, ActionStateMachine actions)
    {
        this.controls = new PlayerInputActions();
        this.actions = actions;
        this.player = player;

        controls.Player.Move.started += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = ctx.ReadValue<Vector2>();

        controls.Player.Interact.started += ctx => talkTriggered = true;

    }

    public bool CheckForActionChange()
    {
        if (isConversationEnded) {
            actions.Change("moving", new Dictionary<string, object>());
            return true;
        }

        return false;
    }

    public void PostAction()
    {
        controls.Player.Move.Disable();
        controls.Player.Interact.Disable();
    }

    public void PreAction(Dictionary<string, object> preParams)
    {
        controls.Player.Move.Enable();
        controls.Player.Interact.Enable();

        isConversationEnded = player.npc.StartConversation(player.transform);
    }

    public void Update()
    {
        if (CheckForActionChange()) return;

        if (talkTriggered) {
            isConversationEnded = player.npc.ContinueConversation();
        }

        Debug.Log(movement.y);
        if (Mathf.Abs(movement.y) > 0.1f) {
            player.npc.Answer(movement.y > 0);
        }

        talkTriggered = false;
    }

    public string Name()
    {
        return "talking";
    }
}

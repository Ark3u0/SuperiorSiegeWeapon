﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// Reference: Brackey's - https://www.youtube.com/watch?v=4HpC--2iowE
public class PlayerController : MonoBehaviour
{
    public CameraTarget cameraTarget;
    public CharacterController controller;
    public PlayerInputActions controls;
    private ActionStateMachine actions;
    private InputBoxManager inputBoxManager;
    private Ball ballToReset;
    private Camera cam;
    public Ball ball;
    public NpcController npc;
    public float speed = 6.0f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    void Awake() {
        controls = new PlayerInputActions();
        actions = new ActionStateMachine();
        inputBoxManager = FindInputBoxManagerInScene();
        ballToReset = FindBallInScene();
        cam = FindCameraInScene();

        actions.Initialize("moving", new Dictionary<string, System.Func<Action>> {
            { "moving", () => new PlayerMoving(this, actions, inputBoxManager, cam) },
            { "aiming", () => new PlayerAiming(this, actions, inputBoxManager, cam) },
            { "kicking", () => new PlayerKicking(this, actions) },
            { "talking", () => new PlayerTalking(this, actions) },
            { "resetingBall", () => new PlayerResetingBall(this, actions) }
        });

        CameraFollowPlayer();
    }

    public Camera AttachedCamera()
    {
        return cam;
    }

    private Camera FindCameraInScene() {
        Camera cam = GameObject.FindObjectOfType<Camera>();
        if (cam == null) {
            Debug.LogError("[PlayerController] expected Camera to exist in scene. Please add Camera and required dependencies to scene and rebuild.");
            throw new System.Exception("[PlayerController] Missing dependency: (Camera)");
        }
        return cam;
    }

    private InputBoxManager FindInputBoxManagerInScene() {
        InputBoxManager inputBoxManager = GameObject.FindObjectOfType<InputBoxManager>();
        if (inputBoxManager == null) {
            Debug.LogError("[PlayerController] expected inputBoxManager to exist in scene. Please add InputBoxManager and required dependencies to scene and rebuild.");
            throw new System.Exception("[PlayerController] Missing dependency: (InputBoxManager)");
        }
        return inputBoxManager;
    }

    private Ball FindBallInScene() {
        Ball ball = GameObject.FindObjectOfType<Ball>();
        if (ball == null) {
            Debug.LogError("[PlayerController] expected Ball to exist in scene. Please add Ball and required dependencies to scene and rebuild.");
            throw new System.Exception("[PlayerController] Missing dependency: (Ball)");
        }
        return ball;
    }

    public void ResetBallToPlayer()
    {
        if (ballToReset != null)
        {
            ballToReset.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            ballToReset.transform.position = transform.position + new Vector3(0, 5, 0);
        }
    }

    public void CameraFollowPlayer()
    {
        cam.GetComponent<FollowCameraTarget>().SetCameraTarget(cameraTarget);
    }

    void Update()
    {
        actions.Update();
    }

    public void SetNpc(NpcController npc)
    {
        if (actions.current.Name() != "talking") {
            this.npc = npc;
        }
    }
 
    public void SetBall(Ball ball) {
        if (actions.current.Name() != "aiming") {
            if (ball != null) {
                ballToReset = ball;
            }
            this.ball = ball;
        }
    }
}
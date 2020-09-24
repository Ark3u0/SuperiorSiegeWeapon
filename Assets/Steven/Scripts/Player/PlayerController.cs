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
    private ActionStateMachine actions;
    public Ball ball;
    public NpcController npc;
    public Camera cam;
    public float speed = 6.0f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    void Awake() {
        controls = new PlayerInputActions();
        actions = new ActionStateMachine();

        actions.Initialize("moving", new Dictionary<string, System.Func<Action>> {
            { "moving", () => new PlayerMoving(this, actions) },
            { "aiming", () => new PlayerAiming(this, actions) },
            { "kicking", () => new PlayerKicking(this, actions) },
            { "talking", () => new PlayerTalking(this, actions) },
            // { "resetingBall", () => new PlayerResetingBall(this, actions) }
        });

        CameraFollowPlayer();
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
            this.ball = ball;
        }
    }
}

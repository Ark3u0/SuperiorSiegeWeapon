using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public KickTrajectoryRenderer kickTrajectoryRenderer;
    public Vector3 kickTarget;
    public float maxKickAngle = 70f;

    public float minKickAngle = 30f;
    
    [Range(1f, 10f)]
    public float kickTargetMoveSensitivity = 10f;

    [Range(1f, 10f)]
    public float kickAngleSensitivity = 10f;

    float kickAngle = 30f;

    public BallState ballState;

    private Rigidbody rigidBody;
    private float dragSetting = 0.5f;

    void Start() 
    {
        rigidBody = GetComponent<Rigidbody>();
        dragSetting = rigidBody.drag;
    }

    void OnCollisionEnter(Collision other) {
        if (ballState == BallState.IN_AIR_FROM_KICK) {
            rigidBody.drag = dragSetting;
            ballState = BallState.DEFAULT_PLAY;
        }
    }

    void Update()
    {
        switch (ballState) {
            case BallState.PLACED_FOR_KICK:
                 // Is ball "grounded"
                if(rigidBody.velocity.y > 0 && rigidBody.velocity.y < .1)
                {
                    ResetAimData();
                    ballState = BallState.READY_FOR_AIM;
                }
                break;
            default:
                break;
        }
    }

    public void Aim(Vector3 moveInput, Vector3 lookInput, Camera cam)
    {
        if (ballState == BallState.READY_FOR_AIM) {
        
            if (moveInput.magnitude > 0.1f) {
                float targetAngle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                
                kickTarget = kickTarget + (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * kickTargetMoveSensitivity * Time.deltaTime;
            }
            
            if (lookInput.magnitude > 0.1f) {
                kickAngle = Mathf.Max(minKickAngle, Mathf.Min(maxKickAngle, kickAngle + lookInput.y * 10f * kickAngleSensitivity * Time.deltaTime));       
            }

            kickTrajectoryRenderer.Render(transform.position, kickTarget, kickAngle);
        }
        
    }

    void ResetAimData() 
    {
        if (transform.parent != null) {
            // TODO: for declines this will float after arc trajectory - how to get collision to track ball with ground?
            kickTarget = transform.position + 3 * transform.parent.forward.normalized;
        }
        kickAngle = 30f;    
    }

    public void Kick()
    {
        if (ballState == BallState.READY_FOR_AIM) {
            // Stop renderering kick trajectory
            kickTrajectoryRenderer.StopRendering();

            // Calculate velocity magnitude and direction
            float angleInRadians = Mathf.Deg2Rad * kickAngle;

            float velocityMagnitude = kickTrajectoryRenderer.CalculateVelocityMagnitude(angleInRadians, transform.position, kickTarget);
            Vector3 velocityDirection = kickTrajectoryRenderer.CalculateVelocityDirection(1, angleInRadians, velocityMagnitude, transform.position, kickTarget).normalized;

            Debug.Log(velocityMagnitude);
            Debug.Log(velocityDirection);
            
            // Unfreeze constraints and set velocity
            rigidBody.constraints = RigidbodyConstraints.None;
            rigidBody.velocity = velocityDirection * velocityMagnitude;

            // Remove drag until next collision
            rigidBody.drag = 0;
            ballState = BallState.IN_AIR_FROM_KICK;

            // Reset aim data
            ResetAimData();
            transform.SetParent(null);
        }
        
    }

    public void PlaceForKick(Transform kicker) 
    {
        // Stop movement of ball and constraint XZ
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        // Set position of ball in front of player and let drop
        transform.position = new Vector3(kicker.position.x, kicker.position.y + 0.5f, kicker.position.z) + kicker.forward * 0.5f;

        // Parent ball
        transform.SetParent(kicker);

        ballState = BallState.PLACED_FOR_KICK;
    }

    public enum BallState
    {
        DEFAULT_PLAY,
        PLACED_FOR_KICK,
        READY_FOR_AIM,
        IN_AIR_FROM_KICK
    }
}

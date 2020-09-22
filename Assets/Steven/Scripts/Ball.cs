using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public KickTrajectoryRenderer kickTrajectoryRenderer;
    public Vector3 kickTarget;

    public float maxKickDistance = 12f;
    public float minKickDistance = 2f;
    public float maxDeviationFromForward = 60f;
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
        // Reenable drag after the first collision
        if (ballState == BallState.IN_AIR_FROM_KICK) {
            rigidBody.drag = dragSetting;
            ballState = BallState.DEFAULT_PLAY;
        }
    }

    public void Aim(Vector3 moveInput, float angleInput, Camera cam)
    {
        if (ballState == BallState.AIMING) {
        
            if (moveInput.magnitude > 0.1f) {
                float targetAngle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                
                Vector3 newKickTarget = kickTarget + (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * kickTargetMoveSensitivity * Time.deltaTime;
                Vector3 displacement = newKickTarget - transform.position;

                float signedAngle = Vector3.SignedAngle(transform.parent.forward, displacement, Vector3.up);
               
                float kickDistance = Mathf.Max(minKickDistance, Mathf.Min(maxKickDistance, displacement.magnitude));

                if (Mathf.Abs(signedAngle) > maxDeviationFromForward) {
                    // GET TO ALIGN WITH SHOOT BOUNDARY LEFT/RIGHT
                    kickTarget = transform.position + (Quaternion.AngleAxis(Mathf.Sign(signedAngle) * maxDeviationFromForward, Vector3.up) * transform.parent.forward).normalized * kickDistance;
                } else {
                    kickTarget = transform.position + displacement.normalized * kickDistance;
                }
                
            }
            
            if (angleInput != 0f) {
                kickAngle = Mathf.Max(minKickAngle, Mathf.Min(maxKickAngle, kickAngle + angleInput * 10f * kickAngleSensitivity * Time.deltaTime));       
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
        if (ballState == BallState.AIMING) {
            // Stop renderering kick trajectory
            kickTrajectoryRenderer.StopRendering();

            // Calculate velocity magnitude and direction
            float angleInRadians = Mathf.Deg2Rad * kickAngle;

            float velocityMagnitude = kickTrajectoryRenderer.CalculateVelocityMagnitude(angleInRadians, transform.position, kickTarget);
            Vector3 velocityDirection = kickTrajectoryRenderer.CalculateVelocityDirection(1, angleInRadians, velocityMagnitude, transform.position, kickTarget).normalized;
            
            // Set velocity
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

        // Set position of ball in front of player and let drop
        transform.position = new Vector3(kicker.position.x, kicker.position.y + 0.5f, kicker.position.z) + kicker.forward * 0.5f;

        // Parent ball
        transform.SetParent(kicker);

        ResetAimData();
        ballState = BallState.AIMING;
    }

    public enum BallState
    {
        DEFAULT_PLAY,
        AIMING,
        IN_AIR_FROM_KICK
    }
}

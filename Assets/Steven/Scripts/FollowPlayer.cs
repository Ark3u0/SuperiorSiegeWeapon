using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Reference: https://www.youtube.com/watch?v=xcn7hz7J7sI
public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;

    public bool LookAtPlayer = true;

    public bool RotateAroundPlayer = true;

    [Range(1.0f, 1000.0f)]
    public float RotationSpeed = 5.0f;

    public PlayerInputActions controls;

    public Vector2 lookInput;

    void Awake()
    {
        controls = new PlayerInputActions();

        controls.Player.Look.performed += OnLook;
        controls.Player.Look.canceled += OnLook;
        controls.Player.Look.started += OnLook;
    }

    void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (RotateAroundPlayer) {
            Quaternion camRotateAngle = Quaternion.AngleAxis(lookInput.x * RotationSpeed * Time.deltaTime, Vector3.up);

            _cameraOffset = camRotateAngle *_cameraOffset;
        }

        Vector3 newPosition = playerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        if (LookAtPlayer) {
            transform.LookAt(playerTransform);
        }
        
    }

    private void OnEnable() {
        controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }
}

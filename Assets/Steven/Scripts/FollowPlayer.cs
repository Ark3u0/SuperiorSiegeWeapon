using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Reference: https://www.youtube.com/watch?v=xcn7hz7J7sI
public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;

    private Vector3 cameraOffset;

    [Range(-10f, 10f)]
    public float aboveTargetOffset = 1.0f;

    [Range(1f, 20f)]
    public float defaultZoom = 5f;

    [Range(10f, 20f)]
    public float maxZoomOut = 20f;

    [Range(1f, 10f)]
    public float minZoomIn = 3f;

    [Range(10f, 45f)]
    public float angleFromPlayer = 30f;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 1.0f;

    public bool lookAtPlayer = true;

    public bool rotateAroundPlayer = true;

    public bool zoomInOnPlayer = true;

    [Range(1.0f, 1000.0f)]
    public float rotationSpeed = 5.0f;

    [Range(1.0f, 1000.0f)]
    public float zoomSpeed = 5.0f;

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
        ResetToDefaultZoom();
    }

    void ResetToDefaultZoom()
    {
        transform.position = playerTransform.position + (Quaternion.AngleAxis(angleFromPlayer, Vector3.forward) * Vector3.right) * defaultZoom;
        cameraOffset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (zoomInOnPlayer) {
            float offsetMagnitude = Mathf.Min(maxZoomOut, Mathf.Max(minZoomIn, cameraOffset.magnitude - lookInput.y * zoomSpeed * Time.deltaTime));
            cameraOffset = cameraOffset.normalized * offsetMagnitude;
            transform.position = playerTransform.position + cameraOffset;
        }

        if (rotateAroundPlayer) {
            Quaternion camRotateAngle = Quaternion.AngleAxis(lookInput.x * rotationSpeed * Time.deltaTime, Vector3.up);

            cameraOffset = camRotateAngle * cameraOffset;
        }

        Vector3 newPosition = playerTransform.position + cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        if (lookAtPlayer) {
            transform.rotation = Quaternion.LookRotation(playerTransform.position + new Vector3(0, aboveTargetOffset, 0) - transform.position);
        }
    }

    private void OnEnable() {
        controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }
}

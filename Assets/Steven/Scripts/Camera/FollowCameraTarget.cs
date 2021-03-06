﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Reference: https://www.youtube.com/watch?v=xcn7hz7J7sI
public class FollowCameraTarget : MonoBehaviour
{
    public CameraTarget cameraTarget;

    private Vector3 cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 1.0f;

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
        transform.position = cameraTarget.transform.position + (Quaternion.AngleAxis(cameraTarget.angleFromTarget, Vector3.forward) * Vector3.right) * cameraTarget.defaultZoom;
        cameraOffset = transform.position - cameraTarget.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cameraTarget.canCameraZoom) {
            ZoomOnTarget();    
        }

        if (cameraTarget.canCameraRotate) {
            RotateAroundTarget();
        }

        transform.position = Vector3.Slerp(transform.position, cameraTarget.transform.position + cameraOffset, smoothFactor);

        if (cameraTarget.canCameraLookAt) {
            LookAtTarget();
        }
    }

    void RotateAroundTarget() {
        Quaternion camRotateAngle = Quaternion.AngleAxis(lookInput.x * cameraTarget.rotationSpeed * Time.deltaTime, Vector3.up);
        cameraOffset = camRotateAngle * cameraOffset;
    }

    void ZoomOnTarget() {
        float offsetMagnitude = Mathf.Min(cameraTarget.maxZoomOut, Mathf.Max(cameraTarget.minZoomIn, cameraOffset.magnitude - lookInput.y * cameraTarget.zoomSpeed * Time.deltaTime));
        cameraOffset = cameraOffset.normalized * offsetMagnitude;
        transform.position = cameraTarget.transform.position + cameraOffset;
    }

    void LookAtTarget() {
        transform.rotation = Quaternion.LookRotation(cameraTarget.transform.position + new Vector3(0, cameraTarget.aboveTargetOffset, 0) - transform.position);
    }

    public void SetCameraTarget(CameraTarget newCameraTarget) {
        if (this.cameraTarget.GetInstanceID() != newCameraTarget.GetInstanceID()) 
        {
            this.cameraTarget = newCameraTarget;
        }   
    }


    private void OnEnable() {
        controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }
}

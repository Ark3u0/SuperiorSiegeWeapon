using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineFreeLook))]
public class OnLookAddOn : MonoBehaviour
{
    private CinemachineFreeLook freelook;

    public PlayerInputActions controls;

    void Awake()
    {
        freelook = GetComponent<CinemachineFreeLook>();

        controls = new PlayerInputActions();

        controls.Player.Look.performed += OnLook;
        controls.Player.Look.canceled += OnLook;
        controls.Player.Look.started += OnLook;
    }

    void OnLook(InputAction.CallbackContext context)
    {
        Vector2 look = context.ReadValue<Vector2>().normalized;

        freelook.m_XAxis.Value = look.x * 180f * Time.deltaTime;
        freelook.m_YAxis.Value = look.y * Time.deltaTime;
    }

    private void OnEnable() {
        controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }

    
}

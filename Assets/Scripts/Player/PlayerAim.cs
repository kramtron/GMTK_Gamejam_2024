using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 aimInput;
    private InputMapping controls;

    void Awake()
    {
        controls = new InputMapping();
    }

    void OnEnable()
    {
        controls.Player.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Aim();
    }

    void Aim()
    {
        Vector3 aimDirection;

        if (Gamepad.current != null && aimInput.magnitude > 0.1f)
        {
            // Gamepad
            aimDirection = new Vector3(aimInput.x, aimInput.y, 0f);
        }
        else
        {
            // Mouse
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(aimInput);
            aimDirection = mousePosition - transform.position;
            aimDirection.z = 0f;
        }

        if (aimDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}

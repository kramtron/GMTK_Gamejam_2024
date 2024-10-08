using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Vector2 aimInput;
    [HideInInspector] public Vector3 aimDirection;
    private InputMapping controls;
    private ArmController armController;

    void Awake()
    {
        controls = new InputMapping();
    }

    void OnEnable()
    {
        controls.Player.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Enable();
        armController = gameObject.GetComponentInChildren<ArmController>();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        if (armController.currentState != ArmState.Waiting && armController.currentState != ArmState.GapClosing)
        {
            Aim();
        }
    }

    void Aim()
    {
        if (Gamepad.current != null && aimInput.magnitude > 0.1f)
        {
            // Gamepad
            aimDirection = new Vector3(aimInput.x, aimInput.y, 0f);
        }
        else
        {
            // Mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(aimInput);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public float stretchSpeed = 5f;
    private Vector3 originalScale;
    private bool isStretching = false;

    private InputMapping controls;

    void Awake()
    {
        controls = new InputMapping();
    }

    void OnEnable()
    {
        controls.Player.StretchArm.started += ctx => StartStretching();
        controls.Player.StretchArm.canceled += ctx => StopStretching();
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        originalScale = transform.parent.localScale;
    }

    void Update()
    {
        if (isStretching)
        {
            StretchArm();
        }
    }

    void StartStretching()
    {
        isStretching = true;
    }

    void StopStretching()
    {
        isStretching = false;
        ResetArm();
    }

    private void StretchArm()
    {
        transform.parent.localScale += new Vector3(stretchSpeed * Time.deltaTime, 0, 0);
    }

    public void ResetArm()
    {
        transform.parent.localScale = originalScale;
    }
}

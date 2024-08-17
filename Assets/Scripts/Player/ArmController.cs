using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public float stretchSpeed = 5f;
    public float moveSpeed = 10f;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isStretching = false;
    private bool isMoving = false;
    private Vector3 targetPosition;

    private InputMapping controls;
    private Transform playerTransform;

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
        originalPosition = transform.parent.localPosition;
        playerTransform = transform.parent.parent;
    }

    void Update()
    {
        if (isStretching)
        {
            StretchArm();
        }

        if (isMoving)
        {
            MovePlayerTowardsTarget();
        }
    }

    void StartStretching()
    {
        isStretching = true;
        originalPosition = transform.parent.localPosition;
    }

    void StopStretching()
    {
        isStretching = false;
    }

    private void StretchArm()
    {
        transform.parent.localScale += new Vector3(stretchSpeed * Time.deltaTime, 0, 0);
    }

    private void MovePlayerTowardsTarget()
    {
        playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        float distance = Vector3.Distance(playerTransform.position, targetPosition);
        float scaleFactor = distance / Vector3.Distance(originalPosition, targetPosition);

        transform.localScale = new Vector3(originalScale.x * scaleFactor, originalScale.y, originalScale.z);

        if (distance < 0.2f)
        {
            isMoving = false;
            ResetArm();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isStretching && ((1 << collision.gameObject.layer) & LayerMask.GetMask("Grabbable")) != 0)
        {
            StopStretching();
            targetPosition = transform.position;
            isMoving = true;
        }
    }


    public void ResetArm()
    {
        transform.parent.localScale = originalScale;
    }
}

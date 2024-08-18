using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    [SerializeField] LayerMask grabbableMask;
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
        else if (isMoving)
        {
            MovePlayerTowardsTarget();
        }
        else
        {
            ResetArm();
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

        transform.parent.localScale = new Vector3(originalScale.x * scaleFactor, originalScale.y, originalScale.z);

        if (distance < 0.2f)
        {
            isMoving = false;
            ResetArm();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isStretching && ((1 << collision.gameObject.layer) & grabbableMask) != 0)
        {
            Debug.Log("Collision...");
            RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, playerTransform.gameObject.GetComponent<PlayerAim>().aimDirection, 1000f, grabbableMask);
            Debug.Log("Direction: " + playerTransform.gameObject.GetComponent<PlayerAim>().aimDirection);
            Debug.Log("Target: " + hit.point);

            if (hit.collider != null)
            {
                Debug.Log("Grabbing...");
                StopStretching();
                targetPosition = hit.point;
                isMoving = true;
            }
        }
    }
    public void ResetArm()
    {
        transform.parent.localScale = originalScale;
    }
}

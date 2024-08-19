using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum ArmState
{
    Idle,
    Stretching,
    GapClosing,
    Waiting
}

public class ArmController : MonoBehaviour
{

    [SerializeField] LayerMask grabbableMask;
    [SerializeField] LayerMask notGrabbableMask;
    [SerializeField] GameObject hitCollider;
    [SerializeField] Animator armsAnimator;
    [SerializeField] Animator headAnimator;
    [HideInInspector] public ArmState currentState = ArmState.Idle;
    private InputMapping controls;
    private Transform playerTransform;
    private Transform handsTransform;
    [SerializeField] Transform headTransform;


    public float stretchSpeed = 5f;
    public float moveSpeed = 10f;
    private bool alreadyColliding = false;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private Vector3 initialLocalScale;
    private Vector3 gapClosePosition;
    private Vector3 gapCloseScale;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    

    void Awake()
    {
        controls = new InputMapping();
    }

    void OnEnable()
    {
        controls.Player.StretchArm.started += ctx => StartStretching();
        controls.Player.StretchArm.canceled += ctx => StopStretching();
        controls.Player.Launch.started += ctx => LaunchTowardsTarget();
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        handsTransform = transform.parent;
        playerTransform = transform.parent.parent;

        initialLocalPosition = handsTransform.localPosition;
        initialLocalRotation = handsTransform.localRotation;
        initialLocalScale = handsTransform.localScale;
        hitCollider.SetActive(false);
    }

    void Update()
    {
        switch(currentState)
        {
            case ArmState.Idle:
                ResetArm();
                break;
            case ArmState.Stretching:
                StretchArm();
                break; 

            case ArmState.GapClosing:
                MovePlayerTowardsTarget();
                break;
                
            case ArmState.Waiting:
                StickToWall();
                break;
        }
    }

    void StartStretching()
    {
        if (!alreadyColliding)
        {
            currentState = ArmState.Stretching;
            armsAnimator.SetBool("Attack", true);
            headAnimator.SetBool("Attack", true);
            StartCoroutine(StopAttackAnim());
        }
    }

    IEnumerator StopAttackAnim()
    {
        yield return new WaitForSeconds(0.1f);
        armsAnimator.SetBool("Attack", false);
        headAnimator.SetBool("Attack", false);
        yield break;
    }

    void StopStretching()
    {
        if (currentState != ArmState.GapClosing)
        {
            currentState = ArmState.Idle;
        }
    }
    void LaunchTowardsTarget()
    {
        if (currentState == ArmState.Waiting)
        {
            currentState = ArmState.GapClosing;
        }
    }

    private void StretchArm()
    {
        handsTransform.localScale += new Vector3(stretchSpeed * Time.deltaTime, 0, 0);
    }

    private void MovePlayerTowardsTarget()
    {
        playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPosition, moveSpeed * Time.deltaTime);
        hitCollider.SetActive(true);

        MaintainArms();
    }

    private void StickToWall()
    {
        Vector3 dir = (targetPosition - playerTransform.position).normalized;
        dir.z = 0f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        handsTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        headTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        MaintainArms();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState == ArmState.Stretching && ((1 << collision.gameObject.layer) & grabbableMask) != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, playerTransform.gameObject.GetComponent<PlayerAim>().aimDirection, 1000f, grabbableMask);

            if (hit.collider != null)
            {
                StopStretching();
                targetPosition = hit.point;
                gapClosePosition = handsTransform.position;
                gapCloseScale = handsTransform.localScale;
                originalPosition = playerTransform.localPosition;
                currentState = ArmState.Waiting;
            }
        }
        else if(((1 << collision.gameObject.layer) & notGrabbableMask) != 0)
        {
            StopStretching();
            currentState = ArmState.Idle;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & grabbableMask) != 0)
        {
            alreadyColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & grabbableMask) != 0)
        {
            alreadyColliding = false;
        }
    }
    public void ResetArm()
    {
        handsTransform.localPosition = initialLocalPosition;
        handsTransform.localRotation = initialLocalRotation;
        handsTransform.localScale = initialLocalScale;
        headTransform.localRotation = Quaternion.identity;
        hitCollider.SetActive(false);
    }

    private void MaintainArms()
    {
        float distance = Vector3.Distance(playerTransform.position, targetPosition);
        float scaleFactor = distance / Vector3.Distance(originalPosition, targetPosition);

        handsTransform.localScale = new Vector3(gapCloseScale.x * scaleFactor, gapCloseScale.y, gapCloseScale.z);

        if (distance < 0.1f)
        {
            currentState = ArmState.Idle;
            ResetArm();
        }
    }
}

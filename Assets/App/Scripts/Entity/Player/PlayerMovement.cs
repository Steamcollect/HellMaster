using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float groundAcceleration = 20f;
    [SerializeField] float airAcceleration = 10f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float friction = 8f;
    float moveSpeedMultiplier = 1;

    float distanceTravelled;
    private Vector3 lastPosition;

    [Header("Jump")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField] float jumpImpulsionMult = 1.2f;

    [Header("Physics")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;

    [Header("Camera FOV Settings")]
    [SerializeField] float defaultFOV = 60f;
    [SerializeField] float maxFOV = 75f;
    [SerializeField] float fovSpeed = 5f;

    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;

    [Header("Achievment")]
    [SerializeField] SSO_Achievment_RunDistance[] achievmentsRunDistance;

    [Space(10)]
    [SerializeField] RSO_PlayerTransform rsoPlayerTransform;
    [SerializeField] RSO_ContentSaved rsoContentSaved;

    Vector3 moveDirection;
    bool isGrounded;
    float lastGroundedTime;
    float jumpBufferCounter;

    bool canMove = false;

    [Header("Input")]
    [SerializeField] RSE_AddMoveSpeedMultiplier rseAddMoveSpeedMult;
    [SerializeField] RSE_OnGameStart rseOnGameStart;
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;

    private void OnEnable()
    {
        rsoPlayerTransform.Value = transform;
        rseAddMoveSpeedMult.action += AddMoveSpeedMultiplier;
        rseOnGameStart.action += OnGameStart;
        rseOnPlayerDeath.action += OnPlayerDeath;
    }
    private void OnDisable()
    {
        rseAddMoveSpeedMult.action -= AddMoveSpeedMultiplier;
        rseOnPlayerDeath.action -= OnPlayerDeath;
        rseOnGameStart.action -= OnGameStart;
    }

    void Update()
    {
        HandleInput();
        CheckGroundStatus();
        UpdateCameraFOV();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
            TrackDistanceTravelled();

            ApplyFriction();
            HandleJump();
        }        
    }

    void OnGameStart()
    {
        distanceTravelled = rsoContentSaved.Value.totalDistanceTravelled;
        canMove = true;
    }
    void OnPlayerDeath()
    {
        rsoContentSaved.Value.totalDistanceTravelled = distanceTravelled;
        canMove = false;
    }

    void AddMoveSpeedMultiplier(float multGiven)
    {
        moveSpeedMultiplier += multGiven;
    }

    void HandleInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            lastGroundedTime = Time.time;
        }
    }

    void MovePlayer()
    {
        float acceleration = isGrounded ? groundAcceleration : airAcceleration;
        Vector3 targetVelocity = moveDirection * maxSpeed * moveSpeedMultiplier;
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = Vector3.ClampMagnitude(targetVelocity - new Vector3(velocity.x, 0, velocity.z), acceleration * moveSpeedMultiplier * Time.fixedDeltaTime);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void TrackDistanceTravelled()
    {
        float distance = Vector3.Distance(lastPosition, transform.position);

        distanceTravelled += distance;
        lastPosition = transform.position;

        foreach (var item in achievmentsRunDistance)
        {
            item.CheckDistance(distanceTravelled);
        }
    }

    void ApplyFriction()
    {
        if (isGrounded && moveDirection.magnitude < 0.1f)
        {
            Vector3 velocity = rb.velocity;
            Vector3 frictionForce = -friction * new Vector3(velocity.x, 0, velocity.z);
            rb.AddForce(frictionForce, ForceMode.Acceleration);
        }
    }

    void HandleJump()
    {
        if (jumpBufferCounter > 0 && (isGrounded || Time.time - lastGroundedTime <= coyoteTime))
        {
            rb.velocity = new Vector3(rb.velocity.x * jumpImpulsionMult, jumpForce, rb.velocity.z * jumpImpulsionMult);
            jumpBufferCounter = 0;
        }
    }

    void UpdateCameraFOV()
    {
        float currentSpeed = rb.velocity.magnitude;

        // Interpolation du FOV entre le FOV par défaut et le FOV maximum
        float targetFOV = Mathf.Lerp(defaultFOV, maxFOV, currentSpeed / maxSpeed);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * fovSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
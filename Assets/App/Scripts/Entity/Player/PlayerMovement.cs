using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float deceleration = 15f;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float gravity = 20f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float jumpBufferTime = 0.1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;

    [Header("References")]
    [SerializeField] Rigidbody rb;
    Vector3 movementInput;
    bool isGrounded;
    float coyoteTimeCounter;
    float jumpBufferCounter = 0f;
    float verticalVelocity;

    void Update()
    {
        HandleMovementInput();
        HandleJump();
    }
    void FixedUpdate()
    {
        ApplyMovement();
        ApplyGravity();
    }

    void HandleMovementInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 targetDirection = transform.right * moveX + transform.forward * moveZ;

        if (targetDirection.magnitude > 0.1f)
        {
            movementInput = Vector3.Lerp(movementInput, targetDirection * moveSpeed, Time.fixedDeltaTime * acceleration);
        }
        else
        {
            movementInput = Vector3.Lerp(movementInput, Vector3.zero, Time.fixedDeltaTime * deceleration);
        }
    }
    void ApplyMovement()
    {
        Vector3 horizontalVelocity = new Vector3(movementInput.x, 0f, movementInput.z);
        rb.velocity = horizontalVelocity + new Vector3(0f, verticalVelocity, 0f);
    }

    void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpBufferCounter = 0f;

            if (verticalVelocity < 0f)
            {
                verticalVelocity = 0f;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            jumpBufferCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && verticalVelocity <= 0f)
        {
            verticalVelocity = jumpForce;
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.fixedDeltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
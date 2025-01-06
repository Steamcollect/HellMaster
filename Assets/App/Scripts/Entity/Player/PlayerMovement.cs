using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [Header("References")]
    [SerializeField] Rigidbody rb;

    bool jumpInput;

    public void Move(Vector3 moveInput)
    {
        print(transform.forward + moveInput);
        rb.AddForce(transform.forward + (moveInput * moveSpeed));
    }
    public void jump(bool jumpInput)
    {
        if(!this.jumpInput && jumpInput)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }

        this.jumpInput = jumpInput;
    }
}

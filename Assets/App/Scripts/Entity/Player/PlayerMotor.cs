using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerMovement movement;

    Vector3 moveInput;
    bool jumpInput;

    private void Update()
    {
        SetInputs();
    }
    private void FixedUpdate()
    {
        movement.Move(moveInput);
        //movement.jump(jumpInput);
    }

    void SetInputs()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");

        jumpInput = Input.GetKey(KeyCode.Space);
    }
}

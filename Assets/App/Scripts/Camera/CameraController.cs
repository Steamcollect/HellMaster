using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] Transform playerBody;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float maxTiltAngle = 10f;
    [SerializeField] float tiltSpeed = 5f;

    float xRotation = 0f;
    float currentTilt = 0f;

    bool canMove = false;

    [Space(10)]
    [SerializeField] RSE_OnGameStart rseOnGameStart;
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;

    private void OnEnable()
    {
        rseOnGameStart.action += OnGameStart;
        rseOnPlayerDeath.action += OnPlayerDeath;
    }
    private void OnDisable()
    {
        rseOnGameStart.action -= OnGameStart;
        rseOnPlayerDeath.action -= OnPlayerDeath;
    }

    void OnGameStart()
    {
        canMove = true;   
    }
    void OnPlayerDeath()
    {
        canMove = false;
    }

    private void LateUpdate()
    {
        if (canMove)
        {
            RotateCamera();
            TiltCameraBasedOnMovement();
        }    
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void TiltCameraBasedOnMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float targetTilt = -horizontalInput * maxTiltAngle;

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentTilt);
    }
}

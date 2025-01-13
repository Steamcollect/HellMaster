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
    [SerializeField] float shakeSpeed;

    [Space(10)]
    [SerializeField] RSO_MouseSensitivityMultiplier rsoMouseSensitivityMult;

    float xRotation = 0f;
    float currentTilt = 0f;
    float shakeOffset = 0f; // Variable to hold the shake offset

    bool canMove = false;

    [Space(10)]
    [SerializeField] RSE_OnGameStart rseOnGameStart;
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;
    [SerializeField] RSE_CameraShake rseCameraShake;
    [SerializeField] RSO_ContentSaved rsoContentSave;

    private void OnEnable()
    {
        rseOnGameStart.action += OnGameStart;
        rseOnPlayerDeath.action += OnPlayerDeath;
        rseCameraShake.action += Shake;
    }
    private void OnDisable()
    {
        rseOnGameStart.action -= OnGameStart;
        rseOnPlayerDeath.action -= OnPlayerDeath;
        rseCameraShake.action -= Shake;
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

        // Apply shake offset to the rotation
        ApplyShake();
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * (mouseSensitivity * rsoMouseSensitivityMult.Value) * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * (mouseSensitivity * rsoMouseSensitivityMult.Value) * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void Shake(float time, float range)
    {
        StartCoroutine(CameraShake(time, range));
    }

    IEnumerator CameraShake(float time, float range)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            shakeOffset = Mathf.Sin(elapsedTime * shakeSpeed) * range;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        shakeOffset = 0f; // Reset the shake offset after shaking
    }

    void ApplyShake()
    {
        transform.localRotation = Quaternion.Euler(xRotation, 0f, shakeOffset);
    }

    void TiltCameraBasedOnMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float targetTilt = -horizontalInput * maxTiltAngle;

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentTilt);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float maxTiltAngle = 10f;
    [SerializeField] float tiltSpeed = 5f;
    [SerializeField] float shakeSpeed;

    [Space(10)]
    [SerializeField] RSO_MouseSensitivityMultiplier rsoMouseSensitivityMult;

    float xRotation = 0f;
    float yRotation = 0f;
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
        CombineRotations();
    }

    void RotateCamera()
    {
        if (rsoMouseSensitivityMult.Value < .1f)
        {
            rsoMouseSensitivityMult.Value = 1;
        }

        float mouseX = Input.GetAxis("Mouse X") * (mouseSensitivity * rsoMouseSensitivityMult.Value) * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * (mouseSensitivity * rsoMouseSensitivityMult.Value) * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void Shake(float time, float range)
    {
        StartCoroutine(CameraShake(time, range));
    }

    IEnumerator CameraShake(float time, float range)
    {
        if (range > Mathf.Abs(shakeOffset))
        {
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                shakeOffset = Mathf.Sin(elapsedTime * shakeSpeed) * range;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            shakeOffset = 0f;
        }
    }

    void TiltCameraBasedOnMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float targetTilt = -horizontalInput * maxTiltAngle;

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
    }

    void CombineRotations()
    {
        // Combine the rotation components into a single Quaternion
        Quaternion rotationXY = Quaternion.Euler(xRotation, yRotation, 0f);
        Quaternion tilt = Quaternion.Euler(0f, 0f, currentTilt);
        Quaternion shake = Quaternion.Euler(0f, 0f, shakeOffset);

        transform.localRotation = rotationXY * tilt * shake;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraControl : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 2f; // Speed for rotating around the player
    [SerializeField] private float verticalSpeed = 2f; // Speed for moving up and down
    [SerializeField] private float distanceFromPlayer = 5f; // Default distance from the player

    private float currentAngle = 0f; // Current rotation angle around the player
    private float currentVerticalAngle = 0f; // Current vertical angle up/down

    private void Update()
    {
        HandleCameraRotation();
        HandleCameraVerticalMovement();
        UpdateCameraPosition();
    }

    private void HandleCameraRotation()
    {
        // Get mouse input for horizontal rotation
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        currentAngle += mouseX;

        // Calculate the new position based on the horizontal rotation
        float radians = Mathf.Deg2Rad * currentAngle;
        float x = Mathf.Cos(radians) * distanceFromPlayer;
        float z = Mathf.Sin(radians) * distanceFromPlayer;

        Vector3 newPosition = new Vector3(x, virtualCamera.transform.position.y, z);
        virtualCamera.transform.position = player.position + newPosition;
    }

    private void HandleCameraVerticalMovement()
    {
        // Get mouse input for vertical rotation (looking up and down)
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;
        currentVerticalAngle -= mouseY; // Invert to match expected camera movement

        // Clamp the vertical angle to prevent flipping the camera
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, -80f, 80f);

        // Update the camera's rotation to look up and down
        Quaternion verticalRotation = Quaternion.Euler(currentVerticalAngle, currentAngle, 0);
        virtualCamera.transform.rotation = verticalRotation;
    }

    private void UpdateCameraPosition()
    {
        // Ensure the camera is looking at the player
        virtualCamera.transform.LookAt(player);
    }
}

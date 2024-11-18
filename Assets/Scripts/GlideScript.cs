using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class GlideScript : MonoBehaviour
{
    private CharacterController _characterController;
    private InputAction _glideAction;

    [SerializeField] private float baseGlideSpeed = 5f; // Base glide speed after boost
    [SerializeField] private float maxGlideSpeed = 15f; // Maximum glide speed when boosting
    [SerializeField] private float glideSpeedIncreaseRate = 5f; // Speed increase rate while boosting
    [SerializeField] private float boostDuration = 3f; // Duration of the boost in seconds
    [SerializeField] private float gravity = -9.81f; // Gravity strength
    [SerializeField] private float buttonCooldownTime = 1f; // Cooldown time in seconds before glide can be triggered again
    [SerializeField] private float glideCooldownSpeed = 2f; // Rate of glide speed decay during cooldown

    private float currentGlideSpeed = 0f; // Current glide speed
    private float boostTimeRemaining = 0f;
    private float buttonCooldownRemaining = 0f; // Time remaining for button cooldown
    private bool isBoosting = false;

    private Vector3 velocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        var playerInput = GetComponent<PlayerInput>();

        _glideAction = playerInput.actions["Glide"];
        _glideAction.started += StartBoost;
        _glideAction.canceled += StopBoost;
    }

    private void Update()
    {
        // Handle button cooldown
        if (buttonCooldownRemaining > 0)
        {
            buttonCooldownRemaining -= Time.deltaTime;
        }

        // Check if we are in the air to glide
        if (!_characterController.isGrounded)
        {
            GlideMovement();
        }
        else
        {
            // Reset when grounded
            velocity = Vector3.zero;
        }

        // Update boost behavior
        if (isBoosting)
        {
            HandleBoost();
        }
        else
        {
            HandleCooldown(); // Gradual speed decay during cooldown
        }
    }

    private void GlideMovement()
    {
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Glide forward based on current glide speed
        Vector3 forwardDirection = transform.forward;
        _characterController.Move(forwardDirection * currentGlideSpeed * Time.deltaTime);

        // Apply the velocity to the character controller
        _characterController.Move(velocity * Time.deltaTime);
    }

    private void StartBoost(InputAction.CallbackContext context)
    {
        if (buttonCooldownRemaining <= 0 && !_characterController.isGrounded)
        {
            isBoosting = true;
            boostTimeRemaining = boostDuration;
            buttonCooldownRemaining = buttonCooldownTime; // Reset button cooldown
        }
    }

    private void StopBoost(InputAction.CallbackContext context)
    {
        isBoosting = false;
    }

    private void HandleBoost()
    {
        boostTimeRemaining -= Time.deltaTime;

        if (boostTimeRemaining > 0)
        {
            // Increase glide speed while boosting, up to max glide speed
            currentGlideSpeed = Mathf.Min(currentGlideSpeed + glideSpeedIncreaseRate * Time.deltaTime, maxGlideSpeed);
        }
        else
        {
            isBoosting = false;
        }
    }

    private void HandleCooldown()
    {
        if (buttonCooldownRemaining > 0)
        {
            // Gradually reduce glide speed back to base speed
            currentGlideSpeed = Mathf.Lerp(currentGlideSpeed, baseGlideSpeed, glideCooldownSpeed * Time.deltaTime);
        }
        else
        {
            currentGlideSpeed = baseGlideSpeed; // Ensure it's at base speed when cooldown is over
        }
    }

    private void OnDisable()
    {
        _glideAction.started -= StartBoost;
        _glideAction.canceled -= StopBoost;
    }
}

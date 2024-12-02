using Cinemachine;
using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float respawnTime = 5.0f; // Time in seconds for the power-up to respawn
    private Vector3 initialPosition;                  // Store the initial position for respawn
    private Quaternion initialRotation;               // Store the initial rotation for respawn
    private Animator animator;                        // Reference to the Animator component
    private bool isRespawning = false;                // Prevent multiple respawn triggers
    private SpriteRenderer spriteRenderer;            // Reference to SpriteRenderer for hiding
    public CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera == null)
        {
            Debug.LogError("camera not found");
        }
    }

    private void Awake()
    {
        // Save the initial position and rotation of the power-up
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Cache the Animator and SpriteRenderer components
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (virtualCamera != null)
        {
            Transform cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
            transform.forward = new Vector3(cameraTransform.forward.x, cameraTransform.forward.y, cameraTransform.forward.z);
        }
    }

    public void ActivatePowerUp(CharacterController playerController)
    {
        if (isRespawning) return; // Prevent activation if already respawning

        // Reference to PlayerScript to apply power-up effects
        var playerScript = playerController.GetComponent<PlayerScript1>();
        if (playerScript != null)
        {
            playerScript.AddExtraJump(1); // Adds one extra jump
            Debug.Log("Power-up activated: Extra jump activated!");
        }

        // Start the respawn process
        StartCoroutine(RespawnPowerUp());
    }

    private IEnumerator RespawnPowerUp()
    {
        isRespawning = true;

        // Hide the power-up
        HidePowerUp();

        // Wait for the respawn time
        yield return new WaitForSeconds(respawnTime);

        // Reset position and rotation before reactivating
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Show the power-up
        ShowPowerUp();

        isRespawning = false;
    }

    private void HidePowerUp()
    {
        // Disable rendering and collider
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        GetComponent<Collider>().enabled = false;

        // Disable the Animator if present
        if (animator != null) animator.enabled = false;
    }

    private void ShowPowerUp()
    {
        // Enable rendering and collider
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        GetComponent<Collider>().enabled = true;

        // Enable the Animator if present
        if (animator != null)
        {
            animator.enabled = true;
            animator.Rebind(); // Reset the Animator to its default state
            animator.Update(0); // Force an immediate state update
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a player
        CharacterController playerController = other.GetComponent<CharacterController>();
        if (playerController != null)
        {
            // Activate the power-up if a player collides
            ActivatePowerUp(playerController);
        }
    }
}

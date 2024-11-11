using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private CharacterController _characterController; // Remember Reference to the CharacterController
    private Vector3 _respawnPoint; // Store the respawn position

    void Start()
    {
        // Initialize the CharacterController component
        _characterController = GetComponent<CharacterController>();
    }

    // This method will be called to update the last activated checkpoint position
    public void UpdateCheckpoint(Vector3 newCheckpoint)
    {
        _respawnPoint = newCheckpoint; // Set the new respawn position to the checkpoint's position
        Debug.Log("Checkpoint updated to: " + newCheckpoint);
    }

    // This method handles the respawning of the player
    public void Respawn()
    {
        if (_respawnPoint != Vector3.zero) // Check if a valid respawn point is set
        {
            // Turn off character controller temporarily to avoid interference during teleportation
            _characterController.enabled = false;

            // Teleport the player to the last checkpoint
            transform.position = _respawnPoint;

            // Re-enable the character controller after teleporting
            _characterController.enabled = true;

            Debug.Log("Player respawned to: " + _respawnPoint);
        }
        else
        {
            Debug.LogError("No checkpoint set. Can't respawn.");
        }
    }

    // This method handles when the player falls into the fall zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with the fall zone
        if (other.CompareTag("FallZone"))
        {
            // Trigger the respawn process when falling off
            Respawn();
        }
    }
}


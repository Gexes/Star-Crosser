using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Variable to keep track of whether the checkpoint has been activated
    private bool _isActivated = false;

    // Color change to indicate activation
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isActivated)
        {
            // Change the color to indicate activation
            _renderer.material.color = Color.green;

            // Mark the checkpoint as activated
            _isActivated = true;

            // Update the last activated checkpoint in the PlayerRespawn script
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.UpdateCheckpoint(transform.position);
            }

            // Show checkpoint popup on the UI
            UIController uiController = FindObjectOfType<UIController>();
            if (uiController != null)
            {
                uiController.ShowCheckpointPopup("Checkpoint Activated!");
            }
        }
    }
}

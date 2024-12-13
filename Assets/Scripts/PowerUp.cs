using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Oxygen Gas Amount")]
    [SerializeField] private float oxygenAmount = 1f; // The amount of OxygenGas to add

    [Header("Respawn Settings")]
    [SerializeField] private float respawnTime = 5f; // Time before the object becomes visible again

    private Renderer objectRenderer;
    private Collider objectCollider;

    private void Start()
    {
        // Cache Renderer and Collider components
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding has the Player tag
        if (other.CompareTag("Player"))
        {
            // Try to get the WalkMovement component from the player
            WalkMovement walkMovement = other.GetComponent<WalkMovement>();

            if (walkMovement != null)
            {
                // Increase the player's OxygenGas
                walkMovement.OxygenGas += oxygenAmount;

                // Ensure it doesn't exceed any maximum limit
                walkMovement.OxygenGas = Mathf.Clamp(walkMovement.OxygenGas, 0, 100f); // Adjust max value as needed

                // Activate RespawnMode
                RespawnMode();
            }
        }
    }

    private void RespawnMode()
    {
        // Start the respawn timer
        StartCoroutine(RespawnTimer());
    }

    private IEnumerator RespawnTimer()
    {
        // Hide the object
        objectRenderer.enabled = false;
        objectCollider.enabled = false;

        // Wait for the respawn time
        yield return new WaitForSeconds(respawnTime);

        // Make the object visible and interactive again
        objectRenderer.enabled = true;
        objectCollider.enabled = true;
    }
}

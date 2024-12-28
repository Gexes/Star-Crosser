using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour
{
    [SerializeField] private RespawnManager respawnManager; // referencing the respawn manager
    [SerializeField] private float fallDamage = 10f; // Damage dealt to the player

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Call the Respawn method from the RespawnManager
            respawnManager.Respawn();
        }

        WalkMovement player = other.GetComponent<WalkMovement>();
        if (player != null)
        {
            FallZoneDamage(player);
        }
    }

    public void FallZoneDamage(WalkMovement player)
    {
        // Apply fall damage to the player's health
        player.currentHealth -= fallDamage;
        player.currentHealth = Mathf.Max(player.currentHealth, 0); // Prevent health from dropping below zero
        Debug.Log($"Player took {fallDamage} damage from FallZone. Current health: {player.currentHealth}");
    }
}


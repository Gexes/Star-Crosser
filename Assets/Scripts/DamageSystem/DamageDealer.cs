using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Damage Configuration")]
    [SerializeField] private DamageData damageData; // Reference to the DamageData ScriptableObject

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit has a health system (like WalkMovement)
        WalkMovement targetHealth = collision.gameObject.GetComponent<WalkMovement>();

        if (targetHealth != null)
        {
            // Apply damage to the target's health
            targetHealth.TakeDamage(damageData.damageAmount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Alternative for trigger-based collision
        WalkMovement targetHealth = other.gameObject.GetComponent<WalkMovement>();

        if (targetHealth != null)
        {
            // Apply damage to the target's health
            targetHealth.TakeDamage(damageData.damageAmount);
        }
    }
}

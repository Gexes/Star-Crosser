using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Damage Configuration")]
    [SerializeField] private DamageData damageData; // Reference to the DamageData ScriptableObject

    private void OnTriggerEnter(Collider other)
    {
        // Alternative for trigger-based collision
        WalkMovement targetHealth = other.gameObject.GetComponent<WalkMovement>();

        if (targetHealth != null)
        {
            // Apply damage to the target's health
            ApplyDamage(targetHealth);
        }


    }

    private void ApplyDamage(WalkMovement targetHealth)
    {
        // Apply base damage
        targetHealth.TakeDamage(damageData.damageAmount);

        // If the damage type is "Fire", start continuous damage
        if (damageData.damageType.Equals("Fire", System.StringComparison.OrdinalIgnoreCase))
        {
            ApplyFireDamage(targetHealth);
        }
    }

    private void ApplyFireDamage(WalkMovement targetHealth)
    {
        // Apply initial fire damage
        targetHealth.TakeFireDamage(damageData.damageAmount);

        // Start continuous fire damage over time
        StartCoroutine(FireDamageOverTime(targetHealth, damageData.fireTickDamage, damageData.fireDuration, damageData.fireInterval));
    }

    private IEnumerator FireDamageOverTime(WalkMovement targetHealth, float tickDamage, float duration, float interval)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            targetHealth.TakeFireDamage(tickDamage); // Apply fire tick damage
            yield return new WaitForSeconds(interval);
            elapsedTime += interval;
        }
    }
}

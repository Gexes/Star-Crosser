using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public void ActivatePowerUp(CharacterController playerController)
    {
        // Reference to PlayerScript to apply power-up effects
        var playerScript = playerController.GetComponent<PlayerScript>();
        if (playerScript != null)
        {
            playerScript.AddExtraJump(1); // Adds one extra jump
            playerScript.AddExtraGlide(1); // Adds one extra glide
            Debug.Log("Power-up activated: Extra jump and glide granted!");
        }

        // Optional: Destroy or deactivate the power-up after activation
        Destroy(gameObject);
    }

}




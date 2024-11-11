using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public UIController uiController; // Reference to the UIController

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Collectible"))
        {
            // Notify the UIController to increment the collectible count
            if (uiController != null)
            {
                uiController.AddCollectible();
            }

            // Destroy this collectible object
            Destroy(gameObject);
        }
    }
}


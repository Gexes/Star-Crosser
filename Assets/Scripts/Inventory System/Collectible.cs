using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    public CollectibleData collectibleData; // Reference to the ScriptableObject

    [Header("UI Reference")]
    public UIController uiController; // Reference to the UIController

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && collectibleData != null)
        {
            // Add the collectible to the inventory UI
            if (uiController != null)
            {
                uiController.AddCollectibleToInventory(collectibleData);
            }

            Debug.Log($"Collected: {collectibleData.collectibleName}");

            // Destroy this collectible object
            Destroy(gameObject);
        }
    }
}

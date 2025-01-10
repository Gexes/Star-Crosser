using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    public CollectibleData collectibleData; // Reference to the ScriptableObject

    [Header("UI Reference")]
    public UIController uiController; // Reference to the UIController

    [Header("Collectible Settings")]
    public AudioClip collectSound; // Sound to play upon collection
    private AudioSource audioSource; // AudioSource for playing sound

    private void Awake()
    {
        // Ensure the GameObject has an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Prevent sound from playing on start
    }

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

            // Play the sound if it exists
            if (collectSound != null)
            {
                audioSource.clip = collectSound;
                audioSource.Play();
            }

            // Destroy this collectible object
            Destroy(gameObject, audioSource.clip != null ? audioSource.clip.length : 0f);
        }
    }
}

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [Header("Checkpoint References")]
    public TextMeshProUGUI checkpointPopupText; // Text for checkpoint popup
    public float popupDuration = 2f;            // Duration for the popup message
    private Coroutine popupCoroutine;
    
    [Header("Collectibles References")]
    private int collectibleCount = 0;
    public TextMeshProUGUI collectibleText; // Reference to the UI Text element

    [Header("Inventory Panel")]
    public Transform inventoryPanel; // Parent container for inventory items
    public GameObject inventoryItemPrefab; // Prefab for inventory



    void Start()
    {
        // Ensure the checkpoint popup is hidden on start
        checkpointPopupText.gameObject.SetActive(false);
    }

    // Increase the collectible count
    public void AddCollectible(int value)
    {
        collectibleCount += value;
        UpdateCollectibleText();
    }

    // Update the UI text with the current count
    private void UpdateCollectibleText()
    {
        if (collectibleText != null)
        {
            collectibleText.text = $"{collectibleCount}";
        }
    }

    // Display a popup message for a checkpoint
    public void ShowCheckpointPopup(string message)
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
        }
        popupCoroutine = StartCoroutine(ShowPopupCoroutine(message));
    }

    // Coroutine to handle popup visibility
    private IEnumerator ShowPopupCoroutine(string message)
    {
        checkpointPopupText.text = message;
        checkpointPopupText.gameObject.SetActive(true); // Show the popup

        yield return new WaitForSeconds(popupDuration);

        checkpointPopupText.gameObject.SetActive(false); // Hide the popup
    }

    // Note to self, add popup collectible and description of item here //
    public void AddCollectibleToInventory(CollectibleData collectibleData)
    {
        if (inventoryPanel == null || inventoryItemPrefab == null)
        {
            Debug.LogError("Inventory Panel or Item Prefab is not assigned.");
            return;
        }

        // Instantiate a new inventory item
        GameObject newItem = Instantiate(inventoryItemPrefab, inventoryPanel);

        // Set the icon and name for the new item
        Image itemIcon = newItem.transform.Find("Icon").GetComponent<Image>();
        if (itemIcon != null)
        {
            itemIcon.sprite = collectibleData.collectibleIcon;
        }

        TextMeshProUGUI itemName = newItem.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        if (itemName != null)
        {
            itemName.text = collectibleData.collectibleName;
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI collectibleCountText; // Use TextMeshProUGUI
    public TextMeshProUGUI checkpointPopupText; // Text for checkpoint popup
    public float popupDuration = 2f;            // Duration for the popup message

    private int collectibleCount = 0;
    private Coroutine popupCoroutine;

    // Increase the collectible count
    public void AddCollectible()
    {
        collectibleCount++;
        UpdateCollectibleText();
    }

    // Update the UI text with the current count
    private void UpdateCollectibleText()
    {
        collectibleCountText.text = collectibleCount + "/5";
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
}

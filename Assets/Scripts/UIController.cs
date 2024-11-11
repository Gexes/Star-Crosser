using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI collectibleCountText; // Use TextMeshProUGUI
    private int collectibleCount = 0;

    // increase the collectible count
    public void AddCollectible()
    {
        collectibleCount++;
        UpdateCollectibleText();
    }

    // update the UI text with the current count
    private void UpdateCollectibleText()
    {
        collectibleCountText.text = collectibleCount + "/3";
    }
}

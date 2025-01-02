using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] private RectTransform InventoryButton;

    // Keeping track whether the inventory is open or closed
    private bool isOpen = false;

    public void InventoryButtonClick()
    {
        // Null check: for clarification if the inventory is assigned or not
        if (InventoryButton == null)
        {
            Debug.LogError("InventoryButton not assigned in the inspector.");
            return;
        }

        if (isOpen)
        {
            // Close the inventory (move to -1190)
            InventoryButton.DOAnchorPosX(-1190, 0.5f)
                           .SetEase(Ease.InOutQuad);
        }
        else
        {
            // Open the inventory (move to -868)
            InventoryButton.DOAnchorPosX(-868f, 0.5f)
                           .SetEase(Ease.InOutQuad);
        }

        // Toggle the state
        isOpen = !isOpen;
    }
}

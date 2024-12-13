using UnityEngine;
using UnityEngine.UI;

public class UIGlider : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider OxygenBar; // Reference to the slider in the UI

    [Header("Player Script Reference")]
    [SerializeField] private WalkMovement player; // Reference to the WalkMovement script

    private void Start()
    {
        // Ensure references are set
        if (OxygenBar == null)
        {
            Debug.LogError("OxygenBar slider is not assigned!");
        }

        if (player == null)
        {
            Debug.LogError("Player reference (WalkMovement) is not assigned!");
        }
    }

    private void Update()
    {
        if (player != null && OxygenBar != null)
        {
            // Update the slider value based on the player's OxygenGas
            OxygenBar.value = player.OxygenGas;
        }
    }
}

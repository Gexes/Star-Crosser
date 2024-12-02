using System.Collections;
using UnityEngine;
using TMPro;

public class TypeWriterScript : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.05f; // Delay between each character
    [SerializeField] private RectTransform panelRect; // The RectTransform of the panel to act as a boundary

    private Coroutine currentTypingCoroutine; // Tracks the current coroutine

    /// <summary>
    /// Starts the typewriter effect for the given TextMeshProUGUI component.
    /// </summary>
    /// <param name="textComponent">The TextMeshProUGUI component to type text into.</param>
    /// <param name="fullText">The full text to display.</param>
    public void StartTypewriterEffect(TextMeshProUGUI textComponent, string fullText)
    {
        if (textComponent != null)
        {
            // Stop any ongoing typewriter effect
            StopTypewriterEffect();

            // Ensure the text stays within bounds by configuring overflow modes
            textComponent.overflowMode = TextOverflowModes.Masking; // Masks overflow text

            // Start the typewriter coroutine
            currentTypingCoroutine = StartCoroutine(TypeTextCoroutine(textComponent, fullText));
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is null. Please assign a valid component.");
        }
    }

    /// <summary>
    /// Coroutine that handles the typewriter effect.
    /// </summary>
    /// <param name="textComponent">The TextMeshProUGUI component to type text into.</param>
    /// <param name="fullText">The full text to display.</param>
    private IEnumerator TypeTextCoroutine(TextMeshProUGUI textComponent, string fullText)
    {
        textComponent.text = ""; // Clear any existing text

        foreach (char letter in fullText)
        {
            textComponent.text += letter; // Append one letter at a time

            // Check if text exceeds the panel's boundaries
            if (!IsTextWithinBounds(textComponent))
            {
                Debug.LogWarning("Text is exceeding panel bounds!");
                break; // Stop typing if text goes out of bounds
            }

            yield return new WaitForSeconds(typingSpeed); // Wait before adding the next character
        }

        currentTypingCoroutine = null; // Reset coroutine reference when done
    }

    /// <summary>
    /// Checks if the text fits within the panel's bounds.
    /// </summary>
    /// <param name="textComponent">The TextMeshProUGUI component to check.</param>
    /// <returns>True if the text fits within the bounds, false otherwise.</returns>
    private bool IsTextWithinBounds(TextMeshProUGUI textComponent)
    {
        if (panelRect == null)
        {
            Debug.LogError("Panel RectTransform is not assigned.");
            return true; // Assume within bounds if no panel is assigned
        }

        RectTransform textRect = textComponent.GetComponent<RectTransform>();
        if (textRect == null)
        {
            Debug.LogError("No RectTransform found on the TextMeshProUGUI component.");
            return true; // Assume within bounds if no RectTransform is found
        }

        // Check if the text's bounds are within the panel's bounds
        return panelRect.rect.Contains(panelRect.InverseTransformPoint(textRect.position));
    }

    /// <summary>
    /// Stops any ongoing typewriter effect for the given TextMeshProUGUI component.
    /// </summary>
    public void StopTypewriterEffect()
    {
        if (currentTypingCoroutine != null)
        {
            StopCoroutine(currentTypingCoroutine);
            currentTypingCoroutine = null;
        }
    }
}

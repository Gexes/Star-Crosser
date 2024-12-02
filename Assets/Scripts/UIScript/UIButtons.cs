using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening; // Using DoTween for animations

public class UIButtons : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;

    // References for slide-out panel
    [SerializeField]
    private RectTransform slideOutPanel;

    [SerializeField]
    private TextMeshProUGUI displayText;

    [SerializeField]
    private string textToShow;

    // CanvasGroup for other UI elements
    [SerializeField]
    private CanvasGroup otherUIElements;

    // Background overlay to handle clicking outside the panel
    [SerializeField]
    private CanvasGroup backgroundOverlay;

    private bool isPanelVisible = false; // Tracks panel visibility
    private Vector2 panelHiddenPosition; // Store the panel's off-screen position
    private Vector2 panelVisiblePosition; // Store the panel's visible position
    private float slideDuration = 0.5f; // Animation duration for the slide

    // Skybox-related variables
    [SerializeField]
    private Material[] skyboxes; // Array of skybox materials to cycle through
    private int currentSkyboxIndex = 0; // Tracks the currently active skybox

    private void Start()
    {
        if (slideOutPanel != null)
        {
            // Initialize the panel's hidden and visible positions
            panelHiddenPosition = new Vector2(-slideOutPanel.rect.width, slideOutPanel.anchoredPosition.y);
            panelVisiblePosition = slideOutPanel.anchoredPosition;

            // Set the panel to its hidden position at the start
            slideOutPanel.anchoredPosition = panelHiddenPosition;
        }

        if (backgroundOverlay != null)
        {
            // Hide the background overlay initially
            backgroundOverlay.alpha = 0;
            backgroundOverlay.interactable = false;
            backgroundOverlay.blocksRaycasts = false;
        }

        // Set the initial skybox
        if (skyboxes != null && skyboxes.Length > 0)
        {
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
        }
    }

    /// <summary>
    /// Loads a new scene when called.
    /// </summary>
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name is empty, add a scene in the inspector.");
        }
    }

    /// <summary>
    /// Toggles the slide-out panel, updates the text if necessary, and manages other UI elements.
    /// </summary>
    public void TogglePanel()
    {
        if (slideOutPanel == null || displayText == null || otherUIElements == null || backgroundOverlay == null)
        {
            Debug.LogError("Slide-out panel, TextMeshPro, other UI reference, or background overlay is missing. Please assign them in the Inspector.");
            return;
        }

        isPanelVisible = !isPanelVisible;

        if (isPanelVisible)
        {
            // Update text only if `textToShow` is set and differs from the current text
            if (!string.IsNullOrEmpty(textToShow) && displayText.text != textToShow)
            {
                displayText.text = textToShow;
            }

            // Slide in the panel
            slideOutPanel.DOAnchorPos(panelVisiblePosition, slideDuration).SetEase(Ease.OutCubic);

            // Hide other UI elements
            otherUIElements.alpha = 0;
            otherUIElements.interactable = false;
            otherUIElements.blocksRaycasts = false;

            // Show the background overlay
            backgroundOverlay.alpha = 1;
            backgroundOverlay.interactable = true;
            backgroundOverlay.blocksRaycasts = true;
        }
        else
        {
            // Slide out the panel
            slideOutPanel.DOAnchorPos(panelHiddenPosition, slideDuration).SetEase(Ease.InCubic);

            // Restore other UI elements after animation
            slideOutPanel.DOAnchorPos(panelHiddenPosition, slideDuration).OnComplete(() =>
            {
                otherUIElements.alpha = 1;
                otherUIElements.interactable = true;
                otherUIElements.blocksRaycasts = true;
            });

            // Hide the background overlay
            backgroundOverlay.alpha = 0;
            backgroundOverlay.interactable = false;
            backgroundOverlay.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// Hides the slide-out panel when clicking outside.
    /// </summary>
    public void HidePanelOnClickOutside()
    {
        if (isPanelVisible)
        {
            TogglePanel();
        }
    }

    /// <summary>
    /// Changes the skybox to the next one in the array.
    /// </summary>
    public void ChangeSkybox()
    {
        if (skyboxes != null && skyboxes.Length > 0)
        {
            currentSkyboxIndex = (currentSkyboxIndex + 1) % skyboxes.Length; // Cycle through the array
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
            


            // Optionally, trigger a skybox update if necessary (e.g., for reflections)
            DynamicGI.UpdateEnvironment();
        }
        else
        {
            Debug.LogWarning("No skyboxes assigned. Please add skyboxes to the inspector.");
        }
    }
}

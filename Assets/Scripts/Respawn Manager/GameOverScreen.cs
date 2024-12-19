using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [Header("Player Health Reference")]
    [SerializeField] private WalkMovement player; // Reference to the WalkMovement script

    [Header("Animator Reference")]
    [SerializeField] private Animator panelAnimator; // Reference to the Animator attached to the Game Over Panel

    private bool isGameOverTriggered = false; // Flag to prevent multiple triggers

    private void Start()
    {
        // player reference is assigned
        if (player == null)
        {
            
        }

        // animator is set up
        if (panelAnimator == null)
        {
            
        }
    }

    private void Update()
    {
        // Check if the player's health is 0
        if (player != null && player.currentHealth <= 0 && !isGameOverTriggered)
        {
            isGameOverTriggered = true; // Prevent further triggers
            TriggerGameOver();
        }

    }

    private void TriggerGameOver()
    {
        // Start the "SlideIn_Bottom" animation
        if (panelAnimator != null)
        {
            
        }

        // Start coroutine to play the next animation after the first one ends
        StartCoroutine(PlaySlideInOpenAnimation());
        PlaySlideOutBottomAnimation();


    }

    private IEnumerator PlaySlideInOpenAnimation()
    {
        // Wait for the "SlideIn_Bottom" animation to finish before triggering the next animation
        yield return new WaitForSeconds(panelAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Trigger the "SlideIn_Open" animation
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger("SlideIn_Open");
        }
    }

    private IEnumerator PlaySlideOutBottomAnimation()
    {
        // Wait for the "SlideOut_Bottom" animation to finish before triggering the next animation
        yield return new WaitForSeconds(panelAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Trigger the "SlideOut_Bottom" animation
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger("SlideOut_Bottom");
            Debug.Log($"animation is triggered");
        }
    }

    public void RevivePlayer()
    {
        if (player != null)
        {
            player.currentHealth += 100; // Add 100 to the player's current health
            player.currentHealth = Mathf.Min(player.currentHealth, player.maxHealth); // Ensure health doesn't exceed maxHealth
            isGameOverTriggered = false; // Reset the game over flag
            Debug.Log($"Player revived. Current Health: {player.currentHealth}");
        }
    }

}


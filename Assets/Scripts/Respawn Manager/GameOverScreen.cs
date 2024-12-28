using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class GameOverScreen : MonoBehaviour
{
    [Header("Respawn Manager Reference")]
    [SerializeField] private RespawnManager respawnManager; // Reference to the RespawnManager script

    [Header("Player Health Reference")]
    [SerializeField] private WalkMovement player; // Reference to the WalkMovement script

    [Header("Game Over Panel Reference")]
    [SerializeField] private RectTransform gameOverPanel; // Reference to the Game Over panel RectTransform



    private bool isGameOverTriggered = false; // Flag to prevent multiple triggers

    private void Start()
    {
        

    }

    private void Update()
    {
        // Trigger Game Over only when health is 0 and it hasn't already been triggered
        if (player != null && player.currentHealth <= 0 && !isGameOverTriggered)
        {
            isGameOverTriggered = true; // Prevent further triggers
            TriggerGameOver();
        }
        // Reset the game over state if health is above 0
        else if (player != null && player.currentHealth > 0)
        {
            isGameOverTriggered = false;
        }

    }

    private void TriggerGameOver()
    {
        Cursor.visible = true;

        // Set the off-screen starting position (bottom of the canvas)
        gameOverPanel.anchoredPosition = new Vector2(gameOverPanel.anchoredPosition.x, -1000f);

        // Define the target position (middle of the screen)
        Vector2 targetPosition = new Vector2(gameOverPanel.anchoredPosition.x, 540f);

        // Use DOTween to animate the position
        gameOverPanel.DOAnchorPos(targetPosition, 1f).SetEase(Ease.InOutQuad);

    }

    public void GamerOverFadeOut()
    {
        Cursor.visible = false;
        // Set the off-screen starting position (bottom of the canvas)
        gameOverPanel.anchoredPosition = new Vector2(gameOverPanel.anchoredPosition.x, 540f);

        // Define the target position (middle of the screen)
        Vector2 targetPosition = new Vector2(gameOverPanel.anchoredPosition.x, -1000f);

        // Use DOTween to animate the position
        gameOverPanel.DOAnchorPos(targetPosition, 1f).SetEase(Ease.InOutQuad);
    }

    public void RevivePlayer()
    {
        player.currentHealth += 100; // Add 100 to the player's current health
        player.currentHealth = Mathf.Min(player.currentHealth, player.maxHealth); // Ensure health doesn't exceed maxHealth
        player.OxygenGas += 100;
        player.OxygenGas = Mathf.Min(player.OxygenGas);
        Debug.Log($"Player revived. Current Health: {player.currentHealth}, Current Oxygen: {player.OxygenGas}");

        RespawnToMain();
        GamerOverFadeOut();
    }

    public void RespawnToMain()
    {
        if (respawnManager)
        {
            respawnManager.RespawnToMainCheckpoint();
        }
        else
        {
            Debug.LogWarning("RespawnManager is not assigned!");
        }
    }

}


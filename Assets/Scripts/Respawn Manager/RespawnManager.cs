using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("Main Checkpoint")]
    [SerializeField] private Transform mainCheckpoint;// this is the one and main checkpoint

    [SerializeField] private CheckpointData checkpointData; // Reference to current checkpoint
    [SerializeField] private Transform player; // Player object
    private List<CheckpointData> activeCheckpoints = new List<CheckpointData>();

    public void Respawn()
    {
        if (checkpointData.isActive)
        {
            player.position = checkpointData.position;
        }

        else
        {
            Debug.LogWarning("No active checkpoint found!");
        }

    }
    public void ActivateCheckpoint(CheckpointData checkpointData)
    {
        if (!activeCheckpoints.Contains(checkpointData))
        {
            activeCheckpoints.Add(checkpointData);
            checkpointData.isActive = true;
        }
    }

    // Reset any active checkpoints
    public void ResetActiveCheckpoints()
    {
        foreach (var checkpoint in activeCheckpoints)
        {
            checkpoint.isActive = false;
        }
        activeCheckpoints.Clear();
    }

    // Respawns the player to main checkpoint while resetting all active checkpoints
    public void RespawnToMainCheckpoint()
    {
        if (mainCheckpoint != null)
        {
            player.position = mainCheckpoint.position;
            ResetActiveCheckpoints();
            Debug.Log("Player respawned at the main checkpoint and active checkpoints reset.");
        }
        else
        {
            Debug.LogWarning("Main checkpoint is not assigned!");
        }
    }

}

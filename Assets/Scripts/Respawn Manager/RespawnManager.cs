using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
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
    private void ActivateCheckpoint()
    {
        checkpointData.isActive = true;
        checkpointData.position = transform.position;
    }

    // Respawn player at a specific checkpoint based on its name
    public void MainCheckPoint(string checkpointName)
    {
        CheckpointData specificCheckpoint = activeCheckpoints.Find(cp => cp.checkpointName == checkpointName);

        if (specificCheckpoint != null && specificCheckpoint.isActive)
        {
            player.position = specificCheckpoint.position;
            Debug.Log($"Player respawned at checkpoint: {checkpointName}");
        }
        else
        {
            Debug.LogWarning($"Checkpoint '{checkpointName}' not found or not active!");
        }
    }

}

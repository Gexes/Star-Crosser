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

}

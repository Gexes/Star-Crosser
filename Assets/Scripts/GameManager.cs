using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Checkpoint Data References")]
    [SerializeField] private List<CheckpointData> checkpoints; // List of all checkpoint ScriptableObjects

    private void Awake()
    {
        // Deactivate all checkpoints at the start of the game
        foreach (CheckpointData checkpoint in checkpoints)
        {
            if (checkpoint != null)
            {
                checkpoint.isActive = false;
            }
        }

        Debug.Log("All checkpoints have been reset.");
    }
}

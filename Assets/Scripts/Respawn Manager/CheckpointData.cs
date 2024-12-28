using UnityEngine;

[CreateAssetMenu(fileName = "NewCheckpoint", menuName = "Checkpoint/CheckpointData")]
public class CheckpointData : ScriptableObject
{
    public Vector3 position; // coordinates to the checkpoint
    public bool isActive; // Check to see if the checkpoint is active
}

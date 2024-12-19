using UnityEngine;

[CreateAssetMenu(fileName = "NewCheckpoint", menuName = "Checkpoint/CheckpointData")]
public class CheckpointData : ScriptableObject
{
    public string checkpointName;
    public Vector3 position;
    public bool isActive;
}

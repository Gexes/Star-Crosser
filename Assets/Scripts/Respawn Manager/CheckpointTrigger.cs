using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private CheckpointData checkpointData;
    [SerializeField] private UIController uiController; // Getting Reference to my UIController

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        // Mark this checkpoint as active
        checkpointData.isActive = true;

        // Update position in the checkpoint data
        checkpointData.position = transform.position;

        Debug.Log($"Checkpoint Activated at {checkpointData.position}");

        // Show the checkpoint popup
        if (uiController != null)
        {
            uiController.ShowCheckpointPopup("Checkpoint Activated!");
        }

        Debug.Log($"Checkpoint Activated at {checkpointData.position}");
    }
}


using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    void Start()
    {
        lastCheckpointPosition = transform.position; // Set initial position as last checkpoint
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition; // Update the last checkpoint position
    }

    public void Respawn()
    {
        transform.position = lastCheckpointPosition; // Move the player to the last checkpoint position
        // Implement any additional respawn logic if necessary
    }
}

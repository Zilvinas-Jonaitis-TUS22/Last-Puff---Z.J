using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActive = true; // Track if the checkpoint is active

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            // Save this checkpoint as the player's respawn position
            other.GetComponent<PlayerRespawn>().SetCheckpoint(transform.position);
            Debug.Log("Checkpoint Set");

            // Disable the checkpoint so it can only be set once
            isActive = false;
            GetComponent<Collider>().enabled = false; // Disable the collider to prevent re-triggering
        }
    }
}

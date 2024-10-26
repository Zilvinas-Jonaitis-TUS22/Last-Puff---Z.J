using UnityEngine;

public class KeyInteraction : MonoBehaviour
{
    private PlayerInventory playerInventory; // Reference to PlayerInventory for updating keys

    private void Start()
    {
        // Find the PlayerInventory in the scene
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player") && playerInventory != null)
        {
            // Add 1 to the player's key count
            playerInventory.keys++;
            Debug.Log("Key collected! Total keys: " + playerInventory.keys);

            Destroy(gameObject);
        }
    }
}

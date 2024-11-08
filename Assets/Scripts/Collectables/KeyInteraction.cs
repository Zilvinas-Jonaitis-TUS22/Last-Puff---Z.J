using UnityEngine;

public class KeyInteraction : MonoBehaviour
{
    private PlayerInventory playerInventory; // Reference to PlayerInventory for updating keys
    public AudioSource keyCollectAudioSource; // Audio source for key collection sound
    public Animator handAnimator;

    private void Start()
    {
        // Find the PlayerInventory in the scene
        playerInventory = FindObjectOfType<PlayerInventory>();

        // If there's no audio source set in the inspector, add one dynamically
        if (keyCollectAudioSource == null)
        {
            keyCollectAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player") && playerInventory != null)
        {
            // Play the key collection sound
            if (keyCollectAudioSource != null && keyCollectAudioSource.clip != null)
            {
                keyCollectAudioSource.Play();
            }

            // Add 1 to the player's key count
            playerInventory.keys++;
            Debug.Log("Key collected! Total keys: " + playerInventory.keys);

            //Play Interaction Animation
            handAnimator.SetTrigger("Interacting");

    // Destroy the key object after the sound finishes playing
    Destroy(gameObject, keyCollectAudioSource.clip.length);
        }
    }
}

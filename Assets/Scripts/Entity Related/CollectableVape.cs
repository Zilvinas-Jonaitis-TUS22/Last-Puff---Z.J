using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableVape : MonoBehaviour
{
    public float refillAmount = 20f; // Amount to refill the vape bar

    // Reference to the VapeScript (assign this in the Inspector or find it at runtime)
    private VapeScript vapeScript;

    public AudioSource collectSound; // AudioSource for collection sound

    void Start()
    {
        // Try to find the VapeScript component in the scene
        vapeScript = FindObjectOfType<VapeScript>();

        // If there's no AudioSource assigned in the inspector, add one dynamically
        if (collectSound == null)
        {
            collectSound = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with this one is the player
        if (other.CompareTag("Player")) // Make sure your player GameObject has the tag "Player"
        {
            // Refills the vape bar
            if (vapeScript != null)
            {
                vapeScript.vapeJuiceSlider.value += refillAmount; // Refill the vape juice slider
                vapeScript.vapeJuiceSlider.value = Mathf.Clamp(vapeScript.vapeJuiceSlider.value, 0, vapeScript.vapeJuiceAmount); // Clamp to max
            }

            // Play the collection sound
            if (collectSound != null && collectSound.clip != null)
            {
                collectSound.Play();
            }

            // Destroy the collectable object after the sound has finished playing
            Destroy(gameObject, collectSound.clip.length);
        }
    }
}

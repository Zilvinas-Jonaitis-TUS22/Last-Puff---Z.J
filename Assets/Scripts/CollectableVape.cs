using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableVape : MonoBehaviour
{
    public float refillAmount = 20f; // Amount to refill the vape bar

    // Reference to the VapeScript (assign this in the Inspector or find it at runtime)
    private VapeScript vapeScript;

    void Start()
    {
        // Try to find the VapeScript component in the scene
        vapeScript = FindObjectOfType<VapeScript>();
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

            // Optionally play a collection sound or animation here

            // Destroy the collectable object
            Destroy(gameObject);
        }
    }
}

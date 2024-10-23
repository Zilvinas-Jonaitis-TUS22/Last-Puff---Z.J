using StarterAssets;
using UnityEngine;
using UnityEngine.UI; // Add this if using Unity UI
// or using TMPro; // If you're using TextMeshPro

public class DoorInteraction : MonoBehaviour
{
    public Animator doorAnimator; // Reference to the Animator component
    public GameObject interactionPrompt; // Reference to the UI element
    private bool isPlayerInRange = false;
    private bool hasOpened = false; // Track if the door has been opened
    private StarterAssetsInputs inputs;
    private CrosshairManager crosshairManager; // Reference to the CrosshairManager

    private void Start()
    {
        inputs = FindObjectOfType<StarterAssetsInputs>();
        crosshairManager = FindObjectOfType<CrosshairManager>(); // Get the CrosshairManager reference
        interactionPrompt.SetActive(false); // Hide prompt at start
    }

    private void Update()
    {
        // Check if the player is interacting and if the door hasn't been opened
        if (isPlayerInRange && inputs.interacting && !hasOpened)
        {
            OpenDoor();
        }

        // Update the interacting state in CrosshairManager
        if (crosshairManager != null)
        {
            crosshairManager.interacting = isPlayerInRange; // Set interacting to true if in range
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionPrompt.SetActive(true); // Show the interaction prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionPrompt.SetActive(false); // Hide the interaction prompt
        }
    }

    private void OpenDoor()
    {
        // Trigger the door opening animation
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenDoor");
        }

        Debug.Log("Door opened!");
        hasOpened = true; // Mark the door as opened
        interactionPrompt.SetActive(false); // Hide the interaction prompt

        // Optionally, disable further interaction or add other logic here
    }
}

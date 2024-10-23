using StarterAssets;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Animator doorAnimator; // Reference to the Animator component
    private bool isPlayerInRange = false;
    private StarterAssetsInputs inputs;

    private void Start()
    {
        inputs = FindObjectOfType<StarterAssetsInputs>();
    }

    private void Update()
    {
        // Check if the player is interacting
        if (isPlayerInRange && inputs.interacting)
        {
            OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
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
        // Optionally, disable further interaction or add other logic here
    }
}

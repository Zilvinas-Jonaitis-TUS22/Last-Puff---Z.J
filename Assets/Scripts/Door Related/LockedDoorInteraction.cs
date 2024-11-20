using StarterAssets;
using UnityEngine;
// or using TMPro; // If you're using TextMeshPro

public class LockedDoorInteraction : MonoBehaviour
{
    public string doorColor = "";

    public Animator doorAnimator; // Reference to the Animator component
    public GameObject interactionPrompt; // Reference to the UI element
    public GameObject keyPrompt; // Reference to the UI element
    public BoxCollider triggerCollider;

    public Animator handAnimator;

    public bool isPlayerInRange = false;
    public bool hasOpened = false; // Track if the door has been opened
    private StarterAssetsInputs inputs;
    private CrosshairManager crosshairManager; // Reference to the CrosshairManager
    private PlayerInventory playerInventory; // Reference to PlayerInventory for checking keys

    //Audio
    public AudioSource theAudioSource;

    private void Start()
    {
        inputs = FindObjectOfType<StarterAssetsInputs>();
        crosshairManager = FindObjectOfType<CrosshairManager>(); // Get the CrosshairManager reference
        playerInventory = FindObjectOfType<PlayerInventory>(); // Get the PlayerInventory reference
        interactionPrompt.SetActive(false); // Hide prompt at start
        keyPrompt.SetActive(false); // Hide prompt at start
    }

    private void Update()
    {
        ShakeDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionPrompt.SetActive(true); // Show the interaction prompt
            keyPrompt.SetActive(true); // Show the key prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionPrompt.SetActive(false); // Hide the interaction prompt
            keyPrompt.SetActive(false); // Hide the key prompt
        }
    }

    public void ShakeDoor()
    {
        if (isPlayerInRange && inputs.interacting && !hasOpened)
        {
            doorAnimator.SetTrigger("DoorShake");
        }

    }
    public void OpenDoor(string color)
    {
        if (color == doorColor && isPlayerInRange)
        {
            // Trigger the door opening animation
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("OpenDoor");
            }
            //Play Interaction Animation
            handAnimator.SetTrigger("Interacting");

            triggerCollider.enabled = false;
            theAudioSource.Play();
            isPlayerInRange = false;
            hasOpened = true; // Mark the door as opened
            interactionPrompt.SetActive(false); // Hide the interaction prompt
            keyPrompt.SetActive(false); // Hide the key prompt

            // Optionally, disable further interaction or add other logic here
        }
    }
}

using UnityEngine;
using TMPro; // For TextMeshPro
using System.Collections; // For coroutines

public class TooltipTrigger : MonoBehaviour
{
    // Reference to the tooltip UI element
    public GameObject tooltipUI;
    public Animator UIanimator;
    public Animator WithdrawalImageAnimator;
    // TextMeshProUGUI component to display tooltip text
    public TextMeshProUGUI tooltipText;

    public string tooltipMessage = "Use your vape to avoid dying from withdrawal";

    // Time delay between each character in the typing effect
    public float typingSpeed = 0.05f;

    private bool tipShown = false; // Ensure the tip is only shown once

    private void Start()
    {
        // Ensure the tooltip is hidden initially
        tooltipUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the "Player" tag and the tooltip isn't already shown
        if (other.CompareTag("Player") && !tipShown)
        {
            ShowTooltip();
        }
    }

    // Method to show the tooltip with a typing effect
    void ShowTooltip()
    {
        tooltipUI.SetActive(true);
        UIanimator.SetTrigger("Open");
        WithdrawalImageAnimator.SetTrigger("Open");
        StartCoroutine(TypeText(tooltipMessage));
        tipShown = true;
    }

    // Coroutine to simulate typing the text over time
    IEnumerator TypeText(string message)
    {
        tooltipText.text = ""; // Clear the text initially
        foreach (char letter in message)
        {
            tooltipText.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed); // Wait for the typing speed
        }
    }
}

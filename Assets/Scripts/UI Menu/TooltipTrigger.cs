using UnityEngine;
using TMPro; // For TextMeshPro
using System.Collections; // For coroutines

public class TooltipTrigger : MonoBehaviour
{
    // Reference to the tooltip UI element
    public GameObject tooltipUI;
    public Animator UIanimator;
    public Animator WithdrawalImageAnimator;

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

    void ShowTooltip()
    {
        tooltipUI.SetActive(true);
        UIanimator.SetTrigger("Open");
        WithdrawalImageAnimator.SetTrigger("Open");
        tipShown = true;
        StartCoroutine(RemoveTextAfterDelay(8f));
    }

    public void HideTooltip()
    {
        UIanimator.SetTrigger("Close");
        WithdrawalImageAnimator.SetTrigger("Close");
    }
    IEnumerator RemoveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        HideTooltip();
    }

}

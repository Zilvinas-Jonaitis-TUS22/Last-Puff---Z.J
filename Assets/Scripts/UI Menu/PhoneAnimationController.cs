using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for using UI elements

public class PhoneAnimationController : MonoBehaviour
{
    // Animator reference
    private Animator animator;

    // UI elements to be controlled
    public GameObject imageToShow;      // Reference to the image GameObject

    // Audio sources for sounds
    public AudioSource soundBeforeRaise; // Sound to play before raising the phone
    public AudioSource soundDuringRaise; // Sound to play while the phone is raised and lowered

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();

        // Start the phone raise sequence
        StartCoroutine(RaisePhoneCoroutine());
    }

    private IEnumerator RaisePhoneCoroutine()
    {
        // Wait for 1 second before anything happens
        yield return new WaitForSeconds(10f);

        // Play the sound before raising the phone
        if (soundBeforeRaise != null)
        {
            soundBeforeRaise.Play();
        }

        // Wait for the sound to finish playing (optional)
        yield return new WaitForSeconds(soundBeforeRaise.clip.length - 2f);

        // Wait for a few seconds before raising the phone
        yield return new WaitForSeconds(0.01f); // Change this duration as needed

        // Set RaisedPhone to true
        animator.SetBool("RaisedPhone", true); // Directly using the string

        // Play the sound during raising the phone
        if (soundDuringRaise != null)
        {
            soundDuringRaise.Play();
        }

        // Wait for 1 second before showing the UI elements
        yield return new WaitForSeconds(1f);

        // Show the image and text elements
        if (imageToShow != null) imageToShow.SetActive(true);

        // Wait for 5 seconds while the phone is raised
        yield return new WaitForSeconds(5f);

        // Set RaisedPhone to false
        animator.SetBool("RaisedPhone", false); // Directly using the string

        // Hide the image and text elements
        if (imageToShow != null) imageToShow.SetActive(false);

        // Uncomment the line below if you want to play the sound again
        if (soundDuringRaise != null) soundDuringRaise.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMovement : MonoBehaviour
{
    public Transform player; // Assign the player object in the inspector
    public Animator animator; // Assign the Animator component in the inspector
    public AudioSource audioSource; // Assign the AudioSource component for looping sounds
    public AudioSource transitionAudioSource; // Assign the AudioSource component for transition sounds
    public AudioClip lostAudio; // Assign the audio clip for the lost state
    public AudioClip normalAudio; // Assign the audio clip for the normal state
    public AudioClip transitionAudio; // Assign the audio clip for the transition
    public float spottedRadius = 5f; // Radius to trigger "Spotted"
    public float lostRadius = 10f;   // Radius to trigger "Lost"

    private bool isLost = false; // Track whether the object is in the lost state

    void Start()
    {
        // Optionally initialize things here
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Keep the rotation on the horizontal plane

            // Rotate towards the player
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            // Check the distance to the player
            float distance = direction.magnitude;

            // Trigger "Spotted" if within spotted radius
            if (distance < spottedRadius)
            {
                animator.SetBool("Spotted", true);
                if (isLost)
                {
                    PlayTransitionAudio(); // Play transition audio when turning not lost
                    StopAudio(); // Stop lost audio
                    isLost = false; // Transition to not lost
                }
                PlayAudio(normalAudio, false); // Play normal audio once
            }
            else if (distance > lostRadius)
            {
                animator.SetBool("Spotted", false);
                animator.SetBool("Lost", true);
                if (!isLost)
                {
                    isLost = true; // Transition to lost
                    PlayAudio(lostAudio, true); // Loop lost audio
                }
            }
            else
            {
                animator.SetBool("Lost", false);
            }
        }
    }

    void PlayAudio(AudioClip clip, bool loop = false)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }

    void StopAudio()
    {
        audioSource.Stop();
    }

    void PlayTransitionAudio()
    {
        transitionAudioSource.PlayOneShot(transitionAudio); // Play transition audio once
    }

    void OnDrawGizmos()
    {
        // Draw the spotted radius
        Gizmos.color = Color.green; // Color for spotted radius
        Gizmos.DrawWireSphere(transform.position, spottedRadius);

        // Draw the lost radius
        Gizmos.color = Color.red; // Color for lost radius
        Gizmos.DrawWireSphere(transform.position, lostRadius);
    }
}

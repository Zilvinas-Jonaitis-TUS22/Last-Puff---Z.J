using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowMovement : MonoBehaviour
{
    public Transform player; // Assign the player object in the inspector
    public Animator animator; // Assign the Animator component in the inspector
    public AudioSource audioSource; // Assign the AudioSource component for looping sounds
    public AudioSource transitionAudioSource; // Assign the AudioSource component for transition sounds
    public AudioClip lostAudio; // Assign the audio clip for the lost state
    public AudioClip normalAudio; // Assign the audio clip for the normal state
    public AudioClip transitionAudio; // Assign the audio clip for the transition
    public List<Transform> patrolPoints; // List of patrol points
    public float spottedRadius = 5f; // Radius to trigger "Spotted"
    public float lostRadius = 10f;   // Radius to trigger "Lost"
    public float loseSightTime = 3f; // Time in seconds to lose sight if blocked

    private NavMeshAgent navAgent; // NavMesh agent component
    private bool isLost = false; // Track whether the object is in the lost state
    private int currentPatrolIndex = 0; // Current patrol point index
    private bool isFollowingPlayer = false; // Is the entity following the player
    private float lostSightTimer = 0f; // Timer for losing sight of the player

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            if (isFollowingPlayer)
            {
                // Following player - check if sight is lost
                bool canSeePlayer = CheckLineOfSight();

                if (distance < spottedRadius && canSeePlayer)
                {
                    // Maintain following state
                    lostSightTimer = 0f;
                    animator.SetBool("Spotted", true);
                    animator.SetBool("Lost", false);
                    navAgent.SetDestination(player.position);
                    PlayAudio(normalAudio, false);
                }
                else
                {
                    // Increment sight timer if sight is lost
                    lostSightTimer += Time.deltaTime;
                    if (lostSightTimer >= loseSightTime)
                    {
                        isFollowingPlayer = false;
                        animator.SetBool("Spotted", false);
                        animator.SetBool("Lost", true);
                        lostSightTimer = 0f;
                        GoToNextPatrolPoint();
                        PlayAudio(lostAudio, true);
                    }
                }
            }
            else
            {
                // Patrolling - check if player is within spotted radius and line of sight
                if (distance < spottedRadius && CheckLineOfSight())
                {
                    isFollowingPlayer = true;
                    animator.SetBool("Spotted", true);
                    PlayTransitionAudio();
                    PlayAudio(normalAudio, false);
                }
                else if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
                {
                    // Move to the next patrol point if close to current one
                    GoToNextPatrolPoint();
                }
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Count == 0) return;

        navAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
    }

    bool CheckLineOfSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, directionToPlayer);
        if (Physics.Raycast(ray, out RaycastHit hit, lostRadius))
        {
            // Check if the ray hit the player
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
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

        // Draw patrol points
        Gizmos.color = Color.blue;
        foreach (var patrolPoint in patrolPoints)
        {
            Gizmos.DrawWireSphere(patrolPoint.position, 0.5f);
        }
    }
}

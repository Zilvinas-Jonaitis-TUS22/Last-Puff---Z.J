using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowMovement : MonoBehaviour
{
    public Transform player; // Assign the player object in the inspector
    public Animator animator; // Assign the Animator component in the inspector
    public AudioSource audioSource; // Looping sound audio source
    public AudioSource transitionAudioSource; // Transition audio source
    public AudioSource jumpscareAudioSource; // Jumpscare audio source
    public AudioClip lostAudio; // Lost state audio clip
    public AudioClip normalAudio; // Normal state audio clip
    public AudioClip transitionAudio; // Transition audio clip
    public List<Transform> patrolPoints; // List of patrol points
    public float spottedRadius = 5f; // Radius to trigger "Spotted"
    public float lostRadius = 10f; // Radius to trigger "Lost"
    public float jumpScareRadius = 3f; // Radius to trigger jumpscare
    public float loseSightTime = 3f; // Time in seconds to lose sight if blocked

    private NavMeshAgent navAgent; // NavMesh agent component
    private int currentPatrolIndex = 0; // Patrol point index
    private bool isFollowingPlayer = false; // Is the entity following the player
    private float lostSightTimer = 0f; // Timer for losing sight of the player
    private PlayerRespawn playerRespawn; // Reference to PlayerRespawn script

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        playerRespawn = player.GetComponent<PlayerRespawn>();

        // Start the lost audio since the entity begins in patrol state
        PlayAudio(lostAudio, true);

        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // Check for jumpscare and respawn
        if (distance < jumpScareRadius)
        {
            TriggerJumpScare();
            return; // Skip other behavior when in jumpscare range
        }

        if (isFollowingPlayer)
        {
            bool canSeePlayer = CheckLineOfSight();

            if (distance < spottedRadius && canSeePlayer)
            {
                lostSightTimer = 0f;
                animator.SetBool("Spotted", true);
                animator.SetBool("Lost", false);
                navAgent.SetDestination(player.position);
                PlayAudio(normalAudio, false);
            }
            else
            {
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
                GoToNextPatrolPoint();
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
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    void PlayAudio(AudioClip clip, bool loop = false)
    {
        if (audioSource.clip != clip || !audioSource.isPlaying)
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

    void TriggerJumpScare()
    {
        if (jumpscareAudioSource != null && !jumpscareAudioSource.isPlaying)
        {
            jumpscareAudioSource.Play(); // Play jumpscare audio
        }

        if (playerRespawn != null)
        {
            playerRespawn.RespawnWithDeathScreen(); // Trigger player respawn with death screen
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Spotted radius
        Gizmos.DrawWireSphere(transform.position, spottedRadius);

        Gizmos.color = Color.red; // Lost radius
        Gizmos.DrawWireSphere(transform.position, lostRadius);

        Gizmos.color = Color.yellow; // Jumpscare radius
        Gizmos.DrawWireSphere(transform.position, jumpScareRadius);

        Gizmos.color = Color.blue;
        foreach (var patrolPoint in patrolPoints)
        {
            Gizmos.DrawWireSphere(patrolPoint.position, 0.5f);
        }
    }
}

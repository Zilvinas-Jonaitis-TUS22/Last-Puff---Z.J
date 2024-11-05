using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;
    public VideoPlayer deathScreenVideo;
    public GameObject normalUI;
    public AudioSource deathScreenAudio; 
    public AudioSource deathScreenAudio2; 

    void Start()
    {
        lastCheckpointPosition = transform.position;
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    public void RespawnWithDeathScreen()
    {
        StartCoroutine(ShowDeathVideoAndRespawn());
    }

    private IEnumerator ShowDeathVideoAndRespawn()
    {
        normalUI.SetActive(false);
        // Enable the video player
        if (deathScreenAudio != null && !deathScreenAudio.isPlaying)
        {
            deathScreenAudio.Play();
            deathScreenAudio2.Play();
        }
        deathScreenVideo.gameObject.SetActive(true);
        

        // Play the audio if it exists


        // Wait for 1 second before disabling the video
        yield return new WaitForSeconds(0.5f);

        // Move the player to the last checkpoint position
        transform.position = lastCheckpointPosition;

        // Wait an additional 0.5 seconds, then disable the video
        yield return new WaitForSeconds(0.5f);
        deathScreenVideo.gameObject.SetActive(false);
        normalUI.SetActive(true);
    }
}

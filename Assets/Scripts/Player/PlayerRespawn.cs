using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 lastCheckpointPosition;
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
        deathScreenVideo.gameObject.SetActive(true);
        StartCoroutine(ShowDeathVideoAndRespawn());
        transform.position = lastCheckpointPosition;
        Debug.Log("Respawn Succesful");
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
        yield return new WaitForSeconds(0.5f);
        
        // Wait an additional 0.5 seconds, then disable the video
        deathScreenVideo.gameObject.SetActive(false);
        normalUI.SetActive(true);
    }
}

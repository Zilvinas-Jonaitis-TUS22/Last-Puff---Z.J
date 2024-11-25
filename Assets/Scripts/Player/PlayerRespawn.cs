using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Video;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 lastCheckpointPosition;
    public GameObject normalUI;
    public CharacterController controller;

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
        controller.enabled = false;
        transform.position = lastCheckpointPosition;
        Debug.Log("Respawn Succesful");
        controller.enabled = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDoor : MonoBehaviour
{
    public GameObject nightCompleteUI;
    public GameObject playerUI;
    public GameObject thePlayer;
    public Transform homeLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nightCompleteUI.SetActive(true);
            thePlayer.GetComponent<WithdrawalScript>().enabled = false;
            playerUI.SetActive(false);
            thePlayer.GetComponent<CharacterController>().enabled = false;
            thePlayer.transform.position = homeLocation.position;
            thePlayer.transform.rotation = homeLocation.rotation;
            thePlayer.GetComponent<CharacterController>().enabled = true;
        }
    }
}

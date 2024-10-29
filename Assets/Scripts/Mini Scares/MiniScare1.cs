using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MiniScare1 : MonoBehaviour
{
    private bool enabledScare = false;
    public Light carLight;
    public AudioSource radio;
    public AudioSource staticSound;

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
        if (other.CompareTag("Player") && !enabledScare)
        {
            enabledScare = true;
            carLight.gameObject.SetActive(true);
            radio.Play();
            staticSound.Play();
        }
    }
}

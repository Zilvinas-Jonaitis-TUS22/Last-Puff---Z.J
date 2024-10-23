using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public Animator animator;

    public bool interacting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interacting)
        {
            animator.SetBool("Interactable", interacting);
        }
        else
        {
            animator.SetBool("Interactable", interacting);
        }
    }
}

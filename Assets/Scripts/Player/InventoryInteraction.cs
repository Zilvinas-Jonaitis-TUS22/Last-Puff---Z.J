using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteraction : MonoBehaviour
{
    public StarterAssetsInputs _input;
    public GameObject inventoryUI;
    public GameObject crosshairUI;
    public Animator leftHandAnimator;

    void Start()
    {
        leftHandAnimator.SetBool("InventoryOpen", false);
        inventoryUI.SetActive(false);
        crosshairUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.inventoryState)
        {
            leftHandAnimator.SetBool("InventoryOpen", true);
            inventoryUI.SetActive(true);
            crosshairUI.SetActive(false);
        }
        else
        {
            leftHandAnimator.SetBool("InventoryOpen", false);
            inventoryUI.SetActive(false);
            crosshairUI.SetActive(true);
        }
        
    }
}

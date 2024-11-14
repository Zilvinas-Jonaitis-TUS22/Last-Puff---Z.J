using StarterAssets;
using UnityEngine;

public class InventoryInteraction : MonoBehaviour
{
    public StarterAssetsInputs _input;
    public Animator inventoryUI;
    public GameObject crosshairUI;
    public Animator leftHandAnimator;

    void Start()
    {
        leftHandAnimator.SetBool("InventoryOpen", false);
        crosshairUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.inventoryState)
        {
            leftHandAnimator.SetBool("InventoryOpen", true);
            inventoryUI.SetBool("InventoryOpen", true);
            crosshairUI.SetActive(false);
        }
        else
        {
            leftHandAnimator.SetBool("InventoryOpen", false);
            inventoryUI.SetBool("InventoryOpen", false);
            crosshairUI.SetActive(true);
        }

    }
}

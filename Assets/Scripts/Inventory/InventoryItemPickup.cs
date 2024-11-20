using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemPickup : MonoBehaviour
{
    public Animator leftHandAnimator;
    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.tag == "Collectable")
        {
            InventoryObject theItemSO = other.gameObject.GetComponent<InventoryItem>().inventoryScriptableObject;
            GameManager.Instance.AddInventoryItem(theItemSO);
            leftHandAnimator.SetTrigger("Grabbing");
            Destroy(other.gameObject);
        }
    }
}

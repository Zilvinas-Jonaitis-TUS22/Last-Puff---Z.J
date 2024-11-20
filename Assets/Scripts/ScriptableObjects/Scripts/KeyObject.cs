using UnityEngine;


[CreateAssetMenu(fileName = "Default-Key", menuName = "Inventory System/Items/Key")]
public class KeyObject : InventoryObject
{

    [Header("Key Attributes")]
    public string color = "";

    public void ItemAction()
    {
        if (quantity > 0)
        {
            LockedDoorInteraction doorScript = FindObjectOfType<LockedDoorInteraction>();
            if (!doorScript.hasOpened && doorScript.isPlayerInRange)
            {
                doorScript.OpenDoor(color);
            }
        }

    }
}

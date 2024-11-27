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
            // Find all LockedDoorInteraction scripts in the scene
            LockedDoorInteraction[] doorScripts = FindObjectsOfType<LockedDoorInteraction>();

            foreach (var doorScript in doorScripts)
            {
                // Check if the door is not opened and the player is in range
                if (!doorScript.hasOpened && doorScript.isPlayerInRange)
                {
                    doorScript.OpenDoor(color);
                    // Optionally, you could reduce the quantity here if a single key is consumed per door
                    break; // Exit the loop if you only want to interact with one door
                }
            }
        }

    }
}

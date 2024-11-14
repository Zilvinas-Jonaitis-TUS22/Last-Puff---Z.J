using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<InventoryObject> inventoryList = new List<InventoryObject>();
    public InventoryPanelManager inventoryPanelManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddInventoryItem(InventoryObject itemToAdd)
    {
        bool newItem = true;

        // Loop through the inventory to see if we already have
        // this item

        /*
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (itemToAdd == inventoryList[i])
            {
                inventoryList[i].quantity++;
                newItem = false;
                break;
            }
        }
        */

        foreach (InventoryObject item in inventoryList)
        {
            if (itemToAdd == item)
            {
                item.quantity++;
                newItem = false;
                break;
            }
        }

        if (newItem)
        {
            itemToAdd.quantity++;
            inventoryList.Add(itemToAdd);
            inventoryPanelManager.AddItem(itemToAdd);
        }

        inventoryPanelManager.UpdateItems(inventoryList);

    }

    public void UpdateInventory()
    {
        inventoryPanelManager.UpdateItems(inventoryList);
    }

    private void OnDestroy()
    {
        for (int i = 0;i < inventoryList.Count;i++)
        {
            inventoryList[i].quantity = 0;
        }
    }
}

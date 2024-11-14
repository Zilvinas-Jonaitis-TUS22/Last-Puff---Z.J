using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelManager : MonoBehaviour
{
    public List<GameObject> inventoryItemPanels = new List<GameObject>();

    private int activeItemPanels;
    public void AddItem(InventoryObject item)
    {
        activeItemPanels++;

        if (activeItemPanels < inventoryItemPanels.Count)
        {
            inventoryItemPanels[activeItemPanels - 1].SetActive(true);
            inventoryItemPanels[activeItemPanels - 1].GetComponent<InventoryItemPanelManager>().SetImage(item.UIImage);
            if (item is VapeObject vapeItem)
            {
                inventoryItemPanels[activeItemPanels - 1].GetComponent<InventoryItemPanelManager>().SetData(vapeItem);
            }
        }
    }

    public void UpdateItems(List<InventoryObject> inventory)
    {
        for (int i=0; i < inventory.Count; i++)
        {
            int q = inventory[i].quantity;
            inventoryItemPanels[i].GetComponent<InventoryItemPanelManager>().UpdateQuantity(q);
        }
    }
}

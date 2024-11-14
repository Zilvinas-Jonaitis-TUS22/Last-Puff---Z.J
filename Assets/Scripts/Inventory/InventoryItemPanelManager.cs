using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.LightingExplorerTableColumn;

public class InventoryItemPanelManager : MonoBehaviour
{
    public InventoryObject dataType;
    public TMP_Text tmpQuantity;
    public void SetImage(Sprite uiImage)
    {
        GetComponent<Image>().sprite = uiImage;
    }

    public void UpdateQuantity(int quantity)
    {
        tmpQuantity.text = "" + quantity;
    }

    public void SetData(InventoryObject dataAdded)
    {
        dataType = dataAdded;
    }
    public void ClickedOn()
    {
        if (dataType != null)
        {
            if (dataType is VapeObject vapeObject)
            {
                vapeObject.ItemAction();
            }
        }
    }
}

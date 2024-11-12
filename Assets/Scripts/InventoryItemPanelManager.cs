using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemPanelManager : MonoBehaviour
{
    public TMP_Text tmpQuantity;
    public void SetImage(Sprite uiImage)
    {
        GetComponent<Image>().sprite = uiImage;
    }

    public void UpdateQuantity(int quantity)
    {
        tmpQuantity.text = "" + quantity;
    }
}

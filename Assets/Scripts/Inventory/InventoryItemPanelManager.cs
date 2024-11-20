using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemPanelManager : MonoBehaviour
{
    public InventoryObject emptySlot;
    public InventoryObject dataType;
    private GameManager _gameManager;
    public TMP_Text tmpQuantity;

    public void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    public void SetImage(Sprite uiImage)
    {
        GetComponent<Image>().sprite = uiImage;
    }

    public void UpdateQuantity(int quantity)
    {
        tmpQuantity.text = "" + quantity;
        if (quantity == 0)
        {
            SetImage(emptySlot.UIImage);
        }
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
            else if (dataType is KeyObject keyObject)
            {
                keyObject.ItemAction();
            }
            //Update Quanitity
            _gameManager.UpdateInventory();
        }
    }
}

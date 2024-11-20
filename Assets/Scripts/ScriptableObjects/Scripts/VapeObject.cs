using UnityEngine;


[CreateAssetMenu(fileName = "Default-Vape", menuName = "Inventory System/Items/Vape")]
public class VapeObject : InventoryObject
{

    [Header("Vape Attributes")]
    public float refillAmount = 50f; // Amount to refill the vape bar

    public void ItemAction()
    {
        // Find the VapeScript in the scene
        if (quantity > 0)
        {
            VapeScript vapeScript = FindObjectOfType<VapeScript>();
            vapeScript.vapeJuiceAmount += refillAmount;
            vapeScript.vapeJuiceSlider.value += refillAmount;
            //Minus quanitity
            quantity--;
        }

    }
}

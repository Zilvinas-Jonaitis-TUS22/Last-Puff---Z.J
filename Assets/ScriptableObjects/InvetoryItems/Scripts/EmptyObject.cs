using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Default-Vape", menuName = "Inventory System/Items/Vape")]
public class EmptyObject : InventoryObject
{
    [Header("Vape Attributes")]
    public float refillAmount = 50f; // Amount to refill the vape bar

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : ScriptableObject
{
    [Header("Default Attributes")]
    public Sprite UIImage;
    public string description;
    public int quantity;
    //public AudioClip collectSound;
}

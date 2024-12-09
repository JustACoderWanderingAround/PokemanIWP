using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New inventory item", menuName = "Inventory/Items")]
public class InventoryItem : ScriptableObject
{
    [SerializeField]
    public string itemName;
    [SerializeField]
    public bool isStackable = false;
    public int numberInInventory = 0;
    public bool resetOnStart = false;
}

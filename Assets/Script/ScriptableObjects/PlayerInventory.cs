using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<InventoryItem> inventoryItems;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        foreach (InventoryItem item in inventoryItems)
        {
            if (item.resetOnStart)
            {
                item.numberInInventory = 0;
            }
        }
    }
    public void AddItem(string name)
    {
        Debug.Log("New item name: " + name);
        foreach (InventoryItem item in inventoryItems)
        {
            if (item.itemName == name)
            {
                item.numberInInventory++;
                Debug.Log("Item added!");
                break;
            }
        }
    }
    public void RemoveItem(string name)
    {
        foreach (InventoryItem item in inventoryItems)
        {
            if (item.itemName == name)
            {
                item.numberInInventory--;
                break;
            }
        }
    }
    public int GetCount(string name)
    {
        foreach (InventoryItem item in inventoryItems)
        {
            if (item.itemName == name)
            {
                return item.numberInInventory;
            }
        }
        return -1;
    }
}

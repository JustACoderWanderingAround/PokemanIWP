using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu]
public class PlayerInventory : SingletonScriptableObject<PlayerInventory>
{
    [SerializeField]
    private PlayerInventorySO m_invSO;
    private void Awake()
    {
        //DontDestroyOnLoad(this);
        foreach (InventoryItem item in m_invSO.inventoryItems)
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
        foreach (InventoryItem item in m_invSO.inventoryItems)
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
        foreach (InventoryItem item in m_invSO.inventoryItems)
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
        foreach (InventoryItem item in m_invSO.inventoryItems)
        {
            if (item.itemName == name)
            {
                return item.numberInInventory;
            }
        }
        return -1;
    }
}

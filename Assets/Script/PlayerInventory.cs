using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu]
public class PlayerInventory : SingletonScriptableObject<PlayerInventory>
{
    [SerializeField]
    private PlayerInventorySO m_invSO;
    public System.Action<InventoryItem> AddItemAction;
    public System.Action<InventoryItem> RemoveItemAction;
    private void Start()
    {
        Debug.LogWarning("PlayerInvStart");
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

                if ((!item.isStackable && item.numberInInventory < 1) || (item.isStackable))
                {
                    item.numberInInventory++;
                    if (AddItemAction != null)
                        AddItemAction.Invoke(item);
                }
                Debug.Log("Item added!");
                break;
            }
        }
    }
    public void RemoveItem(string name, int amount = 1)
    {
        foreach (InventoryItem item in m_invSO.inventoryItems)
        {
            if (item.itemName == name)
            {
                if (RemoveItemAction != null)
                    RemoveItemAction.Invoke(item);
                item.numberInInventory -= amount;
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
    public List<InventoryItem> GetItems()
    {
        return m_invSO.inventoryItems;
    }
    public void ResetItemCount(string name)
    {
        foreach (InventoryItem item in m_invSO.inventoryItems)
        {
            if (item.itemName == name)
            {
                item.numberInInventory = 0;
                break;
            }
        }
    }
    public GameObject GetItemPrefab(string itemName)
    {
        foreach (InventoryItem item in m_invSO.inventoryItems)
        {
            if (item.itemName == itemName)
            {
                return item.itemPrefab;
            }
        }
        return null;
    }
}

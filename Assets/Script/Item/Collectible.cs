using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, IInteractable
{
    public string inventoryItemName;

    public string OnHover()
    {
        return "Interact to add to inventory.";
    }

    public void OnInteract()
    {
        PlayerInventory m_inv = PlayerInventory.Instance;
        m_inv.AddItem(inventoryItemName);
        Debug.Log(inventoryItemName + "Collected~!");
        Destroy(gameObject);
    }
}

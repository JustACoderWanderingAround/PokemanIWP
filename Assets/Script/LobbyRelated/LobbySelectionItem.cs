using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySelectionItem : MonoBehaviour, IInteractable
{
    bool isInInventory;
    public string InventoryItemName;
    public void OnInteract()
    {
        Destroy(gameObject);
    }
}

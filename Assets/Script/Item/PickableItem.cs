using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public string inventoryItemName;

    public void OnHover()
    {
    }

    public void OnInteract()
    {
        PlayerInventory.Instance.AddItem(inventoryItemName);
        Destroy(gameObject);
    }
}

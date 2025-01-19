using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySelectionItem : MonoBehaviour, IInteractable
{
    bool isInInventory = false;
    public string InventoryItemName;
    float interactTimer = 0.01f;
    [SerializeField]
    Renderer indicatorRenderer;
    private void Start()
    {
        PlayerInventory.Instance.ResetItemCount(InventoryItemName);
    }
    private void Update()
    {
        if (interactTimer > 0)
            interactTimer -= Time.deltaTime;
    }
    public void OnInteract()
    {
        if (interactTimer <= 0)
        {
            isInInventory = !isInInventory;
            interactTimer = 0.01f;
            indicatorRenderer.material.color = isInInventory ? Color.green : Color.red;
        }
    }
    private void OnDestroy()
    {
        if (isInInventory)
        {
            PlayerInventory.Instance.AddItem(InventoryItemName);
            Debug.Log("Item added!" + InventoryItemName);
        }
    }

    public string OnHover()
    {
        return "Interact to take/leave item during your next mission.";
    }
}

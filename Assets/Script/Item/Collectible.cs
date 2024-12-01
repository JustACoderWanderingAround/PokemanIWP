using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        PlayerInventory m_inv = FindObjectOfType<PlayerInventory>();
        m_inv.AddItem("Objective");
        Debug.Log("Collected~!");
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
            OnInteract();
    }
}

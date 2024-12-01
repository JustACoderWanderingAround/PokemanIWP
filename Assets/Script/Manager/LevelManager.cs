using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    PlayerInventory inventory;
    [SerializeField]
    EndTeleporter endTeleporter;

    private void Update()
    {
        if (inventory.GetCount("Objective") >= 2)
        {
            endTeleporter.ActivateTeleporter();
            Debug.Log("Teleporter active!");
        }
    }
}

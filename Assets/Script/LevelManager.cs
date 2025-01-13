using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    MapGenerator mapGenerator;
    [SerializeField]
    EnemyManager enemyManager;
    [SerializeField]
    GameObject playerObject;
    UnityEvent ObjectiveCheckEvent;
    //[SerializeField]
    //PlayerInventory inventory;
    //[SerializeField]
    //EndTeleporter endTeleporter;

    //private void Update()
    //{
    //    if (inventory.GetCount("Objective") >= 2)
    //    {
    //        endTeleporter.ActivateTeleporter();
    //        Debug.Log("Teleporter active!");
    //    }
    //}

    private void Start()
    {
        mapGenerator.InitGenerator();
        mapGenerator.StartGeneration();
        Transform[] potentialLocs = mapGenerator.GetGridGameObjectParent().GetComponentsInChildren<Transform>();
        enemyManager.SpawnEnemies(potentialLocs);
        Vector2 startPosInd = mapGenerator.GetMapData()[0];
        playerObject.transform.position = mapGenerator.GetGridPosFromIndex((int)startPosInd.x, (int)startPosInd.y);
    }
}

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
    ObjectiveManager objectiveManager;
    Objective mainObjective;
    PlayerInventory playerInventory;
    EndTeleporter endTeleporter;
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
        objectiveManager = ObjectiveManager.Instance;
        playerInventory = PlayerInventory.Instance;
        mapGenerator.InitGenerator();
        mapGenerator.StartGeneration();
        //mainObjective = new Objective("TestCollectible", "Gold elixirs collected: {0}/{1}", 3);
        //objectiveManager.AddObjective(mainObjective);
        mainObjective.OnComplete = () => endTeleporter.ActivateTeleporter();
        playerInventory.AddItemAction += objectiveManager.CheckInventory;
        Transform[] potentialLocs = mapGenerator.GetGridGameObjectParent().GetComponentsInChildren<Transform>();
        enemyManager.SpawnEnemies(potentialLocs);
        Vector2 startPosInd = mapGenerator.GetMapData()[0];
        playerObject.transform.position = mapGenerator.GetGridPosFromIndex((int)startPosInd.x, (int)startPosInd.y);
    }
}

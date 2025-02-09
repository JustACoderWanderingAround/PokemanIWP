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
    [SerializeField]
    GameObject EndScreen;
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

    static LevelManager mInstance;

    public static LevelManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject();
                mInstance = go.AddComponent<LevelManager>();
            }
            return mInstance;
        }
    }

    private void Start()
    {
        objectiveManager = ObjectiveManager.Instance;
        playerInventory = PlayerInventory.Instance;
        mapGenerator.InitGenerator();
        mapGenerator.StartGeneration();
        endTeleporter = FindObjectOfType<EndTeleporter>();
        List<Objective> objectives = objectiveManager.FindObjectives("MainObjective");
        if (objectives != null)
        {
            mainObjective = objectives[0];
            mainObjective.OnComplete += () => endTeleporter.ActivateTeleporter();
            mainObjective.OnComplete += () => OnFirstMainObjectiveCompleted();
            Debug.LogWarning("Objective set successfully!");
        }
        else
        {
            Debug.LogError("OBJECTIVES IS NULL! WHAT THE FUCK ARE YOU DOING!");
        }
        playerInventory.AddItemAction += objectiveManager.CheckInventory;
        
        mapGenerator.GenerateObjectives(mainObjective, playerInventory.GetItemPrefab(mainObjective.ComparisonStr));
        Transform[] potentialLocs = mapGenerator.GetGridGameObjectParent().GetComponentsInChildren<Transform>();
        List<Transform> potentialLocList = new List<Transform>();
        potentialLocList.AddRange(potentialLocs);
        for(int i = 0; i < potentialLocList.Count; ++i)
        {
            if (potentialLocList[i].gameObject.name.Contains("EndTile") || potentialLocList[i].gameObject.name.Contains("Start")) {
                potentialLocList.Remove(potentialLocList[i]);
            }
        }
        enemyManager.SpawnEnemies(potentialLocList.ToArray());
        Vector2 startPosInd = mapGenerator.GetMapData()[0];
        playerObject.GetComponent<MainPlayerController>().OnDeathEvent += OnPlayerGameEnd;
        playerObject.transform.position = mapGenerator.GetGridPosFromIndex((int)startPosInd.x, (int)startPosInd.y);
    }
    public void OnFirstMainObjectiveCompleted()
    {
        //ObjectiveManager.Instance.RemoveObjective("MainObjective");
        objectiveManager.AddObjective(new Objective("MainObjective", "Find the teleporter and escape.", 0));
    }
    private void OnDestroy()
    {
        ObjectiveManager.Instance.RemoveObjective("MainObjective");
    }
    public void OnPlayerGameEnd(bool win)
    {

    }
}

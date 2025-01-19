using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySelectObjectiveManager : MonoBehaviour
{
    [SerializeField]
    GameObject selObjPrefab;
    public List<GameObject> objectiveSlots;
    List<LobbySelectionObjective> selectionObjects;
    int selectedindex;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < objectiveSlots.Count; i++)
        {
            LobbySelectionObjective newInst = Instantiate(selObjPrefab, objectiveSlots[i].transform).GetComponent<LobbySelectionObjective>();

            selectionObjects.Add(newInst);

            newInst.index = i;

            newInst.OnSelection += OnObjectiveSelected;
        }
    }
    void OnObjectiveSelected(int index)
    {
        for(int i = 0; i < objectiveSlots.Count; i++)
        {
            selectionObjects[i].DeSelect();
        }
    }
    private void OnDestroy()
    {
        ObjectiveManager.Instance.AddObjective(new Objective("MainObjective", selectionObjects[selectedindex].selectedUIstr, selectionObjects[selectedindex].selectedAmount));
    }
}

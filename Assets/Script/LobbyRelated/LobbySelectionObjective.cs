using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySelectionObjective : MonoBehaviour, IInteractable
{
    float interactTimer = 0.01f;
    [SerializeField]
    Renderer indicatorRenderer;
    bool selected = false;
    public List<string> uiStrings = new List<string>();
    public List<GameObject> allowedObjectiveObjects = new List<GameObject>();
    GameObject selectedObject;
    public string selectedUIstr;
    [HideInInspector]
    public int selectedAmount;
    public int index;
    public System.Action<int> OnSelection;
    enum ObjectiveType
    {
        OT_Collect,
        OT_Kill,
        num_OT
    }
    ObjectiveType m_OT = ObjectiveType.OT_Collect;
    private void Start()
    {
        selectedAmount = Random.Range(1, 4);
        selectedObject = allowedObjectiveObjects[Random.Range(0, allowedObjectiveObjects.Count)];
        if (selectedObject.GetComponent<Enemy>())
        {
            m_OT = ObjectiveType.OT_Kill;
        }
    }
    public string OnHover()
    {
        string returnstr = "Interact with this objective to select it as your main mission\n";
        switch (m_OT)
        {
            case ObjectiveType.OT_Collect:
                selectedUIstr = "Collect " + selectedAmount + " " + selectedObject.gameObject.GetComponent<Collectible>().inventoryItemName;
                break;
            case ObjectiveType.OT_Kill:
                selectedUIstr = "Kill " + selectedAmount + " " + selectedObject.gameObject.name;
                break;
        }
        returnstr += selectedUIstr;
        return returnstr;
    }

    public void OnInteract()
    {
        OnSelection.Invoke(index);
        selected = true;
        indicatorRenderer.material.color = Color.green;
    }
    public void DeSelect()
    {
        selected = false;
        indicatorRenderer.material.color = Color.red;
    }
}

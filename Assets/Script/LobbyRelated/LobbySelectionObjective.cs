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
    public GameObject selectedObject;
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
        selectedAmount = Random.Range(2, 5);
        selectedObject = allowedObjectiveObjects[Random.Range(0, allowedObjectiveObjects.Count)];
        GameObject poo = Instantiate(selectedObject, this.transform);
        Destroy(poo.GetComponent<Collectible>());
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
                selectedUIstr = "Collect " + selectedAmount + " " + selectedObject.gameObject.GetComponent<Collectible>().inventoryItemName + "\n" + selectedObject.gameObject.GetComponent<Collectible>().inventoryItemName + "collected: {0} / {1}";
                break;
            case ObjectiveType.OT_Kill:
                selectedUIstr = "Kill " + selectedAmount + " " + selectedObject.gameObject.name + "\nZombies killed: {0} / {1}"; ;
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

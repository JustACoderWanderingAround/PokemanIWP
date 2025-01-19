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
    public string selectedUIstr;
    public int selectedAmount;
    public int index;
    public System.Action<int> OnSelection;
    private void Start()
    {
        
    }
    public string OnHover()
    {
        return "Interact with this objective to select it as your main mission";
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

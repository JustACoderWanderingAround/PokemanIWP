using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{

    [SerializeField]
    TMP_Text objectiveText;
    [SerializeField]
    TMP_Text hoverText;
    [SerializeField]
    PlayerInteractable m_interactable;
    ObjectiveManager objectiveManager;

    Objective m_obj;
    // Start is called before the first frame update
    void Start()
    {
        objectiveManager = ObjectiveManager.Instance;
        m_interactable.OnHoverStringUpdate += UpdateHoverText;
        m_obj = objectiveManager.FindObjectives("MainObjective")[0];
        if (m_obj == null)
            UpdateObjectiveText(m_obj);
        objectiveManager.OnObjectiveUpdated += UpdateObjectiveText;
    }

    public void UpdateHoverText(string newText)
    {
        hoverText.text = newText;
    }
    public void UpdateObjectiveText(Objective objective)
    {
        objectiveText.text = "Current objective: \n" + objective.GetStatusText();
    }
}

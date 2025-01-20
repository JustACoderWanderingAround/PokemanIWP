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

    // Start is called before the first frame update
    void Start()
    {
        m_interactable.OnHoverStringUpdate += UpdateHoverText;
    }

    void UpdateHoverText(string newText)
    {
        hoverText.text = newText;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestObjectiveUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text m_Text;
    PlayerInventory m_Inventory;
    // Start is called before the first frame update
    void Start()
    {
        m_Inventory = PlayerInventory.instance;
    }

    // Update is called once per frame
    void Update()
    {
        m_Text.text = m_Inventory.GetCount("Objective").ToString() + "/2";
    }
}

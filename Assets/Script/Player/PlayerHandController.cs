using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : UseInputController
{
    /// <summary>
    /// HANDLES:
    /// HOLDING OBJECTS
    /// PICKING OBJECTS UP
    /// DROPPING OBJECTS
    /// </summary>
    [SerializeField]
    private GameObject leftHandSlot;
    [SerializeField]
    private GameObject rightHandSlot;
    public KeyCode secondaryButtonCode = KeyCode.LeftControl;
    public List<KeyCode> itemSlotKeyList = new List<KeyCode>() { 
        KeyCode.Alpha1,
        KeyCode.Alpha2, 
        KeyCode.Alpha3, 
        KeyCode.Alpha4, 
        KeyCode.Alpha5,
        KeyCode.Alpha6, 
        KeyCode.Alpha7, 
        KeyCode.Alpha8, 
        KeyCode.Alpha9, 
        KeyCode.Alpha0 
    }; 
    bool isSecondaryButtonPressed;
    int activeLHandIndex;
    int activeRHandIndex;
    private void Start()
    {
        activeLHandIndex = 0;
        activeRHandIndex = 0;

        PlayerInventory m_inv = PlayerInventory.Instance;

        foreach (InventoryItem ii in m_inv.GetItems())
        {
            if (ii.numberInInventory > 0)
            {
                UsableItem currUI = ii.itemPrefab.GetComponent<UsableItem>();
                if (currUI != null)
                {
                    Instantiate(ii.itemPrefab, currUI.IsRightHanded() ? rightHandSlot.transform : leftHandSlot.transform);
                }
            }
        }
    }

    public override void ReadCommand(Command cmd)
    {
        if (cmd as KeyCodeCommand != null)
        {
            KeyCodeCommand kcc = cmd as KeyCodeCommand;
            if (kcc.KeycodeNumber == secondaryButtonCode)
            {
                isSecondaryButtonPressed = kcc.KeyHeldDown || kcc.KeyDown;
                Debug.Log("SecondaryButtonPressed");
            }
            int counter = 0;
            foreach (KeyCode kc in itemSlotKeyList)
            {
                if (kcc.KeycodeNumber == kc)
                {

                    if (kcc.KeyDown)
                    {
                        if (isSecondaryButtonPressed)
                        {
                            SelectLeftHandItem(counter);
                        }
                        else
                        {
                            SelectRightHandItem(counter);
                        }
                    }
                }
                counter++;
            }
        }
        if (cmd as MouseButtonCommand != null)
        {
            MouseButtonCommand mbc = cmd as MouseButtonCommand;
            if (mbc.MouseDown)
            {
                if (mbc.MouseButton == 0)
                {
                    if (isSecondaryButtonPressed)
                    {                        
                        UseItem(1);
                    }
                    else
                    {
                        UseItem(0);
                    }

                }
                else if (mbc.MouseButton == 1)
                {
                    if (isSecondaryButtonPressed)
                    {
                        UseItemSecondary(1);
                    }
                    else
                    {
                        UseItemSecondary(0);
                    }
                }
            }
        }
    }

    public override void UpdateController(double deltaTime)
    {
        
    }
    public void Drop(int handNum)
    {

    }
    /// <summary>
    /// Use the primary function of the item in handNum's hand
    /// </summary>
    /// <param name="handNum">The number of the hand to use the item in. 0 is right, 1 is left</param>
    public bool UseItem(int handNum)
    {
        if (handNum == 1)
        {

            if (activeLHandIndex < leftHandSlot.transform.childCount)
            {
                if (leftHandSlot.transform.GetChild(activeLHandIndex) != null)
                    return leftHandSlot.transform.GetChild(activeLHandIndex).GetComponent<UsableItem>().PrimaryUse();
                else return false;
            }
            else return false;
        }
        else if (handNum == 0)
        {
            if (activeLHandIndex < leftHandSlot.transform.childCount)
            {
                if (rightHandSlot.transform.GetChild(activeRHandIndex) != null)
                    return rightHandSlot.transform.GetChild(activeRHandIndex).GetComponent<UsableItem>().PrimaryUse();
                else return false;
            }
            else return false;
        }
        else
            return false;
    }
    /// <summary>
    /// Use the secondary function of the item in handNum's hand
    /// </summary>
    /// <param name="handNum">The number of the hand to use the item in. 0 is right, 1 is left</param>
    public bool UseItemSecondary(int handNum)
    {
        if (handNum == 1)
        {
            if (activeLHandIndex < rightHandSlot.transform.childCount)
            {
                if (leftHandSlot.transform.GetChild(activeLHandIndex) != null)
                    return leftHandSlot.transform.GetChild(activeLHandIndex).GetComponent<UsableItem>().SecondaryUse();
                else return false;

            }
            else
            {
                activeLHandIndex = 0;
                return false;
            }
        }
        else if (handNum == 0)
        {
            if (activeRHandIndex < rightHandSlot.transform.childCount)
            {
                if (rightHandSlot.transform.GetChild(activeRHandIndex) != null)
                    return rightHandSlot.transform.GetChild(activeRHandIndex).GetComponent<UsableItem>().SecondaryUse();
                else return false;
            }
            else
            {
                activeRHandIndex = 0;
                return false;
            }
        }
        else
            return false;
    }
    public void CheckAndPickUp()
    {
        // Raycast to check what's around the player
        // Create Copy of item's prefab
        // Delete item copy on the floor

    }
    public void SelectLeftHandItem(int newActiveLItemIndex)
    {
        if (newActiveLItemIndex < leftHandSlot.transform.childCount)
        {
            leftHandSlot.transform.GetChild(activeLHandIndex).gameObject.SetActive(false);
            activeLHandIndex = newActiveLItemIndex;
            leftHandSlot.transform.GetChild(activeLHandIndex).gameObject.SetActive(true);
        }
    }
    public void SelectRightHandItem(int newActiveRItemIndex)
    {
        if (newActiveRItemIndex < rightHandSlot.transform.childCount)
        {
           rightHandSlot.transform.GetChild(activeRHandIndex).gameObject.SetActive(false);
           activeRHandIndex = newActiveRItemIndex;
           rightHandSlot.transform.GetChild(activeRHandIndex).gameObject.SetActive(true);
        }
    }
}

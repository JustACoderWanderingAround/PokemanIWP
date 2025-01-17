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
    bool isSecondaryButtonPressed;

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
            if (leftHandSlot.transform.GetChild(0) != null)
                return leftHandSlot.transform.GetChild(0).GetComponent<UsableItem>().PrimaryUse();
            else return false;
        }
        else if (handNum == 0)
        {
            if (rightHandSlot.transform.GetChild(0) != null)
                return rightHandSlot.transform.GetChild(0).GetComponent<UsableItem>().PrimaryUse();
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
            if (leftHandSlot.transform.GetChild(0) != null)
                return leftHandSlot.transform.GetChild(0).GetComponent<UsableItem>().SecondaryUse();
            else return false;
        }
        else if (handNum == 0)
        {
            if (rightHandSlot.transform.GetChild(0) != null)
                return rightHandSlot.transform.GetChild(0).GetComponent<UsableItem>().SecondaryUse();
            else return false;
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
}

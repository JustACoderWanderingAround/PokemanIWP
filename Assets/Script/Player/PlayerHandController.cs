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
    public KeyCode secondaryButtonCode = KeyCode.LeftAlt;
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
                        leftHandSlot.transform.GetChild(0).gameObject.SetActive(true);
                    }

                }
                else if (mbc.MouseButton == 1)
                {
                    if (isSecondaryButtonPressed)
                    {
                        leftHandSlot.transform.GetChild(0).gameObject.SetActive(false);
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
    public void UseItem(int handNum)
    {

    }
    public void UseItemSecondary(int handNum)
    {

    }
    public void CheckAndPickUp()
    {
        // Raycast to check what's around the player
        // Create Copy of item's prefab
        // Delete item copy on the floor

    }
}

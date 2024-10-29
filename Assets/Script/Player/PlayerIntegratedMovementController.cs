using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntegratedMovementController : UseInputController
{
    /// <summary>
    /// TODO: 
    /// 1. Integrate player rotation on mouse move (using mouse axis)
    /// 2. Integrate player position on movement axies (using GetAxis vert and hori)
    /// 3. Create keybindings struct/scriptableObj to allow input data to come from 1 central place
    /// </summary>
    
    // 
    [SerializeField]
    GameObject orientation;

    // private script only variables
    private Vector3 centreOfMass;
    bool canJump = true;
    bool grounded;
    public bool Grounded { get { return grounded; } set { grounded = value; } }

    public override void ReadCommand(Command cmd)
    {
        if (cmd as MovementAxisCommand != null)
        {

        }
        else if (cmd as KeyCodeCommand != null)
        {

        }
        else if (cmd as MouseButtonCommand != null)
        {

        }
    }

    public override void UpdateController(double deltaTime)
    {
    }

}

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
    
    // I forgot what this is
    [SerializeField]
    GameObject orientation;

    [SerializeField]
    float currentMoveSpeed = 2.0f;
    [SerializeField]
    float standingPlayerHeight = 1.0f;
    // private script only variables
    // private script only variabls
    private Vector3 direction;
    private Vector3 centreOfMass;
    private float horizontal;
    private float vertical;
    bool canJump = true;
    bool grounded;
    public bool Grounded { get { return grounded; } set { grounded = value; } }

    public override void ReadCommand(Command cmd)
    {
        if (cmd as MovementAxisCommand != null)
        {
            MovementAxisCommand movementCommand = cmd as MovementAxisCommand;
            if (movementCommand != null)
            {
                // set horizontal and vertical axes vals according to latest command
                horizontal = movementCommand.HorizontalAxis;
                vertical = movementCommand.VerticalAxis;
            }
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
        // calculate directions - directly forward and 90 degrees to the side
        Vector3 forwardDirection = Vector3.ProjectOnPlane(Camera.main.transform.forward * vertical, Vector3.up);
        Vector3 sideDirection = Vector3.ProjectOnPlane(Camera.main.transform.right * horizontal, Vector3.up);
        // set main direction
        if (vertical != 0)
            // for forward movement
            direction = (forwardDirection + sideDirection).normalized;
        else if (horizontal != 0)
            // for strafing / crabwalking
            direction = sideDirection;
        else
            // no movement
            direction = Vector3.zero;

        // Move player
        transform.position += direction.normalized * currentMoveSpeed * Time.deltaTime;
        // set player rotation
        float targetAngle = Mathf.Atan2((Camera.main.transform.forward + sideDirection).normalized.x, (Camera.main.transform.forward + sideDirection).normalized.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetAngle, 0), currentMoveSpeed * 10 * Time.deltaTime);
        // for jumping
        centreOfMass = transform.position + new Vector3(0, standingPlayerHeight * 0.5f, 0);
    }
    public void RotatePlayer(float rotAngle)
    {
        Vector3 newRot = new Vector3(0, rotAngle, 0);
        transform.rotation = Quaternion.Euler(newRot);
    }
}

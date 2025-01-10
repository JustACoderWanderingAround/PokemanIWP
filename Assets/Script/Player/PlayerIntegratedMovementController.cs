using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntegratedMovementController : UseInputController
{
    /// <summary>
    /// TODO: 
    /// 3. Create keybindings struct/scriptableObj to allow input data to come from 1 central place
    /// </summary>

    [Header("Physics related things")]
    // I forgot what this is
    //[SerializeField]
    //GameObject orientation;

    [SerializeField]
    float currentMoveSpeed = 2.0f;
    [SerializeField]
    float standingPlayerHeight = 1.0f;

    [Header("Tweakable movement related vars")]
    [SerializeField]
    float maxLeanAngle = 15.0f;
    [SerializeField]
    float runningSpeed;
    [SerializeField]
    float defaultSpeed = 2.0f;
    [SerializeField]
    float crouchSpeed;
    [SerializeField]
    float crawlSpeed;
    [SerializeField]
    bool toggleToLean = true;

    [Header("Physics related objects")]
    [SerializeField]
    GameObject leanSlot;
    [SerializeField]
    PlayerLeanController leanController;

    [SerializeField]
    Rigidbody rb;

    // private script only variables
    private Vector3 direction;
    private Vector3 centreOfMass;
    private float horizontal;
    private float vertical;
    private float targetLeanAngle;
    bool canJump = true;
    bool grounded;
    public enum LeanState
    {
        LeanStateL = -1,
        LeanStateNeutral = 0,
        LeanStateR = 1
    }
    private LeanState leanState;
    // temp
    public KeyCode leanLCode = KeyCode.Q;
    public KeyCode leanRCode = KeyCode.E;

    public bool Grounded { get { return grounded; } set { grounded = value; } }

    public override void ReadCommand(Command cmd)
    {
        if (cmd as MovementAxisCommand != null)
        {
            MovementAxisCommand movementCommand = cmd as MovementAxisCommand;
            if (movementCommand != null)
            {
                // set horizontal and vertical axes vals according to latest command
                horizontal = Mathf.Abs(movementCommand.HorizontalAxis) > 0.999 ? movementCommand.HorizontalAxis : 0;
                vertical = movementCommand.VerticalAxis;
                //Debug.Log("ReadCommandMA: H " + horizontal + " V "+ vertical);
            }
        }
        else if (cmd as KeyCodeCommand != null)
        {
            KeyCodeCommand kcc = cmd as KeyCodeCommand;
            if ((toggleToLean && kcc.KeyDown))
            {
                if (kcc.KeycodeNumber == leanLCode)
                {
                    if (leanState == LeanState.LeanStateL)
                    {
                        leanState = LeanState.LeanStateNeutral;
                    }
                    else if (leanState != LeanState.LeanStateL)
                    {
                        leanState = LeanState.LeanStateL;
                    }
                }
                else if (kcc.KeycodeNumber == leanRCode)
                {
                    if (leanState == LeanState.LeanStateR)
                    {
                        leanState = LeanState.LeanStateNeutral;
                    }
                    else if (leanState != LeanState.LeanStateR)
                    {
                        leanState = LeanState.LeanStateR;
                    }
                }

            }
            else if (!toggleToLean)
            {
                if (kcc.KeyHeldDown)
                {
                    if (kcc.KeycodeNumber == leanLCode)
                    {
                        leanState = LeanState.LeanStateL;
                    }
                    else if (kcc.KeycodeNumber == leanRCode)
                    {
                        leanState = LeanState.LeanStateR;
                    }
                }
                else
                {
                    leanState = LeanState.LeanStateNeutral;
                }
            }
            leanController.LeanPlayer(leanState);
        }
        else if (cmd as MouseButtonCommand != null)
        {

        }
    }

    public override void UpdateController(double deltaTime)
    {
        targetLeanAngle = (int)leanState * maxLeanAngle;
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
        //Debug.Log(horizontal + " " + vertical);
        //transform.position += direction.normalized * currentMoveSpeed * Time.deltaTime;
        rb.AddForce(direction.normalized * currentMoveSpeed);
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
    public void LeanPlayer(float leanAngle)
    {
        //leanSlot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, t)
        targetLeanAngle = leanAngle;
    }
    public void SetMovementState(MainPlayerController.MovementState newState)
    {
        switch (newState)
        {
            case MainPlayerController.MovementState.Walk:
                currentMoveSpeed = defaultSpeed;
                break;
            case MainPlayerController.MovementState.Run:
                currentMoveSpeed = runningSpeed;
                break;
        }
    }
}

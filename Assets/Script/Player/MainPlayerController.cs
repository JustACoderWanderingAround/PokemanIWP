using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : UseInputController
{
    [SerializeField]
    GameObject cameraHolderSlot;
    [Header("Movement")]
    [SerializeField]
    PlayerIntegratedMovementController integratedMovementController;
    [SerializeField]
    MainPlayerCameraController mainPlayerCameraController;
    [SerializeField]
    PlayerHandController playerHandController;
    [SerializeField]
    DamagableEntity data;
    [SerializeField]
    SoundGenerator soundGenerator;
    [SerializeField]
    float runFootstepFrequency;
    [SerializeField]
    float walkFootstepFrequency;
    [SerializeField]
    PlayerTerrainStepController ptsc;
    [SerializeField]
    PlayerInteractable pi;

    // Mouse rotation related vars
    private float horizontalOrientation;
    private float verticalOrientation;
    private float mouseX, mouseY;
    private float maxFootstepTimer = -1;
    private float footstepTimer;
    bool isRunning = false;
    private Vector3 direction;
    private Vector2 orientation;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode interactableKey = KeyCode.F;

    public enum MovementState
    {
        Walk,
        Run
    }
    MovementState moveState = MovementState.Walk;
    public override void ReadCommand(Command cmd)
    {
        if (cmd as MouseAxisCommand != null)
        {
            MouseAxisCommand mac = cmd as MouseAxisCommand;
            mouseX = mac.MouseX;
            mouseY = mac.MouseY;
            verticalOrientation -= mouseY;
            verticalOrientation = Mathf.Clamp(verticalOrientation, -90, 90);
            horizontalOrientation += mouseX;
        }
        else if (cmd as MovementAxisCommand != null)
        {
            MovementAxisCommand mac = (cmd as MovementAxisCommand);
            // set horizontal and vertical axes vals according to latest command
            float horizontal = Mathf.Abs(mac.HorizontalAxis) > 0.999 ? mac.HorizontalAxis : 0;
            if (mac.VerticalAxis != 0 || horizontal != 0)
            {
                switch (moveState)
                {
                    case MovementState.Walk:
         
                        maxFootstepTimer = walkFootstepFrequency;
                        break;
                    case MovementState.Run:
         
                        maxFootstepTimer = runFootstepFrequency;
                        break;
                }
            }
            else
            {
                maxFootstepTimer = -1;
            }
        }
        else if (cmd as KeyCodeCommand != null)
        {
            KeyCodeCommand kcc = cmd as KeyCodeCommand;
            Debug.Log("Run:" + kcc.KeyHeldDown.ToString() + kcc.KeyDown.ToString());
            if (kcc.KeycodeNumber == runKey)
            {
                moveState = MovementState.Walk;
                if (kcc.KeyHeldDown)
                {
     
                    moveState = MovementState.Run;
                    maxFootstepTimer = runFootstepFrequency;
                    Debug.Log("Run");
                }
                if (kcc.KeyHeldDown == false && kcc.KeyDown == false)
                {

                    moveState = MovementState.Walk;
                    maxFootstepTimer = walkFootstepFrequency;
                    Debug.Log("Walk");
                }
                Debug.Log("New Movestate: " + moveState.ToString());
                integratedMovementController.SetMovementState(moveState);

            }
            if (kcc.KeycodeNumber == interactableKey)
            {
                if (kcc.KeyDown)
                    pi.Interact();
            }
        }
        else if (cmd as MouseButtonCommand != null)
        {
            // TODO: Controller for items in hands
            playerHandController.ReadCommand(cmd);
        }
        playerHandController.ReadCommand(cmd);
        integratedMovementController.ReadCommand(cmd);
        Debug.Log(moveState.ToString());
    }

    public override void UpdateController(double deltaTime)
    {
        Vector2 headAngle = new Vector2(verticalOrientation, horizontalOrientation);
        integratedMovementController.UpdateController(deltaTime);
        integratedMovementController.RotatePlayer(horizontalOrientation * Time.timeScale);
        mainPlayerCameraController.RotateHeadXAxis(verticalOrientation * Time.timeScale);
        // plan: if maxFootstepTimer is > -1, then wait until footstepTimer is > maxFootstepTimer. when it does, emit 1 sound. 
        if (maxFootstepTimer > -1)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer > maxFootstepTimer)
            {
                soundGenerator.PlaySoundOnce(0);
                ptsc.CheckFootStep();
                footstepTimer = 0;
            }
        }
        else
        {
            footstepTimer = 0;
        }

    }
    private void Start()
    {
        Camera.main.transform.SetParent(cameraHolderSlot.transform);
        Camera.main.transform.localPosition = Vector3.zero;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AttackCol"))
        {

        }
    }
}

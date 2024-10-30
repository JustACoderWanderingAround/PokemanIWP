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

    // Mouse rotation related vars
    private float horizontalOrientation;
    private float verticalOrientation;
    private float mouseX, mouseY;
    bool isRunning = false;
    private Vector3 direction;
    private Vector2 orientation;
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
            Debug.Log("Orientation: \nX: " + horizontalOrientation + "\nY:" + verticalOrientation);
        }
        else if (cmd as MovementAxisCommand != null)
        {
            integratedMovementController.ReadCommand(cmd);
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
        Vector2 headAngle = new Vector2(verticalOrientation, horizontalOrientation);
        // todo: update direction of mainPlayer;
        integratedMovementController.UpdateController(deltaTime);
        integratedMovementController.RotatePlayer(horizontalOrientation);
        //RotateHead(verticalOrientation);
        //mainPlayerCameraController.RotateHeadXAxis(verticalOrientation);
    }
    public void RotateHead(Vector2 headAngle)
    {
        cameraHolderSlot.transform.rotation = Quaternion.Euler(headAngle);
    }
    private void Start()
    {
        Camera.main.transform.SetParent(cameraHolderSlot.transform);
        Camera.main.transform.localPosition = Vector3.zero;
    }
}

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
        }
        else if (cmd as MovementAxisCommand != null)
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
        // todo: update direction of mainPlayer;
    }

    private void Start()
    {
        Camera.main.transform.SetParent(cameraHolderSlot.transform);
        Camera.main.transform.localPosition = Vector3.zero;
    }
}

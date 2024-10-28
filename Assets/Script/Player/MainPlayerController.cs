using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : UseInputController
{
    [SerializeField]
    GameObject cameraHolderSlot;

    public override void ReadCommand(Command cmd)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateController(double deltaTime)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        Camera.main.transform.SetParent(cameraHolderSlot.transform);
        Camera.main.transform.localPosition = Vector3.zero;
    }
}

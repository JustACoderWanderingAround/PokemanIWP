using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private InputController inputController;
    public List<UseInputController> inputControllerList = new List<UseInputController>();
    public List<KeyCode> playerKeybindings;
    MovementAxisCommand move = new MovementAxisCommand(0, 0, 0);
    MouseAxisCommand mouseAxisCommand = new MouseAxisCommand(0, 0, 0);
    MouseButtonCommand mouseButtonCommand = new MouseButtonCommand(0, 0, false);
    KeyCodeCommand keyCodeCommand = new KeyCodeCommand(0, KeyCode.None, false, false);
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputController.TryGetMovementAxisInput(out move))
        {
            foreach (var controller in inputControllerList)
            {
                controller.ReadCommand(move);
            }
        }
        if (inputController.TryGetMouseAxisInput(out mouseAxisCommand))
        {
            foreach (var controller in inputControllerList)
            {
                controller.ReadCommand(mouseAxisCommand);
            }
        }
        if (inputController.TryGetMouseButton(out mouseButtonCommand))
        {
            foreach (var controller in inputControllerList)
            {
                controller.ReadCommand(mouseButtonCommand);
            }
        }
        foreach (KeyCode key in playerKeybindings) {
            if (inputController.TryGetKeycodeInput(key, out keyCodeCommand))
            {
                foreach (var controller in inputControllerList)
                {
                    controller.ReadCommand(keyCodeCommand);
                }
            }
        }
        foreach (var controller in inputControllerList)
        {
            controller.UpdateController(Time.deltaTime);
        }
    }
}

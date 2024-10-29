using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private InputController inputController;
    [SerializeField] private MainPlayerController mainController;
    public List<UseInputController> inputControllerList = new List<UseInputController>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    MovementAxisCommand move = new MovementAxisCommand(0, 0, 0);
    MouseAxisCommand mouseAxisCommand = new MouseAxisCommand(0, 0, 0);
    MouseButtonCommand mouseButtonCommand = new MouseButtonCommand(0, 0, false);
    KeyCodeCommand keyCodeCommand = new KeyCodeCommand(0, KeyCode.None, false, false);

    // Update is called once per frame
    void Update()
    {
        foreach (var controller in inputControllerList)
        {
            if (inputController.TryGetMovementAxisInput(out move))
            {
                controller.ReadCommand(move);
            }
            if (inputController.TryGetMouseAxisInput(out mouseAxisCommand))
            {
                controller.ReadCommand(mouseAxisCommand);
            }
            if (inputController.TryGetMouseButton(out mouseButtonCommand))
            {
                controller.ReadCommand(mouseButtonCommand);
            }
        }

    }
}

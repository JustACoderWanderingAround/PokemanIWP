using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private InputController inputController;
    [SerializeField] private MainPlayerController mainController;
    public List<UseInputController> inputControllers = new List<UseInputController>();
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
        foreach (var controller in inputControllers)
        {
            if (inputController.TryGetMovementAxisInput(out move))
            {

            }
            if (inputController.TryGetMouseAxisInput(out mouseAxisCommand))
            {

            }
            if (inputController.TryGetMouseButton(out mouseButtonCommand))
            {

            }
        }

    }
}

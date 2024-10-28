using UnityEngine;

public abstract class Command
{
    public float Time { get; private set; }
    public Command(float time) => Time = time;
}

public class MovementAxisCommand : Command
{
    public float HorizontalAxis { get; private set; }
    public float VerticalAxis { get; private set; }

    public MovementAxisCommand(float time, float horizontalAxis, float verticalAxis) : base(time)
    {
        HorizontalAxis = horizontalAxis;
        VerticalAxis = verticalAxis;
    }
}

public class MouseAxisCommand : Command
{
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }

    public MouseAxisCommand(float time, float mouseX, float mouseY) : base(time)
    {
        MouseX = mouseX;
        MouseY = mouseY;
    }
}

public class MouseButtonCommand : Command
{
    public int MouseButton { get; private set; }

    public bool MouseDown { get; private set; }

    public MouseButtonCommand(float time, int mouseButton, bool mouseDown) : base(time)
    {
        MouseButton = mouseButton;
        MouseDown = mouseDown;
    }
}

public class KeyCodeCommand : Command
{
    public KeyCode KeycodeNumber { get; private set; }
    public bool KeyDown { get; private set; }
    public bool KeyHeldDown { get; private set; }

    public KeyCodeCommand(float time, KeyCode keycode, bool keyDown, bool keyHeldDown) : base(time)
    {
        KeycodeNumber = keycode;
        KeyDown = keyDown;
        KeyHeldDown = keyHeldDown;
    }
}

public class InputController : MonoBehaviour
{
    private MovementAxisCommand _lastMovementAxisCommand = new MovementAxisCommand(0, 0, 0);
    private MouseAxisCommand _lastMouseAxisCommand = new MouseAxisCommand(0, 0, 0);

    public bool TryGetMovementAxisInput(out MovementAxisCommand movementAxisCommand)
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        bool hasAxisInputChanged = _lastMovementAxisCommand.HorizontalAxis != horizontalAxis || _lastMovementAxisCommand.VerticalAxis != verticalAxis;
        
        if (hasAxisInputChanged)
            _lastMovementAxisCommand = new MovementAxisCommand(Time.time, horizontalAxis, verticalAxis);

        movementAxisCommand = _lastMovementAxisCommand;

        return hasAxisInputChanged;
    }

    public bool TryGetMouseAxisInput(out MouseAxisCommand mouseAxisCommand)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        bool hasAxisInputChanged = _lastMouseAxisCommand.MouseX != mouseX || _lastMouseAxisCommand.MouseY != mouseY;

        if (hasAxisInputChanged)
            _lastMouseAxisCommand = new MouseAxisCommand(Time.time, mouseX, mouseY);

        mouseAxisCommand = _lastMouseAxisCommand;

        return hasAxisInputChanged;
    }

    public bool TryGetMouseButton(out MouseButtonCommand mouseButtonCommand)
    {
        mouseButtonCommand = null;
        if (Input.GetMouseButtonDown(0))
            mouseButtonCommand = new MouseButtonCommand(Time.time, 0, true);
        else if (Input.GetMouseButtonDown(1))
            mouseButtonCommand = new MouseButtonCommand(Time.time, 1, true);
        else if (Input.GetMouseButtonUp(0))
            mouseButtonCommand = new MouseButtonCommand(Time.time, 0, false);
        else if (Input.GetMouseButtonUp(1))
            mouseButtonCommand = new MouseButtonCommand(Time.time, 1, false);
        return mouseButtonCommand != null;
    }
    public bool TryGetKeycodeInput(KeyCode keycode, out KeyCodeCommand keyCodeCommand)
    {
        keyCodeCommand = null;
        if (Input.GetKey(keycode))
        {
            if (Input.GetKeyDown(keycode))
                keyCodeCommand = new KeyCodeCommand(Time.time, keycode, true, true);
            else
            {
                keyCodeCommand = new KeyCodeCommand(Time.time, keycode, false, true);
            }
        }
        else if (Input.GetKeyUp(keycode))
        {
            keyCodeCommand = new KeyCodeCommand(Time.time, keycode, false, false);
        }
        return keyCodeCommand != null;
    }
}
public abstract class UseInputController : MonoBehaviour
{
    public abstract void ReadCommand(Command cmd);
    public abstract void UpdateController(double deltaTime);
    public void LateUpdateController(double deltaTime) { }
}

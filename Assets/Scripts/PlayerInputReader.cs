using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerControlMode
{
    Keyboard,
    Joystick,
    Gyroscope
}

public class PlayerInputReader : MonoBehaviour
{
    private const string ControlModeKey = "PlayerControlMode";

    [Header("Gyroscope")]
    [SerializeField, Min(1f)] private float gyroDegreesForFullInput = 25f;
    [SerializeField, Range(0f, 0.5f)] private float gyroDeadZone = 0.08f;

    private InputAction keyboardMovement;
    private InputAction joystickMovement;
    private Quaternion neutralAttitude;

    public PlayerControlMode ControlMode { get; private set; }

    public Vector2 Movement
    {
        get
        {
            return ControlMode switch
            {
                PlayerControlMode.Keyboard => keyboardMovement.ReadValue<Vector2>(),
                PlayerControlMode.Joystick => joystickMovement.ReadValue<Vector2>(),
                PlayerControlMode.Gyroscope => ReadGyroscope(),
                _ => Vector2.zero
            };
        }
    }

    private void Awake()
    {
        CreateInputActions();

        int savedMode = PlayerPrefs.GetInt(
            ControlModeKey,
            (int)PlayerControlMode.Keyboard);

        SetControlMode((PlayerControlMode)Mathf.Clamp(savedMode, 0, 2));
    }

    private void OnEnable()
    {
        keyboardMovement.Enable();
        joystickMovement.Enable();
        EnableAttitudeSensor();
    }

    private void OnDisable()
    {
        keyboardMovement.Disable();
        joystickMovement.Disable();

        if (AttitudeSensor.current != null)
        {
            InputSystem.DisableDevice(AttitudeSensor.current);
        }
    }

    private void OnDestroy()
    {
        keyboardMovement.Dispose();
        joystickMovement.Dispose();
    }

    public void SetControlMode(PlayerControlMode mode)
    {
        ControlMode = mode;
        PlayerPrefs.SetInt(ControlModeKey, (int)mode);

        if (mode == PlayerControlMode.Gyroscope)
        {
            CalibrateGyroscope();
        }
    }

    public void CalibrateGyroscope()
    {
        EnableAttitudeSensor();

        if (AttitudeSensor.current != null)
        {
            neutralAttitude = AttitudeSensor.current.attitude.ReadValue();
        }
    }

    private void CreateInputActions()
    {
        keyboardMovement = new InputAction(
            "KeyboardMovement",
            InputActionType.Value,
            expectedControlType: "Vector2");

        keyboardMovement.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        joystickMovement = new InputAction(
            "JoystickMovement",
            InputActionType.Value,
            "<Gamepad>/leftStick",
            expectedControlType: "Vector2");
    }

    private void EnableAttitudeSensor()
    {
        if (AttitudeSensor.current != null && !AttitudeSensor.current.enabled)
        {
            InputSystem.EnableDevice(AttitudeSensor.current);
        }
    }

    private Vector2 ReadGyroscope()
    {
        if (AttitudeSensor.current == null)
        {
            return Vector2.zero;
        }

        Quaternion currentAttitude = AttitudeSensor.current.attitude.ReadValue();
        Quaternion relativeAttitude = Quaternion.Inverse(neutralAttitude) * currentAttitude;
        Vector3 angles = relativeAttitude.eulerAngles;

        float pitch = NormalizeAngle(angles.x);
        float roll = NormalizeAngle(angles.z);
        Vector2 input = new Vector2(-roll, pitch) / gyroDegreesForFullInput;

        input = Vector2.ClampMagnitude(input, 1f);
        return input.magnitude < gyroDeadZone ? Vector2.zero : input;
    }

    private static float NormalizeAngle(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }
}

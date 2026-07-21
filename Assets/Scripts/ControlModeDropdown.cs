using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class ControlModeDropdown : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private GameObject joystickRoot;

    private Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
    }

    private void Start()
    {
        if (inputReader == null)
        {
            inputReader = FindFirstObjectByType<PlayerInputReader>();
        }

        if (inputReader == null)
        {
            Debug.LogError("PlayerInputReader не найден в сцене.", this);
            enabled = false;
            return;
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>
        {
            "Клавиатура",
            "Джойстик",
            "Гироскоп"
        });

        dropdown.SetValueWithoutNotify((int)inputReader.ControlMode);
        dropdown.onValueChanged.AddListener(ChangeControlMode);
        UpdateJoystickVisibility(inputReader.ControlMode);
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(ChangeControlMode);
    }

    private void ChangeControlMode(int value)
    {
        PlayerControlMode mode = (PlayerControlMode)value;
        inputReader.SetControlMode(mode);
        UpdateJoystickVisibility(mode);
    }

    private void UpdateJoystickVisibility(PlayerControlMode mode)
    {
        if (joystickRoot != null)
        {
            joystickRoot.SetActive(mode == PlayerControlMode.Joystick);
        }
    }
}

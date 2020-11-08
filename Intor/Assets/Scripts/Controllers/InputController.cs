#if UNITY_EDITOR || UNITY_STANDALONE
#define KEYBOARD
#endif

using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
#if KEYBOARD

    private static Dictionary<KeyCode, Action> _keyCodes;


    private Robot _robot;


    private void Awake()
    {
        Robot.ActiveRobotChanged += OnActiveRobotChanged;

        _keyCodes = new Dictionary<KeyCode, Action>();
    }

    private void Start()
    {
        _robot = Robot.ActiveRobot;
    }

    private void OnDestroy()
    {
        Robot.ActiveRobotChanged -= OnActiveRobotChanged;
    }

    private void Update()
    {
        RobotMovement();
        CheckInput();
    }


    public static void AddKeyAction(KeyCode code, Action action)
    {
        if (_keyCodes.ContainsKey(code)) return;

        _keyCodes.Add(code, action);
    }

    public static void ChangeKeyAction(KeyCode code, Action newAction)
    {
        if (!_keyCodes.ContainsKey(code)) return;

        _keyCodes[code] = newAction;
    }

    public static void ChangeKey(KeyCode oldCode, KeyCode newCode)
    {
        if (!_keyCodes.ContainsKey(oldCode)) return;

        if (_keyCodes.ContainsKey(newCode)) _keyCodes.Remove(newCode);

        _keyCodes.Add(newCode, _keyCodes[oldCode]);
        _keyCodes.Remove(oldCode);
    }


    private void RobotMovement()
    {
        float forward = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (forward != 0 || horizontal != 0)
        {
            _robot.Move(new Vector2(horizontal, forward));

            if (Input.GetKeyDown(KeyCode.LeftShift)) _robot.IsRunning = true;
            if (Input.GetKeyUp(KeyCode.LeftShift)) _robot.IsRunning = false;
        }
        else _robot.IsRunning = false;
    }

    private void CheckInput()
    {
        foreach (var keyAction in _keyCodes)
        {
            if (Input.GetKeyDown(keyAction.Key))
            {
                keyAction.Value?.Invoke();
                break;
            }
        }
    }

    private void OnActiveRobotChanged(Robot robot) => _robot = robot;

#endif
}
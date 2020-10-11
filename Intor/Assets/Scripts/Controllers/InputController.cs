using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
#if UNITY_EDITOR || UNITY_STANDALONE

    public static event Action<KeyCode> KeyPressed;


    private Robot _robot;

    private Dictionary<KeyCode, Action> _keyCodes = new Dictionary<KeyCode, Action>()
    {
        {KeyCode.Space, () => KeyPressed?.Invoke(KeyCode.Space) },
        {KeyCode.Escape, () => KeyPressed?.Invoke(KeyCode.Escape) }
    };


    private void Awake()
    {
        Robot.ActiveRobotChanged += OnActiveRobotChanged;

        _robot = GetComponent<Robot>();
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
    }

    private void CheckInput()
    {
        foreach (var keyCode in _keyCodes)
        {
            if (Input.GetKeyDown(keyCode.Key))
            {
                keyCode.Value?.Invoke();
                break;
            }
        }
    }

    private void OnActiveRobotChanged(Robot robot)
    {
        enabled = _robot.IsActive;
    }

#endif
}
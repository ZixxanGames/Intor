using UnityEngine;

public class PlayerController : MonoBehaviour
{
#if UNITY_EDITOR

    private Robot _robot;


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
        CheckKeys();
    }


    private void CheckKeys()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Robot.ChangeActiveRobot();

        float forward = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (forward != 0 || horizontal != 0)
        {
            _robot.Move(new Vector2(horizontal, forward));

            if (Input.GetKeyDown(KeyCode.LeftShift)) _robot.IsRunning = true;
            if (Input.GetKeyUp(KeyCode.LeftShift)) _robot.IsRunning = false;
        }
    }

    private void OnActiveRobotChanged(Robot robot)
    {
        enabled = _robot.IsActive;
    }

#endif
}

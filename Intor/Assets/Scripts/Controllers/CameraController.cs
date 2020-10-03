using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _followSpeed;

    protected Transform _target;

    private Vector3 _offset = new Vector3(-5f, -9.3f, 5f);


    private void Awake() =>  Robot.ActiveRobotChanged += OnActiveRobotChanged;

    private void LateUpdate() => transform.position = Vector3.Lerp(transform.position, _target.position - _offset, _followSpeed * Time.deltaTime);

    private void OnDestroy() =>  Robot.ActiveRobotChanged -= OnActiveRobotChanged;


    private void OnActiveRobotChanged(Robot robot)
    {
        _target = robot.transform;

        _followSpeed = robot.MovementSpeed * 1.5f;
    }
}

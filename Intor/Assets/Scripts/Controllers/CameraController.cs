using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool IsTransition => _currentMainTransition != null;


    private float _followSpeed;

    private bool _isOnStartPosition;

    private Transform _target;

    private Coroutine _currentMainTransition = null;

    private Dictionary<CameraPositionType, CameraPosition> _camPositions;

    private Vector3 _positionBeforeMove;
    private Vector3 _offset = new Vector3(-5f, -9.3f, 5f);

    private Quaternion _currentTransitionRotation;
    private Quaternion _rotationBeforeMove;

    [SerializeField]
    private CameraPosition[] _temp = null;



    private void Awake()
    {
        Robot.ActiveRobotChanged += OnActiveRobotChanged;

        PauseUI.Paused += OnPause;
        PauseUI.Continued += OnContinue;

        _camPositions = new Dictionary<CameraPositionType, CameraPosition>();
        foreach (var pos in _temp) _camPositions.Add(pos.Type, pos);
        _temp = null;

        _isOnStartPosition = true;
    }

    private void LateUpdate()
    {
        if (!IsTransition)
        {
            transform.position = Vector3.Lerp(transform.position, _target.position - _offset, _followSpeed * Time.deltaTime);
        }
    }

    private void OnDestroy() =>  Robot.ActiveRobotChanged -= OnActiveRobotChanged;


    private void MoveTo(CameraPosition camPosition) => MoveTo(camPosition.Position, camPosition.Rotation);
    private void MoveTo(Vector3 position, Quaternion rotation)
    {
        StopTransition();

        _currentTransitionRotation = rotation;

        _currentMainTransition = StartCoroutine(transform.MoveTo(position, rotation, actionAfterMove: () =>
        {
            _isOnStartPosition = Vector3.Distance(transform.position, _positionBeforeMove) <= 1f;

            _currentMainTransition = null;
        }));
    }

    private void StopTransition()
    {
        if (_currentMainTransition != null)
        {
            StopCoroutine(_currentMainTransition);

            _currentMainTransition = null;
        }
    }


    private void OnPause()
    {
        if (_isOnStartPosition)
        {
            _positionBeforeMove = transform.position;
            _rotationBeforeMove = transform.rotation;

            _isOnStartPosition = false;
        }

        MoveTo(_camPositions[CameraPositionType.Pause]);
    }

    private void OnContinue()
    {
        MoveTo(_positionBeforeMove, _rotationBeforeMove);
    }

    private void OnActiveRobotChanged(Robot robot)
    {
        StopTransition();

        StartCoroutine(transform.MoveTo(_currentTransitionRotation));

        _target = robot.transform;

        _followSpeed = robot.MovementSpeed * 1.5f;
    }
}
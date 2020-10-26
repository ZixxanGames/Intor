using System;
using System.Collections.Generic;
using UnityEngine;
using Scripts.UI;
using Scripts.Extensions;

public class CameraController : MonoBehaviour
{
    public static event Action CameraFocused;


    private bool IsTransition => _currentMainTransition != null;


    private float _followSpeed;

    private bool _isOnStartPosition;

    [SerializeField]
    private Transform plane = null;

    private Transform _target;

    private Coroutine _currentMainTransition = null;

    private Dictionary<CameraPositionType, CameraPosition> _camPositions;

    private Vector3 _positionBeforeMove;
    private Vector3 _offset = new Vector3(-5f, -9.3f, 5f);

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

        plane.position = transform.position;
    }

    private void OnDestroy() =>  Robot.ActiveRobotChanged -= OnActiveRobotChanged;


    private void MoveTo(CameraPosition camPosition) => MoveTo(camPosition.Position, camPosition.Rotation);
    private void MoveTo(Vector3 position, Quaternion rotation)
    {
        StopTransition();

        _currentMainTransition = StartCoroutine(transform.MoveTo(position, rotation, 1.5f, actionAfterMove: () =>
        {
            _isOnStartPosition = Vector3.Distance(transform.position, _positionBeforeMove) <= 1f || Vector3.Distance(transform.position, _target.position - _offset) <= 1f;

            _currentMainTransition = null;

            if (_isOnStartPosition) CameraFocused?.Invoke();
        }));
    }

    private void StopTransition()
    {
        StopAllCoroutines();

        _currentMainTransition = null;
    }


    private void OnShow()
    {

    }

    private void OnHide()
    {

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
        MoveTo(_target.position - _offset, _rotationBeforeMove);
    }

    private void OnActiveRobotChanged(Robot robot)
    {
        _target = robot.transform;

        _followSpeed = robot.MovementSpeed * 1.5f;
    }
}
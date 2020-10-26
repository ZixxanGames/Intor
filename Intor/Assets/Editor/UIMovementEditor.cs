using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Scripts.UI.Pause;
using Scripts.Extensions;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIMovement))]
public class UIMovementEditor : Editor
{
    private UIMovement _uiMovement;

    private Vector3 _startRotation;
    private Vector3 _destinationRotation;


    private void OnSceneGUI()
    {
        _uiMovement = target as UIMovement;

        _startRotation = _uiMovement.startRotation.eulerAngles.Wrap();
        _destinationRotation = _uiMovement.destinationRotation.eulerAngles.Wrap();
    }

    public override void OnInspectorGUI()
    {
        if (!_uiMovement) return;

        Undo.RecordObject(_uiMovement, "_uiMovement");

        if (_uiMovement.startPosition == Vector3.zero || _uiMovement.startRotation.eulerAngles == Vector3.zero)
            _uiMovement.startTransform = (Transform)EditorGUILayout.ObjectField("Start transform", _uiMovement.startTransform, typeof(Transform), true);

        if (_uiMovement.destinationPosition == Vector3.zero || _uiMovement.destinationRotation.eulerAngles == Vector3.zero)
            _uiMovement.destinationTransform = (Transform)EditorGUILayout.ObjectField("Destination transform", _uiMovement.destinationTransform, typeof(Transform), true);

        if (!_uiMovement.startTransform) _uiMovement.startPosition = EditorGUILayout.Vector3Field("Start position", _uiMovement.startPosition);

        _startRotation = EditorGUILayout.Vector3Field("Start rotation", _startRotation);
        _uiMovement.startRotation = Quaternion.Euler(_startRotation);

        if (!_uiMovement.destinationTransform)  _uiMovement.destinationPosition = EditorGUILayout.Vector3Field("Destination position", _uiMovement.destinationPosition);

        _destinationRotation = EditorGUILayout.Vector3Field("Destination rotation", _destinationRotation);
        _uiMovement.destinationRotation = Quaternion.Euler(_destinationRotation);

        if (GUI.changed && !Application.isPlaying) SetDirty(_uiMovement.gameObject);
    }

    private void SetDirty(GameObject obj)
    {
        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(obj.scene);
    }
}
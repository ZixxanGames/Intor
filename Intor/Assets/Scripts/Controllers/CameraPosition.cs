using UnityEngine;

[System.Serializable]
public struct CameraPosition
{
    public Vector3 Position => camPosition.position;

    public Quaternion Rotation => camPosition.rotation;

    [field: SerializeField]
    public CameraPositionType Type { get; private set; }

    [SerializeField]
    private Transform camPosition;
}

public enum CameraPositionType
{
    None,
    Pause,
    Inventory
}
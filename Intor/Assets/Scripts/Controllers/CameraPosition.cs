using UnityEngine;

[System.Serializable]
public class CameraPosition
{
    public Vector3 Position => camPosition.position;

    public Quaternion Rotation => camPosition.rotation;

    [field: SerializeField]
    public CameraPositionType Type { get; private set; }

    [SerializeField]
    private Transform camPosition = null;
}

public enum CameraPositionType
{
    None,
    Pause,
    Inventory
}
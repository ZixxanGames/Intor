using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Vector3 Position => transform.position;

    public Quaternion Rotation => transform.rotation;

    [field: SerializeField]
    public CameraPositionType Type { get; private set; }
}

public enum CameraPositionType
{
    None,
    Pause,
    Inventory
}
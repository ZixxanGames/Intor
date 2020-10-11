using UnityEngine;

public class StaticObjects : MonoBehaviour
{
    public static StaticObjects Instance;

    [field: SerializeField]
    public Transform Plane { get; set; }


    private void Awake() => Instance = !Instance ? this : Instance;
}

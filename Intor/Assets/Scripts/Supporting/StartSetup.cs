using UnityEngine;

public class StartSetup : MonoBehaviour
{
    public static Vector2 ReferenceResolution { get; } = new Vector2(1920, 1080);


    [SerializeField]
    private GameObject[] _objectsToActivate = null;


    private void Awake()
    {
        foreach (var obj in _objectsToActivate) obj.SetActive(true);

        Time.timeScale = 1;
    }
}

using UnityEngine;

public class StartSetup : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _objectsToActivate = null;

    private void Awake()
    {
        foreach (var obj in _objectsToActivate) obj.SetActive(true);

        Time.timeScale = 1;
    }
}

using UnityEngine;

public class LookAtScreenCenter : MonoBehaviour
{
    private Camera _cam;


    private void Start() => _cam = Camera.main;

    private void LateUpdate() => transform.LookAt((transform.position * 2) - _cam.transform.position);
}
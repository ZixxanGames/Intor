using UnityEngine;
using Scripts.Extensions;

namespace Scripts.UI.Pause
{
    public class UIMovement : MonoBehaviour
    {
        [HideInInspector]
        public Transform startTransform;

        [HideInInspector]
        public Transform destinationTransform;

        [HideInInspector]
        public Vector3 startPosition;

        [HideInInspector]
        public Vector3 destinationPosition;

        [HideInInspector]
        public Quaternion startRotation;

        [HideInInspector]
        public Quaternion destinationRotation;

        private void Awake()
        {
            PauseUI.Paused += OnPause;
            PauseUI.Continued += OnContinue;
        }

        private void Start()
        {
            startPosition = startTransform?.position ?? startPosition;
            destinationPosition = destinationTransform?.position ?? destinationPosition;

            transform.position = startPosition;
            transform.rotation = startRotation;
        }

        private void OnDestroy()
        {
            PauseUI.Paused -= OnPause;
            PauseUI.Continued -= OnContinue;
        }


        private void OnPause()
        {
            StopAllCoroutines();

            StartCoroutine(transform.MoveTo(destinationPosition, destinationRotation));
        }

        private void OnContinue()
        {
            StopAllCoroutines();

            StartCoroutine(transform.MoveTo(startPosition, startRotation));
        }
    }
}

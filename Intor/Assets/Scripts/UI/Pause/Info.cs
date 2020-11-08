using UnityEngine;
using Scripts.Extensions;

namespace Scripts.UI.Pause
{
    public class Info : MonoBehaviour
    {
        public Vector2 ShownPosition { get; private set; }
        public Vector2 HiddenPosition { get; private set; }


        private void Awake()
        {
            PauseUI.Paused += OnPause;
            PauseUI.Continued += OnContinue;
        }

        private void Start()
        {
            ShownPosition = transform.position;
            HiddenPosition = new Vector2(transform.position.x + transform.parent.GetComponent<RectTransform>().rect.width, transform.position.y);

            transform.position = HiddenPosition;
        }

        private void OnDestroy()
        {
            PauseUI.Paused -= OnPause;
            PauseUI.Continued -= OnContinue;
        }


        private void OnPause()
        {
            StopAllCoroutines();

            StartCoroutine(transform.MoveTo(ShownPosition));
        }

        private void OnContinue()
        {
            StopAllCoroutines();

            StartCoroutine(transform.MoveTo(HiddenPosition));
        }
    }
}

using UnityEngine;
using Scripts.Extensions;

namespace Scripts.UI.Pause
{
    public class WindowController : MonoBehaviour
    {
        public Vector2 ShownPosition { get; private set; }
        public Vector2 HiddenPosition { get; private set; }


        private void Awake()
        {
            PauseUI.Continued += OnContinue;
        }

        private void Start()
        {
            ShownPosition = transform.position;
            HiddenPosition = new Vector2(transform.parent.position.x, transform.position.y);

            transform.position = HiddenPosition;
        }

        private void OnDestroy() => PauseUI.Continued -= OnContinue;


        private void OnContinue()
        {
            StopAllCoroutines();

            StartCoroutine(transform.MoveTo(HiddenPosition));
        }
    }
}
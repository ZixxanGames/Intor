using UnityEngine;
using Scripts.Extensions;

namespace Scripts.UI.Pause
{
    public class OpenClose : MonoBehaviour
    {
        private bool _isShown;

        [SerializeField]
        private WindowController _window = null;

        [SerializeField]
        private Info _info = null;


        private void Awake() => PauseUI.Continued += OnContinue;

        private void OnDestroy() => PauseUI.Continued -= OnContinue;


        public void ChangeState()
        {
            _isShown = !_isShown;

            StopAllCoroutines();

            if (_isShown) Show();
            else Hide();
        }


        private void Show()
        {
            StopAllCoroutines();

            StartCoroutine(_window.transform.MoveTo(_window.ShownPosition));

            StartCoroutine(transform.MoveTo(Quaternion.Euler(0, 0, 0)));

            StartCoroutine(_info.transform.MoveTo(_info.HiddenPosition));
        }

        private void Hide()
        {
            StopAllCoroutines();

            StartCoroutine(_window.transform.MoveTo(_window.HiddenPosition));

            StartCoroutine(transform.MoveTo(Quaternion.Euler(0, 0, -180)));

            StartCoroutine(_info.transform.MoveTo(_info.ShownPosition));
        }

        private void OnContinue()
        {
            StopAllCoroutines();

            StartCoroutine(transform.MoveTo(Quaternion.Euler(0, 0, -180)));

            _isShown = false;
        }
    }
}

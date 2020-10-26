#if UNITY_EDITOR || UNITY_STANDALONE
#define KEYBOARD
#endif

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class PauseUI : PanelUI
    {
        public static event Action Paused;
        public static event Action Continued;
        public static event Action Hided;


        public static bool IsPause { get; private set; }


        [SerializeField]
        private GameObject _canvasGame = null;

        [SerializeField]
        private Button _pauseContinue = null;


        private void Start()
        {
            _pauseContinue.onClick.RemoveAllListeners();
            _pauseContinue.onClick.AddListener(() => Show());

            gameObject.SetActive(false);

#if KEYBOARD
            InputController.AddKeyAction(KeyCode.Escape, Show);
#endif
        }


        public override void Show()
        {
#if KEYBOARD
            InputController.ChangeKeyAction(KeyCode.Escape, Hide);
#endif
            gameObject.SetActive(true);

            _pauseContinue.transform.SetParent(transform.GetChild(1).transform);
            _pauseContinue.transform.SetSiblingIndex(2);
            _pauseContinue.onClick.RemoveAllListeners();
            _pauseContinue.onClick.AddListener(() => Hide());

            Paused?.Invoke();

            Time.timeScale = 0;

            IsPause = true;

            CameraController.CameraFocused += OnCameraFocused;
        }

        public override void Hide()
        {
#if KEYBOARD
            InputController.ChangeKeyAction(KeyCode.Escape, Show);
#endif
            Continued?.Invoke();

            IsPause = false;

            _pauseContinue.onClick.RemoveAllListeners();
            _pauseContinue.onClick.AddListener(() => Show());
        }

        public void Restart()
        {
            print("Restart");
        }

        public void QuitToMainMenu()
        {
            print("Quit");
        }


        private void OnCameraFocused()
        {
            _pauseContinue.transform.SetParent(_canvasGame.transform);
            _pauseContinue.transform.SetSiblingIndex(1);

            gameObject.SetActive(false);

            Hided?.Invoke();

            Time.timeScale = 1;

            CameraController.CameraFocused -= OnCameraFocused;
        }
    }
}

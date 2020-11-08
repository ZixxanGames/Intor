#if UNITY_EDITOR || UNITY_STANDALONE
#define KEYBOARD
#endif

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Scripts.UI
{
    public class PauseUI : PanelUI
    {
        public static event Action Paused;
        public static event Action Continued;


        public static bool IsPause { get; private set; }


        private Transform _buttonParent;

        [SerializeField]
        private Button _pauseContinue = null;

        private Action _onInventoryOpened = () =>
        {
            Time.timeScale = 0;
            IsPause = true;
        };
        private Action _onInventoryClosed = () =>
        {
            Time.timeScale = 1;
            IsPause = false;
        };


        private void Awake()
        {
            IsPause = false;

            InventoryUI.Showed += _onInventoryOpened;
            InventoryUI.Hided += _onInventoryClosed;
        }

        private void Start()
        {
            _pauseContinue.onClick.RemoveAllListeners();
            _pauseContinue.onClick.AddListener(() => Show());

            _buttonParent = _pauseContinue.transform.root;

            gameObject.SetActive(false);

#if KEYBOARD
            InputController.AddKeyAction(KeyCode.Escape, Show);
#endif
        }

        private void OnDestroy()
        {
            InventoryUI.Showed -= _onInventoryOpened;
            InventoryUI.Hided -= _onInventoryClosed;

            CameraController.CameraFocused -= OnCameraFocused;
        }


        public override void Show()
        {
#if KEYBOARD
            InputController.ChangeKeyAction(KeyCode.Escape, Hide);
#endif
            _pauseContinue.transform.SetParent(transform.GetChild(1).transform);
            _pauseContinue.transform.SetSiblingIndex(2);
            _pauseContinue.onClick.RemoveAllListeners();
            _pauseContinue.onClick.AddListener(() => Hide());

            base.Show();

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
            _pauseContinue.onClick.RemoveAllListeners();
            _pauseContinue.onClick.AddListener(() => Show());

            Continued?.Invoke();

            BeginHide();

            IsPause = false;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitToMainMenu()
        {
            print("Quit");
        }


        private void OnCameraFocused()
        {
            _pauseContinue.transform.SetParent(_buttonParent);
            _pauseContinue.transform.SetSiblingIndex(1);

            base.Hide();

            Time.timeScale = 1;

            CameraController.CameraFocused -= OnCameraFocused;
        }
    }
}

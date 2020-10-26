#if UNITY_EDITOR || UNITY_STANDALONE
#define KEYBOARD
#endif

using System;
using UnityEngine;

namespace Scripts.UI
{
    public class InventoryUI : PanelUI
    {
        public static event Action Showed;
        public static event Action Hided;


        private void Start()
        {
#if KEYBOARD
            InputController.AddKeyAction(KeyCode.I, Show);
#endif

            gameObject.SetActive(false);
        }


        public override void Show()
        {
#if KEYBOARD
            InputController.ChangeKeyAction(KeyCode.I, Hide);
#endif

            gameObject.SetActive(true);

            Showed?.Invoke();

            CameraController.CameraFocused += OnCameraFocused;
        }

        public override void Hide()
        {
#if KEYBOARD
            InputController.ChangeKeyAction(KeyCode.I, Show);
#endif
        }


        private void OnCameraFocused()
        {
            CameraController.CameraFocused -= OnCameraFocused;

            gameObject.SetActive(false);

            Hided?.Invoke();
        }
    }
}
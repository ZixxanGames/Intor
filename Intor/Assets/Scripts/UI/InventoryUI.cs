#if UNITY_EDITOR || UNITY_STANDALONE
#define KEYBOARD
#endif

using Scripts.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class InventoryUI : PanelUI
    {
        public new static event Action Showed;
        public new static event Action Hided;


        [SerializeField]
        private Transform _uiPosition = null;

        [SerializeField]
        private Button _backpackButton = null;

        private Vector3 _startScale;


        private void Start()
        {
#if KEYBOARD
            InputController.AddKeyAction(KeyCode.I, Show);
#endif
            _backpackButton.onClick.RemoveAllListeners();
            _backpackButton.onClick.AddListener(() => Show());

            _startScale = transform.localScale;

            transform.localScale = Vector3.zero;

            gameObject.SetActive(false);
        }


        public override void Show()
        {
#if KEYBOARD
            InputController.ChangeKeyAction(KeyCode.I, Hide);
#else

#endif
            _backpackButton.onClick.RemoveAllListeners();
            _backpackButton.onClick.AddListener(() => Hide());

            transform.position = _uiPosition.position;
            transform.rotation = _uiPosition.rotation;

            base.Show();

            StopAllCoroutines();
            StartCoroutine(transform.ScaleTo(_startScale, 1.6f));

            Showed?.Invoke();

            CameraController.CameraFocused += OnCameraFocused;
        }

        public override void BeginHide()
        {
            base.BeginHide();

            StopAllCoroutines();
            StartCoroutine(transform.ScaleTo(Vector3.zero, 1.6f));
        }

        public override void Hide()
        {
#if KEYBOARD
            InputController.ChangeKeyAction(KeyCode.I, Show);
#else

#endif
            BeginHide();

            _backpackButton.onClick.RemoveAllListeners();
            _backpackButton.onClick.AddListener(() => Show());
        }


        private void OnCameraFocused()
        {
            CameraController.CameraFocused -= OnCameraFocused;

            base.Hide();

            Hided?.Invoke();
        }
    }
}
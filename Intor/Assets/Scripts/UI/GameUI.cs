#if UNITY_EDITOR || UNITY_STANDALONE
#define KEYBOARD
#endif

using UnityEngine;

namespace Scripts.UI
{
    public class GameUI : PanelUI
    {
        private void Awake()
        {
            CameraController.CameraFocused += OnCameraFocused;
            Showed += OnShown;

#if KEYBOARD
            InputController.AddKeyAction(KeyCode.Space, ChangeActiveRobot);
            InputController.AddKeyAction(KeyCode.Mouse0, Attack);
#endif
        }

        private void OnDestroy()
        {
            CameraController.CameraFocused -= OnCameraFocused;
            Showed -= OnShown;
        }


        public void ChangeActiveRobot()
        {
            if (!PauseUI.IsPause) Robot.ChangeActiveRobot();
        }

        public void Attack()
        {

        }

        public void Sprint()
        {
            Robot.ActiveRobot.IsRunning = !Robot.ActiveRobot.IsRunning;
        }


        private void OnCameraFocused() => Show();

        private void OnShown(PanelUI panel)
        {
            if (panel != this) Hide();
        }
    }
}
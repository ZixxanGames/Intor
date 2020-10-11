using UnityEngine;

public class GameUI : MonoBehaviour
{
#if UNITY_EDITOR || UNITY_STANDALONE

    private void Awake()
    {
        InputController.KeyPressed += OnKeyPressed;
    }

    private void OnDestroy()
    {
        InputController.KeyPressed -= OnKeyPressed;
    }

    public void OnKeyPressed(KeyCode code)
    {
        switch (code)
        {
            case KeyCode.Space: 
                if (!PauseUI.IsPause) ChangeActiveRobot();
                break;
        }
    }

#endif

    public void ChangeActiveRobot() => Robot.ChangeActiveRobot();
}

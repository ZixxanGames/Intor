using System;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public static event Action Paused;
    public static event Action Continued;


    public static bool IsPause { get; private set; }


    [SerializeField]
    private GameObject _canvasGame = null;

#if UNITY_EDITOR || UNITY_STANDALONE

    private void Awake()
    {
        InputController.KeyPressed += OnKeyPressed;
    }

    private void OnDestroy()
    {
        InputController.KeyPressed -= OnKeyPressed;
    }


    private void OnKeyPressed(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Escape:
                if (gameObject.activeSelf) Continue();
                else Pause();
                break;
        }
    }

#endif

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Pause()
    {
        _canvasGame.SetActive(false);
        gameObject.SetActive(true);

        Paused?.Invoke();

        Time.timeScale = 0;

        IsPause = true;
    }

    public void Continue()
    {
        _canvasGame.SetActive(true);
        gameObject.SetActive(false);

        Continued?.Invoke();

        Time.timeScale = 1;

        IsPause = false;
    }
}

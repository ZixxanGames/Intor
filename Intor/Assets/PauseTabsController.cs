using UnityEngine;

public class PauseTabsController : MonoBehaviour
{
    private RectTransform rectTransform;


    private void Awake()
    {
        PauseUI.Paused += OnPause;
        PauseUI.Continued += OnContinue;

        rectTransform = transform.parent.GetComponent<RectTransform>();

        transform.position = new Vector2(rectTransform.rect.width + transform.position.x, transform.position.y);
    }

    private void OnDestroy()
    {
        PauseUI.Paused -= OnPause;
        PauseUI.Continued -= OnContinue;
    }


    private void OnPause()
    {
        StartCoroutine(transform.MoveTo(rectTransform.position, 5.5f, 2));
    }

    private void OnContinue()
    {
        transform.position = new Vector2(rectTransform.rect.width + transform.position.x, transform.position.y);
    }
}

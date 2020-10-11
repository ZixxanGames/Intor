using UnityEngine;

public class PauseInfo : MonoBehaviour
{
    //Change
    public static PauseInfo Instance;

    private Vector2 _sectionShownPosition;
    private Vector2 _sectionHidenPosition;


    private void Awake()
    {
        Instance = this;

        _sectionShownPosition = transform.position;
        _sectionHidenPosition = new Vector2(transform.position.x + transform.parent.GetComponent<RectTransform>().rect.width, transform.position.y);
    }


    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(transform.MoveTo(_sectionHidenPosition));
    }

    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(transform.MoveTo(_sectionShownPosition));
    }
}

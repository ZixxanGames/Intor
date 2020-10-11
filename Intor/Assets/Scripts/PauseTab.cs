using UnityEngine;

public class PauseTab : MonoBehaviour
{
    private static PauseTab _activeTab;


    [SerializeField]
    private Transform _section;


    private Vector2 _sectionShownPosition;
    private Vector2 _sectionHidenPosition;


    private void Awake()
    {
        _sectionShownPosition = _section.position;
        _sectionHidenPosition = new Vector2(_section.position.x - _section.parent.GetComponent<RectTransform>().rect.width, _section.position.y);
        _section.position = _sectionHidenPosition;
    }


    public void Section()
    {
        if(_activeTab == this)
        {
            HideSection();

            _activeTab = null;

            PauseInfo.Instance.Show();
        }
        else if (!_activeTab)
        {
            _activeTab = this;

            ShowSection();

            PauseInfo.Instance.Hide();
        }
        else
        {
            _activeTab.HideSection();

            _activeTab = this;

            ShowSection();
        }
    }

    private void ShowSection()
    {
        StopAllCoroutines();
        StartCoroutine(_section.MoveTo(_sectionShownPosition));
    }

    private void HideSection()
    {
        StopAllCoroutines();
        StartCoroutine(_section.MoveTo(_sectionHidenPosition));
    }
}
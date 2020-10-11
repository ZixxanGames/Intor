using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler
{
#if UNITY_EDITOR || UNITY_ANDORID

    public static bool IsPressed { get; private set; }

    private float _radius;

    private Robot _activeRobot;

    [SerializeField]
    private RectTransform _stick = null;

    [SerializeField]
    private Vector2 _referenceResolution = new Vector2(1920, 1080);

    private Vector2 _startPos;
    private Vector2 _direction;
    private Vector2 _resolutionKoefficient;


    private void Awake() => Robot.ActiveRobotChanged += OnActiveRobotChange;

    private void Start()
    {
        RectTransform joystick = GetComponent<RectTransform>();

        float width = joystick.rect.x;
        float height = joystick.rect.y;

        _radius = Mathf.Sqrt((width * width) + (height * height)) / Mathf.Sqrt(2);

        _startPos = (Vector2)transform.localPosition - transform.parent.GetComponent<RectTransform>().rect.min;

        _resolutionKoefficient = _referenceResolution / new Vector2(Screen.width, Screen.height);

        float minKoef = Mathf.Min(_resolutionKoefficient.x, _resolutionKoefficient.y);

        _resolutionKoefficient = new Vector2(minKoef, minKoef);
    }

    private void OnDestroy() => Robot.ActiveRobotChanged -= OnActiveRobotChange;

    private void Update()
    {
        if (_direction != Vector2.zero) _activeRobot.Move(_direction);
    }


    public void OnDrag(PointerEventData eventData)
    {
        _direction = eventData.position * _resolutionKoefficient - _startPos;

        _stick.localPosition = Vector2.ClampMagnitude(_direction, _radius);

        _direction = Vector2.ClampMagnitude(_direction / _radius, 1);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _direction = Vector2.zero;

        _stick.localPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;

        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;

        OnEndDrag(eventData);
    }


    public void OnActiveRobotChange(Robot robot) => _activeRobot = robot;

#endif
}

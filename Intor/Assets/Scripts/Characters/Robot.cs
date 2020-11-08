using Scripts.UI;
using System;
using System.Collections;
using UnityEngine;

public class Robot : Character
{
    private const string ActiveRoboName = "Biba";

    public static event Action<Robot> ActiveRobotChanged;
    public static event Action<Robot> BeforeActiveRobotChanged;

    public static Robot ActiveRobot { get; private set; }
    public static Robot NonActiveRobot { get; private set; }


    public bool IsActive => ActiveRobot == this;


    private bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;

        set
        {
            _isRunning = value;

            if (_isRunning && _currentSprint == null) _currentSprint = StartCoroutine(Sprint());
        }
    }


    [field: SerializeField]
    public string Name { get; private set; }


    [field: SerializeField]
    [field: Tooltip("Acceleration koefficient of robot's run")]
    public float SprintKoefficient { get; private set; }


    [field: SerializeField]
    [field: Tooltip("Energy expenditure while running (energy/sec)")]
    public float SprintCost { get; private set; }


    public FieldOfView FoV { get; private set; }

    public Backpack Backpack { get; private set; }


    [SerializeField]
    private int _startBackpackSize = 3;


    private Transform _sharedContainer;

    private Coroutine _currentSprint;


    private void Awake()
    {
        if (Name == ActiveRoboName)
        {
            ActiveRobot = this;

            PanelUI.Showed += OnShown;
        }
        else
        {
            NonActiveRobot = this;
        }

        Died += OnDie;
        Exhausted += OnExhausted;
    }

    protected override void Start()
    {
        base.Start();

        FoV = transform.GetChild(0).GetComponent<FieldOfView>();

        Backpack = new Backpack()
        {
            TotalSize = _startBackpackSize
        };

        _sharedContainer = ActiveRobot.transform.GetChild(ActiveRobot.transform.childCount - 1);

        if (IsActive)
        {
            ActiveRobotChanged?.Invoke(this);
        }
    }

    private void OnDestroy()
    {
        PanelUI.Showed -= OnShown;

        Died -= OnDie;
        Exhausted -= OnExhausted;
    }


    public static void ChangeActiveRobot()
    {
        BeforeActiveRobotChanged?.Invoke(ActiveRobot);

        PanelUI.Showed -= ActiveRobot.OnShown;

        ActiveRobot.IsRunning = false;


        (ActiveRobot, NonActiveRobot) = (NonActiveRobot, ActiveRobot);


        PanelUI.Showed += ActiveRobot.OnShown;

        ActiveRobot._sharedContainer.SetParent(ActiveRobot.transform);
        ActiveRobot._sharedContainer.localPosition = Vector3.zero;
        ActiveRobot._sharedContainer.localRotation = Quaternion.identity;

        ActiveRobot.Hp = ActiveRobot.Hp;
        ActiveRobot.Energy = ActiveRobot.Energy;

        ActiveRobotChanged?.Invoke(ActiveRobot);
    }


    public void UseItem(Item item)
    {
        switch (item.ItemType)
        {
            case ItemType.Energizer: Energy += 15; break;
            case ItemType.FirstAidKit: Hp += 5; break;
        }
    }

    public void Move(Vector2 direction) => Move(new Vector3(direction.x, 0f, direction.y));
    public void Move(Vector3 direction)
    {
        direction = Vector3.ClampMagnitude(direction, 1);

        direction = StaticObjects.Instance.Plane.TransformDirection(direction);

        Quaternion direction_ = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, direction_, RotationSpeed * Time.deltaTime);

        transform.Translate(Vector3.forward * direction.magnitude * MovementSpeed * Time.deltaTime);
    }


    private IEnumerator Sprint()
    {
        if (Energy <= 0)
        {
            IsRunning = false;

            _currentSprint = null;

            yield break;
        }

        const float Acceleration = 5;
        float step = 0;
        float speedNormal = MovementSpeed;

        float speedAccelerated = MovementSpeed * SprintKoefficient;
        while (MovementSpeed < speedAccelerated && IsRunning)
        {
            MovementSpeed = Mathf.Lerp(speedNormal, speedAccelerated, step);

            step += Acceleration * Time.fixedDeltaTime;

            yield return null;
        }

        step = 0;
        while (IsRunning && Energy > 0)
        {
            step += Time.deltaTime;

            if (step >= 1f / SprintCost)
            {
                step = 0;

                Energy -= Mathf.CeilToInt(Time.deltaTime / (1f / SprintCost));
            }

            yield return null;
        }

        step = 0;
        float speedCurrent = MovementSpeed;
        while (MovementSpeed > speedNormal)
        {
            MovementSpeed = Mathf.Lerp(speedCurrent, speedNormal, step);

            step += Acceleration * Time.deltaTime;

            yield return null;
        }

        IsRunning = false;

        _currentSprint = null;
    }


    private void OnShown(PanelUI panel)
    {
        ShowStats();
    }

    private async void OnExhausted(Character character)
    {
        int penalty = Mathf.CeilToInt((float)EnergyRegenSpeed / 2);

        EnergyRegenSpeed -= penalty;

        float penaltyKoefficient = 10f; // In future maybe used like debuff

        int penaltyTime = (int)StabilizationTime + Mathf.CeilToInt(penaltyKoefficient / (MaxEnergy / (ExhaustedEnergy + 1)));

        await System.Threading.Tasks.Task.Delay(penaltyTime * 1000);

        if (this) EnergyRegenSpeed += penalty;
    }

    private void OnDie(Character _)
    {
        throw new NotImplementedException();
    }
}

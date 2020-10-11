using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [field: SerializeField]
    public Slider Hp { get; private set; }

    [field: SerializeField]
    public Slider Energy { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI HpCount { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI EnergyCount { get; private set; }


    private Coroutine _currentHpHiding;
    private Coroutine _currentEnergyHiding;


    private void Awake()
    {
        Robot.ActiveRobotChanged += OnActiveRobotChanged;
        Robot.BeforeActiveRobotChanged += OnBeforeActiveRobotChanged;
    }

    private void OnDestroy()
    {
        Robot.ActiveRobotChanged -= OnActiveRobotChanged;
        Robot.BeforeActiveRobotChanged -= OnBeforeActiveRobotChanged;
        Robot.ActiveRobot.EnergyChanged -= OnEnergyChange;
        Robot.ActiveRobot.HpChanged -= OnHpChange;
    }


    public void SetActive(BarType bar, bool enabled)
    {
        switch (bar)
        {
            case BarType.Hp: Hp.gameObject.SetActive(enabled); break;
            case BarType.Energy: Energy.gameObject.SetActive(enabled); break;
        }
    }


    private IEnumerator HideHp()
    {
        float timer = 0;

        while (timer < 5)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        SetActive(BarType.Hp, false);

        _currentHpHiding = null;
    }

    private IEnumerator HideEnergy()
    {
        float timer = 0;

        while (timer < 5)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        SetActive(BarType.Energy, false);

        _currentEnergyHiding = null;
    }


    private void OnHpChange(Character character)
    {
        Hp.value = character.Hp;

        HpCount.text = $"{character.Hp} / {character.MaxHp}";

        SetActive(BarType.Hp, true);

        if (_currentHpHiding != null) StopCoroutine(_currentHpHiding);

        _currentHpHiding = StartCoroutine(HideHp());
    }

    private void OnEnergyChange(Character character)
    {
        Energy.value = character.Energy;

        EnergyCount.text = $"{character.Energy} / {character.MaxEnergy}";

        SetActive(BarType.Energy, true);

        if (_currentEnergyHiding != null) StopCoroutine(_currentEnergyHiding);

        _currentEnergyHiding = StartCoroutine(HideEnergy());
    }

    private void OnBeforeActiveRobotChanged(Robot robot)
    {
        robot.HpChanged -= OnHpChange;
        robot.EnergyChanged -= OnEnergyChange;

        if (_currentEnergyHiding != null) StopCoroutine(_currentEnergyHiding);
        if (_currentHpHiding != null) StopCoroutine(_currentHpHiding);
    }

    private void OnActiveRobotChanged(Robot robot)
    {
        robot.HpChanged += OnHpChange;
        robot.EnergyChanged += OnEnergyChange;

        Hp.maxValue = robot.MaxHp;
        Energy.maxValue = robot.MaxEnergy;

        OnHpChange(robot);
        OnEnergyChange(robot);

        transform.SetParent(robot.transform);
        transform.localPosition = Vector3.up;
    }
}

public enum BarType
{
    Hp,
    Energy
}

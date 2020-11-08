using UnityEngine;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    public List<Collider> Overlaps { get; private set; }

    private Robot _robot;


    private void Start()
    {
        _robot = transform.parent.GetComponent<Robot>();

        Overlaps = new List<Collider>();

        GetComponent<SphereCollider>().radius = _robot.ViewRadius;

        if (_robot == Robot.ActiveRobot) On();
        else Off();
    }

    private void OnDestroy() => Off();

    private void OnTriggerEnter(Collider col)
    {
        if (_robot != Robot.ActiveRobot) return;

        ColldierEnter(col);
    }

    private void OnTriggerExit(Collider col)
    {
        if (_robot != Robot.ActiveRobot) return;

        ColldierExit(col);
    }

    private void On()
    {
        Robot.ActiveRobotChanged += OnActiveRobotChanged;

        _robot.Backpack.ItemAdded += OnItemsChanged;
        _robot.Backpack.ItemRemoved += OnItemsChanged;
    }

    private void Off()
    {
        Robot.ActiveRobotChanged -= OnActiveRobotChanged;

        _robot.Backpack.ItemAdded -= OnItemsChanged;
        _robot.Backpack.ItemRemoved -= OnItemsChanged;
    }

    private void ColldierEnter(Collider col)
    {
        if (col.GetComponent<IActiveObject>() is IActiveObject obj)
        {
            if (!Overlaps.Contains(col)) Overlaps.Add(col);

            obj.EnterInteraction(_robot);
        }
    }

    private void ColldierExit(Collider col)
    {
        if (col && col.GetComponent<IActiveObject>() is IActiveObject obj)
        {
            obj.ExitInteraction(_robot);

            Overlaps.Remove(col);
        }
    }

    private void OnActiveRobotChanged(Robot robot)
    {
        var colliders = Overlaps.ToArray();
        foreach (var col in colliders) ColldierExit(col);

        robot.FoV.On();
        Off();

        colliders = Physics.OverlapSphere(robot.transform.position, robot.ViewRadius);
        foreach (var col in colliders) robot.FoV.ColldierEnter(col);
    }

    private void OnItemsChanged(Item item)
    {
        foreach (var col in Overlaps) ColldierEnter(col);
    }
}

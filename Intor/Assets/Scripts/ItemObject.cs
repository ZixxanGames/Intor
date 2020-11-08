using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Scripts.UI;

public class ItemObject : MonoBehaviour, IActiveObject
{
    private static List<ItemObject> _objects;

    [field: SerializeField]
    public Item Item { get; set; }

    [SerializeField]
    private ItemObjectUI _itemUI = null;


    protected virtual void Start()
    {
        if (_objects == null) _objects = new List<ItemObject>();

        if (Item.Stackable && Item.Amount == 0) Item.Amount = 1;

        Item.GameObject = gameObject;

        _itemUI.gameObject.SetActive(false);

        SetItem(Item);

        _objects.Add(this);
    }

    private void OnDestroy() => _objects.Remove(this);


    public static GameObject GetObject(Item item) => _objects.FirstOrDefault((obj) => !obj.gameObject.activeSelf && obj.Item.GetType() == item.GetType())?.gameObject;


    public void SetItem(Item item)
    {
        Item = item;

        _itemUI.Name.text = Item.Name;
        _itemUI.Amount.text = Item.Stackable ? Item.Amount.ToString() : "";
    }


    public void EnterInteraction(Robot robot)
    {
        _itemUI.PickUp.onClick.RemoveAllListeners();
        _itemUI.PickUp.interactable = robot.Backpack.HasEmptySpace || robot.Backpack.HasItem(Item.ItemType, true);
        _itemUI.PickUp.onClick.AddListener(() => Interaction(robot));

        _itemUI.gameObject.SetActive(true);
    }

    public void Interaction(Robot robot)
    {
        robot.Backpack.Add(Item);

        ExitInteraction(robot);

        gameObject.SetActive(false);
    }

    public void ExitInteraction(Robot robot)
    {
        robot.FoV.Overlaps.Remove(GetComponent<Collider>());

        _itemUI.PickUp.onClick.RemoveAllListeners();

        _itemUI.gameObject.SetActive(false);
    }
}

using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IActiveObject
{
    private static List<ItemObject> _Objects;

    [field: SerializeField]
    public Item Item { get; set; }

    [SerializeField]
    private ItemObjectUI _itemUI = null;


    protected virtual void Awake() => LoadData();

    protected virtual void Start()
    {
        if (_Objects == null) _Objects = new List<ItemObject>();

        if (Item.Stackable && Item.Amount == 0) Item.Amount = 1;

        Item.ItemObject = gameObject;

        _itemUI.gameObject.SetActive(false);

        SetItem(Item);

        _Objects.Add(this);
    }

    private void OnDestroy() => _Objects.Remove(this);


    public static GameObject GetObject(Item item) => _Objects.FirstOrDefault((obj) => !obj.gameObject.activeSelf && obj.Item.GetType() == item.GetType()).gameObject;


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


    protected virtual void LoadData()
    {
        //if (DataList.GetObject(Id) == null) return;

        /*var savedObj = DataList.GetObject(Id) as ItemObjectData;

        Item = savedObj.Item;

        transform.position = savedObj.Position;
        gameObject.SetActive(savedObj.Active);

        if (!gameObject.activeSelf) Start();*/
    }
}

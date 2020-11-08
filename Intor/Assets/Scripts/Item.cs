using System;
using UnityEngine;

[Serializable]
public class Item
{
    [SerializeField]
    private int _amount;
    public int Amount
    {
        get => _amount;
        set
        {
            if (!Stackable) throw new ArgumentException("Attempt to change amount of non-stackable item", nameof(Amount));

            _amount = value;
        }
    }

    public bool Stackable => _itemData.Stackable;

    public string Name => _itemData.Name;

    public string Description => _itemData.Description;

    public ItemType ItemType => _itemData.ItemType;

    public GameObject GameObject { get; set; }

    [SerializeField]
    private ItemData _itemData = null;


    public static implicit operator bool(Item item) => item != null;


    public override string ToString() => Name;


    public virtual Item Clone() => new Item()
    {
        GameObject = GameObject,
        _amount = _amount,
        _itemData = _itemData
    };
}

[Flags]
public enum ItemType
{
    None = 0,
    Key = 1,
    Module = 2,
    FirstAidKit = 4,
    Energizer = 8
}

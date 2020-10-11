using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item", order = 1)]
public class Item : ScriptableObject
{
    private int amount;
    public int Amount
    {
        get => amount;
        set
        {
            if (!Stackable) throw new ArgumentException("Attempt to change amount of non-stackable item", nameof(Amount));

            amount = value;
        }
    }

    [field: SerializeField]
    public bool Stackable { get; set; }

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public string Description { get; set; }

    [field: SerializeField]
    public ItemType ItemType { get; set; }

    [field: NonSerialized]
    public GameObject ItemObject { get; set; }


    public static implicit operator bool(Item item) => item != null;


    public override string ToString() => Name;


    public virtual Item Clone()
    {
        Item item = CreateInstance<Item>();

        item.Name = Name;
        item.Description = Description;
        item.ItemType = ItemType;
        item.amount = Amount;
        item.Stackable = Stackable;
        item.ItemObject = ItemObject;

        return item;
    }
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

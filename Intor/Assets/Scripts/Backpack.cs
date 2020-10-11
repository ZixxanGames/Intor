using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Backpack : IEnumerable<Item>
{
    public event Action<Item> ItemAdded;
    public event Action<Item> ItemRemoved;
    public event Action<Item> ItemAmountChanged;


    public int TotalSize { get; set; }

    public int CurrentSize => _items.Count;

    public bool HasEmptySpace => CurrentSize < TotalSize;


    private List<Item> _items;


    public Backpack() => _items = new List<Item>();


    public Item this[int index] => _items[index];
    public Item this[ItemType itemType] => _items.FirstOrDefault((item) => item.ItemType == itemType);
    //public Item this[ModuleType moduleType] => Items.FirstOrDefault((item) => (item is Module module) && module.ModuleType == moduleType);


    public static explicit operator List<Item>(Backpack backpack) => backpack?._items ?? null;


    public List<Item> GetItems(ItemType itemType) => _items.Where((item) => (itemType & item.ItemType) == item.ItemType).ToList();

    public bool HasItem(ItemType itemType, bool mustStackable = false) => _items.Any((item) => (item.ItemType == itemType) && (!mustStackable || item.Stackable));

    //public bool HasModule(ModuleType moduleType) => Items.Any((item) => (item is Module module) && module.ModuleType == moduleType);


    public void Swap(int index1, int index2) => (_items[index1], _items[index2]) = (_items[index2], _items[index1]);

    public void Add(Item item)
    {
        if (!HasEmptySpace && !item.Stackable) return;

        var equalItem = this[item.ItemType];

        if (!item.Stackable) Add();
        else
        {
            if (!equalItem)
            {
                Add();

                ItemAmountChanged?.Invoke(item);
            }
            else Increase();
        }


        void Add()
        {
            _items.Add(item);

            ItemAdded?.Invoke(item);
        }

        void Increase()
        {
            equalItem.Amount += item.Amount;

            ItemAmountChanged?.Invoke(equalItem);
        }
    }

    public void Remove(Item item)
    {
        if (_items.Remove(item)) ItemRemoved?.Invoke(item);
    }

    public void Decrease(Item item)
    {
        var equalItem = this[item.ItemType];

        if (!equalItem) return;

        equalItem.Amount--;

        ItemAmountChanged.Invoke(item);

        if (equalItem.Amount <= 0) Remove(equalItem);
    }


    public IEnumerator<Item> GetEnumerator()
    {
        foreach (var item in _items) yield return item;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item", order = 1)]
public class ItemData : ScriptableObject
{
    [field: SerializeField]
    public bool Stackable { get; set; }

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public string Description { get; set; }

    [field: SerializeField]
    public ItemType ItemType { get; set; }
}
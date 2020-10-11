using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObjectUI : MonoBehaviour
{
    [field: SerializeField]
    public Button PickUp { get; set; }

    [field: SerializeField]
    public TextMeshProUGUI Name { get; set; }

    [field: SerializeField]
    public TextMeshProUGUI Amount { get; set; }
}

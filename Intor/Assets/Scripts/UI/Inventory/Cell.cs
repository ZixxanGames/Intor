using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Inventory
{
    public class Cell : MonoBehaviour
    {
        public event Action<Cell> Selected;


        private Item _item;
        public Item Item
        {
            get => _item;
            set
            {
                _item = value;

                Button.interactable = _item != null;

                Image.raycastTarget = _item != null;

                _name.text = _item?.Name ?? "";
                _amount.text = (_item?.Stackable ?? false) ? _item.Amount.ToString() : "";
            }
        }

        public bool IsSelected { get; private set; }

        public CellDragController CellDragController { get; private set; }

        [field: SerializeField]
        public Image Image { get; private set; }


        [field: SerializeField]
        public Button Button { get; private set; }

        [SerializeField]
        private TextMeshProUGUI _name = null;

        [SerializeField]
        private TextMeshProUGUI _amount = null;


        private void Awake()
        {
            CellDragController = GetComponent<CellDragController>();
        }

        public void ChangeSelectable()
        {
            IsSelected = !IsSelected;

            Selected?.Invoke(this);
        }

        public void SetInfoActive(bool active)
        {
            if (active) Item = Item;
            else
            {
                _name.text = "";
                _amount.text = "";
            }
        }
    }
}
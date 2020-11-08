using System;
using UnityEngine;

namespace Scripts.UI.Inventory
{
    public class ItemsFilter : MonoBehaviour
    {
        public event Action<ItemType> FilterSelected;


        [SerializeField]
        private Color _colorDisabled = Color.white;

        [SerializeField]
        private Color _colorEnabled = Color.cyan;


        [SerializeField]
        private Filter _activeFilter = null;


        private void Start()
        {
            OnPress(_activeFilter);
        }

        public void OnPress(Filter filter)
        {
            _activeFilter?.SetColor(_colorDisabled);

            _activeFilter = filter;

            _activeFilter.SetColor(_colorEnabled);

            FilterSelected?.Invoke(filter.Type);
        }
    }
}
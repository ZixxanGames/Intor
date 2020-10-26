using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pause
{
    public class Tab : MonoBehaviour
    {
        public static event Action<Tab> Pressed;

        [field: SerializeField]
        public GameObject Panel { get; private set; }

        public Color Color
        {
            get => _image.color;
            set => _image.color = value;
        }

        private Image _image;


        private void Awake() => _image = GetComponent<Image>();


        public void OnPressTab() => Pressed?.Invoke(this);
    }
}
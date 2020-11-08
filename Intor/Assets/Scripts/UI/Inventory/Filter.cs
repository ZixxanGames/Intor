using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Inventory
{
    public class Filter : MonoBehaviour
    {
        [field: SerializeField]
        public ItemType Type { get; private set; }


        private Image _image;


        private void Awake() => _image = GetComponent<Image>();


        public void SetColor(Color color) => _image.color = color;
    }
}
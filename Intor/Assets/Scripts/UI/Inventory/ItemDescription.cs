using Scripts.Extensions;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Inventory
{
    public class ItemDescription : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _description = null;


        private Vector2 startPosition;

        private Vector2 endPosition;


        private void Start()
        {
            var koef = Screen.width / StartSetup.ReferenceResolution.x;

            startPosition = transform.localPosition + (Vector3.up * (GetComponent<RectTransform>().rect.height * koef));
            endPosition = transform.localPosition;

            transform.localPosition = startPosition;

            gameObject.SetActive(false);
        }


        public void ShowDescription(string description)
        {
            gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(transform.MoveTo(endPosition, 3f, isLocal: true));

            _description.text = description;
        }

        public void HideDescription()
        {
            if (!gameObject.activeInHierarchy) return;

            StopAllCoroutines();
            StartCoroutine(transform.MoveTo(startPosition, 3f, () => gameObject.SetActive(false), true));
        }
    }
}
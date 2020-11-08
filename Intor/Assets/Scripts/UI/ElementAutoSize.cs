using UnityEngine;

namespace Scripts.UI
{
    public class ElementAutoSize : MonoBehaviour, IResizeable
    {
        private void Awake()
        {
            CalculateSize();
        }

        public void CalculateSize()
        {
            float referenceResolutionKoefficient = StartSetup.ReferenceResolution.x / StartSetup.ReferenceResolution.y;
            float currentResolutionKoefficient = (float)Screen.width / Screen.height;

            var resolutionsKoefficient = referenceResolutionKoefficient / currentResolutionKoefficient;

            var rectTransform = gameObject.GetComponent<RectTransform>();

            rectTransform.sizeDelta *= resolutionsKoefficient;
        }
    }
}
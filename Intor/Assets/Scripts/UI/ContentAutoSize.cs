using Scripts.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class ContentAutoSize : MonoBehaviour, IResizeable
    {
        [SerializeField]
        private bool _isHorizontal = false;

        private RectTransform rectTransform;


        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = Vector2.zero;
        }


        public async void CalculateSize()
        {
            await Task.Delay(1000);

            RectOffset padding;
            Vector2 spacing;

            var iterations = 1;

            if (gameObject.GetComponent<VerticalLayoutGroup>() is VerticalLayoutGroup vlg)
            {
                spacing = new Vector2(0f, vlg.spacing);
                padding = vlg.padding;
            }
            else if (gameObject.GetComponent<HorizontalLayoutGroup>() is HorizontalLayoutGroup hlg)
            {
                _isHorizontal = true;

                spacing = new Vector2(hlg.spacing, 0f);
                padding = hlg.padding;
            }
            else if (gameObject.GetComponent<GridLayoutGroup>() is GridLayoutGroup glg)
            {
                spacing = glg.spacing;
                padding = glg.padding;

                var resolutionKoefficient = StartSetup.ReferenceResolution.x / Screen.width;

                var rowCount = (int)(rectTransform.rect.width * resolutionKoefficient / glg.cellSize.x);
                iterations = Mathf.CeilToInt((float)transform.childCount / rowCount);

                Vector2 sizeDelta;

                if (_isHorizontal) sizeDelta = new Vector2(iterations * glg.cellSize.x + padding.vertical + (iterations - 1) * spacing.x, 0f);
                else sizeDelta = new Vector2(0f, iterations * glg.cellSize.y + padding.vertical + (iterations - 1) * spacing.y);

                rectTransform.sizeDelta = sizeDelta;

                return;
            }
            else
            {
                Debug.LogError($"Gameobject {name} does not have any required component");
                return;
            }

            for (int i = 0; i < iterations; i++)
            {
                var rect = transform.GetChild(i).GetComponent<RectTransform>();

                Vector2 sizeDelta = rect.sizeDelta;

                if (_isHorizontal) sizeDelta = new Vector2(sizeDelta.x + spacing.x, 0f);
                else sizeDelta = new Vector2(0f, sizeDelta.y + spacing.y);

                rectTransform.sizeDelta += new Vector2(sizeDelta.x, sizeDelta.y);
            }

            if (!_isHorizontal) rectTransform.sizeDelta += new Vector2(0f, padding.vertical - spacing.y);
            else rectTransform.sizeDelta += new Vector2(padding.horizontal - spacing.x, 0f);
        }
    }
}
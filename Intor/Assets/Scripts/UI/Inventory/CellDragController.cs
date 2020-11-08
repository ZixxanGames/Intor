using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI.Inventory
{
    public class CellDragController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
    {
        public event Action<Cell, PointerEventData> CellDropped;
        public event Action<Cell, bool> CellPressed;

        private bool _isPressing;

        private float _longPressTime = 0.7f;

        private Cell _cell;


        private void Awake()
        {
            _cell = GetComponent<Cell>();

            _cell.Selected += OnCellSelected;

            enabled = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressing = true;

            StartCoroutine(Timer());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isPressing) StopAllCoroutines();
            else
            {
                CellDropped?.Invoke(_cell, eventData);

                Destroy(_cell.gameObject);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isPressing = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.root as RectTransform, eventData.position, Camera.main, out Vector2 pos);

            _cell.transform.position = transform.root.TransformPoint(pos);
        }


        private IEnumerator Timer()
        {
            var timer = _longPressTime;

            while (_isPressing && timer > 0f)
            {
                yield return null;

                timer -= Time.unscaledDeltaTime;
            }

            _isPressing = false;

            OnDrag(new PointerEventData(default) { position = Input.mousePosition });

            CellPressed?.Invoke(_cell, timer <= 0f);
        }


        private void OnCellSelected(Cell _)
        {
            enabled = _cell.IsSelected;
        }
    }
}
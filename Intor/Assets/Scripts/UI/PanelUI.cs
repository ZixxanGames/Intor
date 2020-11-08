using System;
using UnityEngine;

namespace Scripts.UI
{
    public abstract class PanelUI : MonoBehaviour
    {
        public static event Action<PanelUI> Showed;
        public static event Action<PanelUI> BeganHiding;
        public static event Action<PanelUI> Hided;

        [field: SerializeField]
        public CameraPositionType PositionType { get; private set; }


        public virtual void Show()
        {
            gameObject.SetActive(true);

            Showed?.Invoke(this);
        }

        public virtual void BeginHide()
        {
            BeganHiding?.Invoke(this);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);

            Hided?.Invoke(this);
        }
    }
}
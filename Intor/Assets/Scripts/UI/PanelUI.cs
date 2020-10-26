using System;
using UnityEngine;

namespace Scripts.UI
{
    public abstract class PanelUI : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Scripts.Extensions;

namespace Scripts.UI.Pause
{
    public class TabsController : MonoBehaviour
    {
        private Tab _activeTab;

        private List<Tab> _tabs;

        [SerializeField]
        private Color _inactiveColor = Color.gray;

        [SerializeField]
        private Color _activeColor = Color.white;


        private void Awake()
        {
            Tab.Pressed += OnTabPressed;

            PauseUI.Paused += OnPause;
            PauseUI.Continued += OnContinue;
        }

        private void Start()
        {
            _tabs = transform.GetChilds<Tab>();

            foreach (var tab in _tabs) tab.Color = _inactiveColor;
        }

        private void OnDestroy()
        {
            Tab.Pressed -= OnTabPressed;

            PauseUI.Paused -= OnPause;
            PauseUI.Continued -= OnContinue;
        }



        private void ActivateTab(Tab tab)
        {
            tab.Color = _activeColor;

            tab.Panel.SetActive(true);

            _activeTab = tab;
        }

        private void DeactivateTab(Tab tab)
        {
            tab.Color = _inactiveColor;

            tab.Panel.SetActive(false);

            _activeTab = null;
        }


        private void OnTabPressed(Tab tab)
        {
            if (!_activeTab) ActivateTab(tab);
            else if (_activeTab != tab)
            {
                DeactivateTab(_activeTab);
                ActivateTab(tab);
            }
        }

        private void OnPause()
        {
            ActivateTab(_tabs[0]);
        }

        private void OnContinue()
        {
            DeactivateTab(_activeTab);
        }
    }
}

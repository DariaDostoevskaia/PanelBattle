using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.TopbarPanel
{
    public class TopbarScreenPanel : MonoBehaviour
    {
        public event Action OnOpenSettings;

        [SerializeField] private Button _settingsPopupButton;

        private void Start()
        {
            _settingsPopupButton.onClick.AddListener(() => OnOpenSettings?.Invoke());
        }

        public void Show()
        {
            _settingsPopupButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            OnOpenSettings = null;

            _settingsPopupButton.onClick.RemoveAllListeners();
        }
    }
}
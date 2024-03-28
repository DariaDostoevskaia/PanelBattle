using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.TopbarPanel
{
    public class TopbarScreenPanel : MonoBehaviour
    {
        public event Action OnSettingsButtonClicked;

        [SerializeField] private Button _topbarScreenPanel;

        private void Start()
        {
            _topbarScreenPanel.onClick.AddListener(() => OnSettingsButtonClicked?.Invoke());
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            OnSettingsButtonClicked = null;

            _topbarScreenPanel.onClick.RemoveAllListeners();
        }
    }
}
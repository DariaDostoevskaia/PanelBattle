using LegoBattaleRoyal.Presentation.UI.Base;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.TopbarPanel
{
    public class TopbarScreenPanel : BaseViewUI
    {
        public event Action OnSettingsButtonClicked;

        [SerializeField] private Button _settingsButton;
        [SerializeField] private TextMeshProUGUI _moneyCount;

        private void Start()
        {
            _settingsButton.onClick.AddListener(() => OnSettingsButtonClicked?.Invoke());
        }

        public void SetText(int count)
        {
            _moneyCount.SetText($"{count}");
        }

        private void OnDestroy()
        {
            OnSettingsButtonClicked = null;

            _settingsButton.onClick.RemoveAllListeners();
        }
    }
}
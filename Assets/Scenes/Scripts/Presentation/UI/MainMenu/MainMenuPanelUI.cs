using LegoBattaleRoyal.Presentation.UI.Base;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.MainMenu
{
    public class MainMenuPanelUI : BaseViewUI
    {
        public event Action OnStartGameClicked;

        public event Action RemoveProgressGameClicked;

        public event Action<bool> SettingsRequested;
        public event Action LeaderboardClicked;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _leaderboardGameButton;
        [SerializeField] private Button _resetGameButton;
        [SerializeField] private Button _endGameButton;

        [SerializeField] private Toggle _levelSelectToggle;
        [SerializeField] private Toggle _settingsToggle;

        [SerializeField] private RectTransform _levelSelectPanel;

        private void Start()
        {
            _startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());
            _leaderboardGameButton.onClick.AddListener(() => LeaderboardClicked?.Invoke());
            _resetGameButton.onClick.AddListener(() => RemoveProgressGameClicked?.Invoke());
            _endGameButton.onClick.AddListener(EndGame);

            _settingsToggle.onValueChanged.AddListener((isOn) =>
            {
                SettingsRequested?.Invoke(isOn);
            });

            _levelSelectToggle.onValueChanged.AddListener((isOn) =>
            {
                _levelSelectPanel.gameObject.SetActive(isOn);
            });
        }

        public void SetActiveLevelToggleWithoutNotify()
        {
            _settingsToggle.SetIsOnWithoutNotify(false);

            _levelSelectToggle.SetIsOnWithoutNotify(true);
            _levelSelectPanel.gameObject.SetActive(true);
        }

        private void EndGame()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            OnStartGameClicked = null;
            RemoveProgressGameClicked = null;
            LeaderboardClicked = null;

            _startGameButton.onClick.RemoveAllListeners();
            _leaderboardGameButton.onClick.RemoveAllListeners();
            _resetGameButton.onClick.RemoveAllListeners();
            _endGameButton.onClick.RemoveAllListeners();

            _settingsToggle.onValueChanged.RemoveAllListeners();
            _levelSelectToggle.onValueChanged.RemoveAllListeners();
        }
    }
}
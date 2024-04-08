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

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _leaderboardGameButton;
        [SerializeField] private Button _resetGameButton;
        [SerializeField] private Button _endGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());
            _endGameButton.onClick.AddListener(EndGame);
            _resetGameButton.onClick.AddListener(() => RemoveProgressGameClicked?.Invoke());
        }

        private void EndGame()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            OnStartGameClicked = null;
            RemoveProgressGameClicked = null;

            _startGameButton.onClick.RemoveAllListeners();
            _endGameButton.onClick.RemoveAllListeners();
            _resetGameButton.onClick.RemoveAllListeners();
        }
    }
}
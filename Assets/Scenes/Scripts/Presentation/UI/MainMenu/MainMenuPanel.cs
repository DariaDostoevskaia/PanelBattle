using LegoBattaleRoyal.Presentation.UI.Base;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.MainMenu
{
    public class MainMenuPanel : BaseViewUI
    {
        public event Action OnStartGameClicked;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _endGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());
            _endGameButton.onClick.AddListener(EndGame);
        }

        private void EndGame()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            OnStartGameClicked = null;

            _startGameButton.onClick.RemoveAllListeners();
            _endGameButton.onClick.RemoveAllListeners();
        }
    }
}
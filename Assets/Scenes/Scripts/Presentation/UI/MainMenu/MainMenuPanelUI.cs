using LegoBattaleRoyal.Presentation.UI.Base;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.MainMenu
{
    public class MainMenuPanelUI : MonoBehaviour
    {
        public event Action OnStartGameClicked;

        public event Action RemoveProgressGameClicked;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _endGameButton;

        [SerializeField] private Button _removeProgressGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());
            _endGameButton.onClick.AddListener(EndGame);
            _removeProgressGameButton.onClick.AddListener(() => RemoveProgressGameClicked?.Invoke());
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
            _removeProgressGameButton.onClick.RemoveAllListeners();
        }
    }
}
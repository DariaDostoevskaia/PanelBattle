using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.MainMenu
{
    public class MainMenuPanel : MonoBehaviour
    {
        public event Action OnStartGameClicked;

        public event Action OnResetGameClicked;

        [SerializeField] private TextMeshProUGUI _ribbonText;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _leaderboardGameButton;
        [SerializeField] private Button _resetGameButton;
        [SerializeField] private Button _endGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());
            _resetGameButton.onClick.AddListener(() => OnResetGameClicked?.Invoke());
            _endGameButton.onClick.AddListener(EndGame);

            _ribbonText.SetText("Menu");
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void EndGame()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            OnStartGameClicked = null;
            OnResetGameClicked = null;

            _startGameButton.onClick.RemoveAllListeners();
            _resetGameButton.onClick.RemoveAllListeners();
            _endGameButton.onClick.RemoveAllListeners();
        }
    }
}
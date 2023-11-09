using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.MainMenu
{
    public class MainMenuPanel : MonoBehaviour
    {
        public event Action OnStartGameClicked;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _endGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());
            _endGameButton.onClick.AddListener(EndGame);
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

            _startGameButton.onClick.RemoveAllListeners();
            _endGameButton.onClick.RemoveAllListeners();
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _endGameButton;
        public event Action OnStartGameClicked;

        private void Start()
        {
            _startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());
            //_endGameButton.onClick.AddListener(EndGame);

        }

        private void StartNewGame()
        {
            //_startGameButton.interactable = false;
            //SceneManager.LoadScene(0);
        }

        private void EndGame()
        {
            //_endGameButton.interactable = false;
            //Application.Quit();
        }

        private void OnDestroy()
        {
            //_startGameButton.onClick.RemoveAllListeners();
            //_endGameButton.onClick.RemoveAllListeners();
        }
    }
}

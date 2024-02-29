using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.MainMenu
{
    public class MainMenuPanel : MonoBehaviour
    {
        public event Action OnStartGameClicked;

        public event Action OnLevelSelectClicked;

        public event Action OnSettingsClicked;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _endGameButton;

        [SerializeField] private Toggle _levelSelectToggle;
        [SerializeField] private Toggle _settingsToggle;

        private void Start()
        {
            _startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());
            _endGameButton.onClick.AddListener(EndGame);

            if (_levelSelectToggle.isOn)
                OnLevelSelectClicked?.Invoke();
            if (_settingsToggle.isOn)
                OnSettingsClicked?.Invoke();
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
            OnLevelSelectClicked = null;
            OnSettingsClicked = null;

            _startGameButton.onClick.RemoveAllListeners();
            _endGameButton.onClick.RemoveAllListeners();
        }
    }
}
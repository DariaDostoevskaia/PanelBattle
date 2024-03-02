using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.MainMenu
{
    public class MainMenuPanel : MonoBehaviour
    {
        public event Action OnStartGameClicked;

        public event Action<bool> SettingsRequested;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _endGameButton;

        [SerializeField] private Toggle _levelSelectToggle;
        [SerializeField] private Toggle _settingsToggle;

        [SerializeField] private RectTransform _levelSettingsPanel;

        private void Start()
        {
            _startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());

            _endGameButton.onClick.AddListener(EndGame);

            _settingsToggle.onValueChanged.AddListener((isOn) =>
            {
                SettingsRequested?.Invoke(isOn);
            });

            _levelSelectToggle.onValueChanged.AddListener((isOn) =>
            {
                _levelSettingsPanel.gameObject.SetActive(isOn);
            });
        }

        public void SetActiveLevelToggleWithoutNotify()
        {
            _settingsToggle.SetIsOnWithoutNotify(false);

            _levelSelectToggle.SetIsOnWithoutNotify(true);
            _levelSettingsPanel.gameObject.SetActive(true);
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

            _settingsToggle.onValueChanged.RemoveAllListeners();
            _levelSelectToggle.onValueChanged.RemoveAllListeners();
        }
    }
}
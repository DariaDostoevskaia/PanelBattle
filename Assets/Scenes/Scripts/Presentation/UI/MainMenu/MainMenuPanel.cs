using LegoBattaleRoyal.Presentation.Controllers.Sound;
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

        [SerializeField] private Toggle _levelSelectToggle;
        [SerializeField] private Toggle _settingsToggle;

        [SerializeField] private SettingsPopup _settingsPopup;
        [SerializeField] private GameObject _settingsButtonsPanel;

        [SerializeField] private GameObject _levelSettingsPanel;

        private void Start()
        {
            _settingsPopup.gameObject.SetActive(false);

            _startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());

            _endGameButton.onClick.AddListener(EndGame);

            _levelSelectToggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    ShowLevelSettingsPanel();
                }
            });

            _settingsToggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    ShowSettingsPanel();
                }
            });
        }

        private void ShowSettingsPanel()
        {
            _settingsPopup.gameObject.SetActive(true);
            _settingsButtonsPanel.SetActive(true);

            _levelSettingsPanel.SetActive(false);
        }

        private void ShowLevelSettingsPanel()
        {
            _levelSettingsPanel.SetActive(true);

            _settingsPopup.gameObject.SetActive(false);
            _settingsButtonsPanel.SetActive(false);
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

            _levelSelectToggle.onValueChanged.RemoveAllListeners();
            _settingsToggle.onValueChanged.RemoveAllListeners();
        }
    }
}
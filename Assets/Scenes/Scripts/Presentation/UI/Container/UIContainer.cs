using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.UI.GamePanel;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Container
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private GamePanelUI _gamePanel;
        [SerializeField] private MainMenuPanel _menuPanel;
        [SerializeField] private SettingsPopup _settingsPopup;
        [SerializeField] private AudioClip _buttonsClickAudio;

        private AudioSource _audioSource;

        public GamePanelUI EndGamePopup => _gamePanel;

        public MainMenuPanel MenuView => _menuPanel;

        public SettingsPopup SettingsPopup => _settingsPopup;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _menuPanel.OnStartGameClicked += _audioSource.Play;

            _gamePanel.OnRestartClicked += _audioSource.Play;
            _gamePanel.OnNextLevelClicked += _audioSource.Play;
            _gamePanel.OnExitMainMenuClicked += _audioSource.Play;

            _audioSource.loop = false;
            _audioSource.clip = _buttonsClickAudio;
        }

        public void CloseAll()
        {
            _gamePanel.Close();
            _menuPanel.Close();
        }

        private void OnDestroy()
        {
            //_menuPanel.OnStartGameClicked -= PLayButtonClickedMusic;

            //_gamePanel.OnRestartClicked -= PLayButtonClickedMusic;
            //_gamePanel.OnNextLevelClicked -= PLayButtonClickedMusic;
            //_gamePanel.OnExitMainMenuClicked -= PLayButtonClickedMusic;
        }
    }
}
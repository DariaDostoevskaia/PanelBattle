using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.UI.GamePanel;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using LegoBattaleRoyal.Presentation.UI.TopbarPanel;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Container
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private GamePanelUI _gamePanel;
        [SerializeField] private MainMenuPanel _menuPanel;
        [SerializeField] private SettingsPopup _settingsPopup;
        [SerializeField] private TopbarScreenPanel _topbarScreenPanel;
        [SerializeField] private GameObject _loadingScreen;

        [SerializeField] private AudioClip _buttonsClickAudio;

        private AudioSource _audioSource;

        public GamePanelUI EndGamePopup => _gamePanel;

        public MainMenuPanel MenuView => _menuPanel;

        public SettingsPopup SettingsPopup => _settingsPopup;

        public TopbarScreenPanel TopbarScreenPanel => _topbarScreenPanel;

        public GameObject LoadingScreen => _loadingScreen;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _menuPanel.OnStartGameClicked += _audioSource.Play;
            _menuPanel.OnRemoveProgressGameClicked += _audioSource.Play;

            _gamePanel.OnRestartClicked += _audioSource.Play;
            _gamePanel.OnNextLevelClicked += _audioSource.Play;
            _gamePanel.OnExitMainMenuClicked += _audioSource.Play;

            _settingsPopup.OnOkClicked += _audioSource.Play;
            _settingsPopup.OnHomeClicked += _audioSource.Play;
            _settingsPopup.OnCloseClicked += _audioSource.Play;

            _topbarScreenPanel.OnSettingsButtonClicked += _audioSource.Play;

            _settingsPopup.OnHomeClicked += GoHome;

            _audioSource.loop = false;
            _audioSource.clip = _buttonsClickAudio;
        }

        private void GoHome()
        {
            _gamePanel.Close();
            _menuPanel.Show();
        }

        public void CloseAll()
        {
            _gamePanel.Close();
            _menuPanel.Close();
        }

        private void OnDestroy()
        {
            _menuPanel.OnStartGameClicked -= _audioSource.Play;
            _menuPanel.OnRemoveProgressGameClicked -= _audioSource.Play;

            _gamePanel.OnRestartClicked -= _audioSource.Play;
            _gamePanel.OnNextLevelClicked -= _audioSource.Play;
            _gamePanel.OnExitMainMenuClicked -= _audioSource.Play;

            _settingsPopup.OnOkClicked -= _audioSource.Play;
            _settingsPopup.OnHomeClicked -= _audioSource.Play;
            _settingsPopup.OnCloseClicked -= _audioSource.Play;

            _topbarScreenPanel.OnSettingsButtonClicked -= _audioSource.Play;

            _settingsPopup.OnHomeClicked -= GoHome;
        }
    }
}
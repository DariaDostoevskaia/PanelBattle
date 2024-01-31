using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.UI.GamePanel;
using LegoBattaleRoyal.Presentation.UI.General;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using LegoBattaleRoyal.Presentation.UI.TopbarPanel;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Container
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private GamePanelUI _gamePanel;
        [SerializeField] private MainMenuPanelUI _menuPanel;
        [SerializeField] private GeneralPopup _generalPopup;
        [SerializeField] private SettingsPopup _settingsPopup;
        [SerializeField] private TopbarScreenPanel _topbarScreenPanel;

        [SerializeField] private GameObject _loadingScreen;

        [SerializeField] private AudioClip _buttonsClickAudio;

        private AudioSource _audioSource;

        public GamePanelUI EndGamePopup => _gamePanel;

        public MainMenuPanelUI MenuView => _menuPanel;

        public GeneralPopup GeneralPopup => _generalPopup;

        public SettingsPopup SettingsPopup => _settingsPopup;

        public TopbarScreenPanel TopbarScreenPanel => _topbarScreenPanel;

        public GameObject LoadingScreen => _loadingScreen;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _gamePanel.OnRestartClicked += _audioSource.Play;
            _gamePanel.OnNextLevelClicked += _audioSource.Play;
            _gamePanel.OnExitMainMenuClicked += _audioSource.Play;

            _menuPanel.OnStartGameClicked += _audioSource.Play;

            _generalPopup.OnGeneralButtonClicked += _audioSource.Play;

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
            CloseAll();
            _menuPanel.Show();
        }

        public void CloseAll()
        {
            _gamePanel.Close();
            _menuPanel.Close();

            _generalPopup.Close();
            _settingsPopup.Close();
            _topbarScreenPanel.Close();

            _loadingScreen.SetActive(false);
        }

        private void OnDestroy()
        {
            _gamePanel.OnRestartClicked -= _audioSource.Play;
            _gamePanel.OnNextLevelClicked -= _audioSource.Play;
            _gamePanel.OnExitMainMenuClicked -= _audioSource.Play;

            _menuPanel.OnStartGameClicked -= _audioSource.Play;

            _generalPopup.OnGeneralButtonClicked -= _audioSource.Play;

            _settingsPopup.OnOkClicked -= _audioSource.Play;
            _settingsPopup.OnHomeClicked -= _audioSource.Play;
            _settingsPopup.OnCloseClicked -= _audioSource.Play;

            _topbarScreenPanel.OnSettingsButtonClicked -= _audioSource.Play;

            _settingsPopup.OnHomeClicked -= GoHome;
        }
    }
}
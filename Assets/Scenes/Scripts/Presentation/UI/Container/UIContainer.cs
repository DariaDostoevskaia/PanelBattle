using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.UI.General;
using LegoBattaleRoyal.Presentation.UI.LevelSelect;
using LegoBattaleRoyal.Presentation.UI.LoadingPopup;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using LegoBattaleRoyal.Presentation.UI.TopbarPanel;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Container
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private MainMenuPanelUI _menuPanel;
        [SerializeField] private GeneralPopup _generalPopup;
        [SerializeField] private SettingsPopup _mainMenuSettingsPopup;
        [SerializeField] private SettingsPopup _gameSettingsPopup;
        [SerializeField] private TopbarScreenPanel _topbarScreenPanel;

        [SerializeField] private LevelSelectView _levelSelectView;

        [SerializeField] private GameObject _background;
        [SerializeField] private LoadingScreenUI _loadingScreen;

        [SerializeField] private AudioClip _buttonsClickAudio;

        private AudioSource _audioSource;

        public MainMenuPanelUI MenuView => _menuPanel;

        public GeneralPopup GeneralPopup => _generalPopup;

        public SettingsPopup MainMenuSettingsPopup => _mainMenuSettingsPopup;

        public SettingsPopup GameSettingsPopup => _gameSettingsPopup;

        public TopbarScreenPanel TopbarScreenPanel => _topbarScreenPanel;

        public LevelSelectView LevelSelectView => _levelSelectView;

        public LoadingScreenUI LoadingScreen => _loadingScreen;

        public GameObject Background => _background;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _menuPanel.OnStartGameClicked += _audioSource.Play;
            _menuPanel.RemoveProgressGameClicked += _audioSource.Play;

            _generalPopup.OnGeneralButtonClicked += _audioSource.Play;

            _gameSettingsPopup.OnOkClicked += _audioSource.Play;
            _gameSettingsPopup.OnHomeClicked += _audioSource.Play;
            _gameSettingsPopup.OnCloseClicked += _audioSource.Play;

            _mainMenuSettingsPopup.OnOkClicked += _audioSource.Play;
            _mainMenuSettingsPopup.OnHomeClicked += _audioSource.Play;
            _mainMenuSettingsPopup.OnCloseClicked += _audioSource.Play;

            _topbarScreenPanel.OnSettingsButtonClicked += _audioSource.Play;

            _audioSource.loop = false;
            _audioSource.clip = _buttonsClickAudio;
        }

        private void GoHome()
        {
            CloseAll();
            _background.SetActive(true);
            _menuPanel.Show();
        }

        public void CloseAll()
        {
            _background.SetActive(false);

            _menuPanel.Close();
            _generalPopup.Close();
            _topbarScreenPanel.Close();

            _gameSettingsPopup.Close();
            _mainMenuSettingsPopup.Close();
            _loadingScreen.Close();
        }

        private void OnDestroy()
        {
            _menuPanel.OnStartGameClicked -= _audioSource.Play;
            _menuPanel.RemoveProgressGameClicked -= _audioSource.Play;

            _generalPopup.OnGeneralButtonClicked -= _audioSource.Play;

            _gameSettingsPopup.OnOkClicked -= _audioSource.Play;
            _gameSettingsPopup.OnHomeClicked -= _audioSource.Play;
            _gameSettingsPopup.OnCloseClicked -= _audioSource.Play;

            _mainMenuSettingsPopup.OnOkClicked -= _audioSource.Play;
            _mainMenuSettingsPopup.OnHomeClicked -= _audioSource.Play;
            _mainMenuSettingsPopup.OnCloseClicked -= _audioSource.Play;

            _topbarScreenPanel.OnSettingsButtonClicked -= _audioSource.Play;
        }
    }
}
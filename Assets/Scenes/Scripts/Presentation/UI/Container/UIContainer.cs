using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.UI.General;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using LegoBattaleRoyal.Presentation.UI.TopbarPanel;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Container
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private MainMenuPanelUI _menuPanel;
        [SerializeField] private GeneralPopup _generalPopup;
        [SerializeField] private SettingsPopup _settingsPopup;
        [SerializeField] private TopbarScreenPanel _topbarScreenPanel;
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private GameObject _background;

        [SerializeField] private AudioClip _buttonsClickAudio;

        private AudioSource _audioSource;

        public MainMenuPanelUI MenuView => _menuPanel;

        public GeneralPopup GeneralPopup => _generalPopup;

        public SettingsPopup SettingsPopup => _settingsPopup;

        public TopbarScreenPanel TopbarScreenPanel => _topbarScreenPanel;

        public GameObject LoadingScreen => _loadingScreen;

        public GameObject Background => _background;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _menuPanel.OnStartGameClicked += _audioSource.Play;
            _menuPanel.RemoveProgressGameClicked += _audioSource.Play;

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
            _menuPanel.Close();

            _generalPopup.Close();
            _topbarScreenPanel.Close();
            _settingsPopup.Close();

            _loadingScreen.SetActive(false);
        }

        private void OnDestroy()
        {
            _menuPanel.OnStartGameClicked -= _audioSource.Play;
            _menuPanel.RemoveProgressGameClicked -= _audioSource.Play;

            _generalPopup.OnGeneralButtonClicked -= _audioSource.Play;

            _settingsPopup.OnOkClicked -= _audioSource.Play;
            _settingsPopup.OnHomeClicked -= _audioSource.Play;
            _settingsPopup.OnCloseClicked -= _audioSource.Play;

            _topbarScreenPanel.OnSettingsButtonClicked -= _audioSource.Play;

            _settingsPopup.OnHomeClicked -= GoHome;
        }
    }
}
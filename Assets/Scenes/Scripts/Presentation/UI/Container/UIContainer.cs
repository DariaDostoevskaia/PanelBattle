using LegoBattaleRoyal.Presentation.UI.GamePanel;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Container
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private GamePanelUI _gamePanel;
        [SerializeField] private MainMenuPanel _menuPanel;
        [SerializeField] private AudioClip _buttonsClickAudio;

        private AudioSource _audioSource;

        public GamePanelUI EndGamePopup => _gamePanel;

        public MainMenuPanel MenuView => _menuPanel;

        private void Start()
        {
            _menuPanel.OnStartGameClicked += PLayButtonClickedMusic;

            _gamePanel.OnRestartClicked += PLayButtonClickedMusic;
            _gamePanel.OnNextLevelClicked += PLayButtonClickedMusic;
            _gamePanel.OnExitMainMenuClicked += PLayButtonClickedMusic;

            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = false;
        }

        private void PLayButtonClickedMusic()
        {
            _audioSource.clip = _buttonsClickAudio;
            _audioSource.Play();
        }

        public void CloseAll()
        {
            _gamePanel.Close();
            _menuPanel.Close();
        }

        private void OnDestroy()
        {
            _menuPanel.OnStartGameClicked -= PLayButtonClickedMusic;

            _gamePanel.OnRestartClicked -= PLayButtonClickedMusic;
            _gamePanel.OnNextLevelClicked -= PLayButtonClickedMusic;
            _gamePanel.OnExitMainMenuClicked -= PLayButtonClickedMusic;
        }
    }
}
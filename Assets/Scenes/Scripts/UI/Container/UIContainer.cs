using LegoBattaleRoyal.UI.GamePanel;
using LegoBattaleRoyal.UI.MainMenu;
using UnityEngine;

namespace LegoBattaleRoyal.UI.Container
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private GamePanelUI _gamePanel;
        [SerializeField] private MainMenuPanel _menuPanel;

        public GamePanelUI GamePanel => _gamePanel;

        public MainMenuPanel MenuPanel => _menuPanel;

        private void Start()
        {
            _gamePanel.OnExitMainMenu += ExitToTheMainMenu;
        }

        private void ExitToTheMainMenu()
        {
            _gamePanel.gameObject.SetActive(false);
            _menuPanel.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _gamePanel.OnExitMainMenu -= ExitToTheMainMenu;
        }
    }
}
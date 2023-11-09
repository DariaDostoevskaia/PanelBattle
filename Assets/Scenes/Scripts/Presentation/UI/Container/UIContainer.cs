using LegoBattaleRoyal.Presentation.UI.GamePanel;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Container
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private GamePanelUI _gamePanel;
        [SerializeField] private MainMenuPanel _menuPanel;

        public GamePanelUI EndGamePopup => _gamePanel;

        public MainMenuPanel MenuView => _menuPanel;

        public void CloseAll()
        {
            _gamePanel.Close();
            _menuPanel.Close();
        }
    }
}
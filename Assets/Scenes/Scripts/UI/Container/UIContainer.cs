using LegoBattaleRoyal.UI.GamePanel;
using LegoBattaleRoyal.UI.MainMenu;
using UnityEngine;

namespace LegoBattaleRoyal.UI.Container
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
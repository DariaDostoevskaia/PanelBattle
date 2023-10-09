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
    }
}

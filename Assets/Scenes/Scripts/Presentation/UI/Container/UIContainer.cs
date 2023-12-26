using LegoBattaleRoyal.Presentation.UI.GamePanel;
using LegoBattaleRoyal.Presentation.UI.General;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Container
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private GamePanelUI _gamePanel;
        [SerializeField] private MainMenuPanelUI _menuPanel;
        [SerializeField] private GeneralPopup _generalPopup;

        public GamePanelUI EndGamePopup => _gamePanel;

        public MainMenuPanelUI MenuView => _menuPanel;

        public GeneralPopup GeneralPopup => _generalPopup;

        public void CloseAll()
        {
            _gamePanel.Close();
            _menuPanel.Close();
        }
    }
}
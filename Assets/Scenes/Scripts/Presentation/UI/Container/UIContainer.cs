using LegoBattaleRoyal.Presentation.UI.Ads;
using LegoBattaleRoyal.Presentation.UI.GamePanel;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Container
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private GamePanelUI _gamePanel;
        [SerializeField] private MainMenuPanelUI _menuPanel;
        [SerializeField] private AdsPanelUI _adsPanel;

        public GamePanelUI EndGamePopup => _gamePanel;

        public MainMenuPanelUI MenuView => _menuPanel;

        public AdsPanelUI AdsPanel => _adsPanel;

        public void CloseAll()
        {
            _gamePanel.Close();
            _menuPanel.Close();
            _adsPanel.Close();
        }
    }
}
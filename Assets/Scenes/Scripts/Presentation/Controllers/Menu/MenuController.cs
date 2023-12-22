using LegoBattaleRoyal.Presentation.UI.MainMenu;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Menu
{
    public class MenuController : IDisposable
    {
        public event Action OnGameStarted;

        private readonly MainMenuPanelUI _menuView;

        public MenuController(MainMenuPanelUI menuView)
        {
            _menuView = menuView;
            _menuView.OnStartGameClicked += StartGame;
        }

        private void StartGame()
        {
            OnGameStarted?.Invoke();
        }

        public void ShowMenu()
        {
            _menuView.Show();
        }

        public void CloseMenu()
        {
            _menuView.Close();
        }

        public void Dispose()
        {
            OnGameStarted = null;

            _menuView.OnStartGameClicked -= StartGame;
        }
    }
}
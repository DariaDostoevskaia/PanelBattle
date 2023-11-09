using LegoBattaleRoyal.Presentation.UI.MainMenu;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Menu
{
    public class MenuController : IDisposable
    {
        public event Action OnGameStarted;

        private readonly MainMenuPanel _menuView;

        public MenuController(MainMenuPanel menuView)
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

        public void Dispose()
        {
            OnGameStarted = null;

            _menuView.OnStartGameClicked -= StartGame;
        }
    }
}
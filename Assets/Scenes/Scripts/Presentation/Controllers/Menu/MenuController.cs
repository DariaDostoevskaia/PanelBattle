using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Menu
{
    public class MenuController : IDisposable
    {
        public event Action OnGameStarted;

        private readonly  MainMenuPanelUI _menuView;
        private readonly IAnalyticsProvider _analyticsProvider;

        public MenuController(MainMenuPanel menuView, IAnalyticsProvider analyticsProvider)
        {
            _menuView = menuView;
            _analyticsProvider = analyticsProvider;
            _menuView.OnStartGameClicked += StartGame;
        }

        private void StartGame()
        {
            OnGameStarted?.Invoke();
        }

        public void ShowMenu()
        {
            _menuView.Show();
            _analyticsProvider.SendEvent(AnalyticsEvents.StartMainMenu);
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
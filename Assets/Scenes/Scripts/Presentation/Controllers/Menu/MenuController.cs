using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Infrastructure.Firebase.Analytics;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Menu
{
    public class MenuController : IDisposable
    {
        public event Action OnGameStarted;

        private readonly MainMenuPanelUI _menuView;
        private readonly FirebaseAnalyticsProvider _analyticsProvider;

        public MenuController(MainMenuPanelUI menuView, FirebaseAnalyticsProvider analyticsProvider)
        {
            _menuView = menuView;
            _analyticsProvider = analyticsProvider;
            _menuView.OnStartGameClicked += StartGame;
        }

        private void StartGame()
        {
            OnGameStarted?.Invoke();
            _analyticsProvider.SendEvent(AnalyticsEvents.StartMainMenu);
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
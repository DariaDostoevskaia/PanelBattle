using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Menu
{
    public class MenuController : IDisposable
    {
        public event Action OnGameStarted;

        public event Action OnGameProgressRemoved;

        private readonly MainMenuPanel _menuView;
        private readonly IAnalyticsProvider _analyticsProvider;

        public MenuController(MainMenuPanel menuView, IAnalyticsProvider analyticsProvider)
        {
            _menuView = menuView;
            _analyticsProvider = analyticsProvider;

            _menuView.OnStartGameClicked += StartGame;
            _menuView.OnRemoveProgressGameClicked += RemoveGameProgress;
        }

        private void RemoveGameProgress()
        {
            OnGameProgressRemoved?.Invoke();
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

        public void Dispose()
        {
            OnGameStarted = null;
            OnGameProgressRemoved = null;

            _menuView.OnStartGameClicked -= StartGame;
        }
    }
}
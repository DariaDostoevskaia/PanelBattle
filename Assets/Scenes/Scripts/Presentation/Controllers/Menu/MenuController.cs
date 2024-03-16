using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Menu
{
    public class MenuController : IDisposable
    {
        public event Action OnGameStarted;

        public event Action OnGameProgressRemoved;

        private readonly MainMenuPanelUI _menuView;
        private readonly RefinementPanel _refinementPanel;
        private readonly IAnalyticsProvider _analyticsProvider;

        public MenuController(MainMenuPanelUI menuView, RefinementPanel refinementPanel, IAnalyticsProvider analyticsProvider)
        {
            _menuView = menuView;
            _refinementPanel = refinementPanel;
            _analyticsProvider = analyticsProvider;

            _refinementPanel.Close();

            _menuView.OnStartGameClicked += StartGame;
            _menuView.RemoveProgressGameClicked += ShowRefinementPanel;
            _refinementPanel.RemoveProgressClicked += (value) => RemoveGameProgress(value);
        }

        private void RemoveGameProgress(bool value)
        {
            if (value == true)
                OnGameProgressRemoved?.Invoke();
        }

        private void ShowRefinementPanel()
        {
            _refinementPanel.Show();
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
            OnGameProgressRemoved = null;

            _menuView.OnStartGameClicked -= StartGame;
            _menuView.RemoveProgressGameClicked -= ShowRefinementPanel;
            _refinementPanel.RemoveProgressClicked -= (value) => RemoveGameProgress(value);
        }
    }
}
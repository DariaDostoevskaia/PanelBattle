using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Menu
{
    public class MenuController : IDisposable
    {
        public event Action OnGameStarted;

        private readonly MainMenuPanel _menuView;
        private readonly IAnalyticsProvider _analyticsProvider;
        private readonly SettingsController _menuSettingsController;

        public MenuController(MainMenuPanel menuView, IAnalyticsProvider analyticsProvider, SettingsController menuSettingsController)
        {
            _menuView = menuView;
            _analyticsProvider = analyticsProvider;
            _menuSettingsController = menuSettingsController;

            _menuView.OnStartGameClicked += StartGame;
            _menuSettingsController.Closed += OnSettingsClosed;
            _menuView.SettingsRequested += OnSettingsRequested;
        }

        private void OnSettingsRequested(bool active)
        {
            if (active)
                _menuSettingsController.ShowSettings();
            else
                _menuSettingsController.CloseSettings();
        }

        private void OnSettingsClosed()
        {
            _menuView.SetActiveLevelToggleWithoutNotify();
        }

        private void StartGame()
        {
            OnGameStarted?.Invoke();
        }

        public void ShowMenu()
        {
            _menuView.Show();
            _menuSettingsController.CloseSettings();
            _analyticsProvider.SendEvent(AnalyticsEvents.StartMainMenu);
        }

        public void Dispose()
        {
            OnGameStarted = null;

            _menuView.OnStartGameClicked -= StartGame;
        }
    }
}
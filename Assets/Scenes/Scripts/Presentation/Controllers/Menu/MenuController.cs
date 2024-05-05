using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Presentation.Controllers.Leaderboard;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Menu
{
    public class MenuController : IDisposable
    {
        public event Action OnGameStarted;

        public event Action OnGameProgressRemoved;

        private readonly MainMenuPanelUI _menuView;
        private readonly IAnalyticsProvider _analyticsProvider;
        private readonly SettingsController _menuSettingsController;
        private readonly LeaderboardController _leaderboardController;

        public MenuController(MainMenuPanelUI menuView, IAnalyticsProvider analyticsProvider, SettingsController menuSettingsController,
            LeaderboardController leaderboardController)
        {
            _menuView = menuView;
            _analyticsProvider = analyticsProvider;
            _menuSettingsController = menuSettingsController;
            _leaderboardController = leaderboardController;

            _menuView.OnStartGameClicked += StartGame;
            _menuView.RemoveProgressGameClicked += RemoveGameProgress;
            _menuView.SettingsRequested += OnSettingsRequested;
            _menuSettingsController.Closed += OnSettingsClosed;
            _menuView.LeaderboardClicked += ShowLeaderboard;
        }

        private void ShowLeaderboard()
        {
            _leaderboardController.ShowLeaderboard();
        }

        private void RemoveGameProgress()
        {
            OnGameProgressRemoved?.Invoke();
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

        public void CloseMenu()
        {
            _menuView.Close();
        }

        public void Dispose()
        {
            OnGameStarted = null;
            OnGameProgressRemoved = null;

            _menuView.OnStartGameClicked -= StartGame;
            _menuView.RemoveProgressGameClicked -= RemoveGameProgress;
            _menuView.SettingsRequested -= OnSettingsRequested;
            _menuSettingsController.Closed -= OnSettingsClosed;
            _menuView.LeaderboardClicked -= ShowLeaderboard;
        }
    }
}
using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.UI.MainMenu;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Menu
{
    public class MenuController : IDisposable
    {
        public event Action OnGameStarted;

        private readonly MainMenuPanel _menuView;
        private readonly IAnalyticsProvider _analyticsProvider;
        private readonly SettingsPopup _mainMenuSettingsPopup;

        public MenuController(MainMenuPanel menuView, IAnalyticsProvider analyticsProvider, SettingsPopup mainMenuSettingsPopup)
        {
            _menuView = menuView;
            _analyticsProvider = analyticsProvider;
            _mainMenuSettingsPopup = mainMenuSettingsPopup;

            _menuView.OnStartGameClicked += StartGame;
            _menuView.OnLevelSelectClicked += ShowLevelSelect;
            _menuView.OnSettingsClicked += ShowSettings;
        }

        private void ShowSettings()
        {
            _mainMenuSettingsPopup.gameObject.SetActive(true);
        }

        private void ShowLevelSelect()
        {
            throw new NotImplementedException();
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

            _menuView.OnStartGameClicked -= StartGame;
        }
    }
}
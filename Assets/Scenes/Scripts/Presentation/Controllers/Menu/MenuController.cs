using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Extensions;
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
        private readonly CameraController _cameraController;

        public MenuController(MainMenuPanelUI menuView, IAnalyticsProvider analyticsProvider, CameraController cameraController)
        {
            _menuView = menuView;
            _analyticsProvider = analyticsProvider;
            _cameraController = cameraController;

            _menuView.OnStartGameClicked += StartGame;
            _menuView.RemoveProgressGameClicked += RemoveGameProgress;
        }

        private void RemoveGameProgress()
        {
            OnGameProgressRemoved?.Invoke();
        }

        private void StartGame()
        {
            OnGameStarted?.Invoke();
            _analyticsProvider.SendEvent(AnalyticsEvents.StartMainMenu);
        }

        public void ShowMenu()
        {
            _menuView.Show();
            _cameraController.CloseRaycaster();
        }

        public void CloseMenu()
        {
            _menuView.Close();
            _cameraController.ShowRaycaster();
        }

        public void Dispose()
        {
            OnGameStarted = null;
            OnGameProgressRemoved = null;

            _menuView.OnStartGameClicked -= StartGame;
            _menuView.RemoveProgressGameClicked -= RemoveGameProgress;
        }
    }
}
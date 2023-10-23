using LegoBattaleRoyal.Presentation.Controllers.Menu;
using LegoBattaleRoyal.Presentation.UI.Container;
using System;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class Bootstrap : MonoBehaviour
    {
        private event Action OnDisposed;

        [SerializeField] private GameBootstrap _gameBootstrap;
        [SerializeField] private UIContainer _uiContainer;

        private void Start()
        {
            _uiContainer.CloseAll();
            var menuController = new MenuController(_uiContainer.MenuView);
            menuController.ShowMenu();

            menuController.OnGameStarted += StartGame;

            OnDisposed += () =>
            {
                menuController.OnGameStarted -= StartGame;
                menuController.Dispose();

                _gameBootstrap.OnRestarted -= StartGame;
            };
        }

        private void StartGame()
        {
            _gameBootstrap.Dispose();
            // subscribe again after dispose

            _gameBootstrap.OnRestarted += StartGame;

            _uiContainer.MenuView.Close();

            _gameBootstrap.Configure();
        }

        private void OnDestroy()
        {
            OnDisposed?.Invoke();
            OnDisposed = null;
        }
    }
}
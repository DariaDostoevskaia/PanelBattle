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

            var levelRepository = new LevelRepository();

            var menuController = new MenuController(_uiContainer.MenuView);

            menuController.OnGameStarted += StartGame;

            menuController.ShowMenu();

            OnDisposed += () =>
            {
                menuController.OnGameStarted -= StartGame;
                menuController.Dispose();

                _gameBootstrap.OnRestarted -= StartGame;
            };

            void StartGame()
            {
                _gameBootstrap.Dispose();
                // subscribe again after dispose

                _gameBootstrap.OnRestarted += StartGame;
                _uiContainer.MenuView.Close();

                _gameBootstrap.Configure(levelRepository);
            }
        }

        private void OnDestroy()
        {
            OnDisposed?.Invoke();
            OnDisposed = null;
        }
    }
}
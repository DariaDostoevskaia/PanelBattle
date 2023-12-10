using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.Infrastructure.Repository;
using LegoBattaleRoyal.Presentation.Controllers.Levels;
using LegoBattaleRoyal.Presentation.Controllers.Menu;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.Controllers.Topbar;
using LegoBattaleRoyal.Presentation.UI.Container;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class Bootstrap : MonoBehaviour
    {
        private event Action OnDisposed;

        [SerializeField] private GameBootstrap _gameBootstrap;
        [SerializeField] private GameSettingsSO _gameSettingsSO;
        [SerializeField] private UIContainer _uiContainer;
        [SerializeField] private SoundController _soundController;

        private void Start()
        {
            _uiContainer.CloseAll();

            _soundController.Play(_gameSettingsSO.MainMusic);
            var levelsSO = _gameSettingsSO.Levels;

            var levelRepository = new LevelRepository();
            var saveService = new SaveService();

            var walletController = new WalletController(saveService, _gameSettingsSO);

            var levelController = new LevelController(levelRepository, saveService, walletController);

            levelController.CreateLevels(levelsSO);

            walletController.LoadWalletData();

            var menuController = new MenuController(_uiContainer.MenuView);
            menuController.OnGameStarted += StartGame;
            menuController.ShowMenu();

            var topbarPopup = _uiContainer.TopbarScreenPanel;
            var topbarController = new TopbarController(topbarPopup);

            var settingsPopup = _uiContainer.SettingsPopup;
            var settingsController = new SettingsController(topbarController, settingsPopup, _soundController);
            topbarController.ShowTopbar();

            OnDisposed += () =>
            {
                menuController.OnGameStarted -= StartGame;
                _gameBootstrap.OnRestarted -= StartGame;

                saveService.Dispose();
                levelController.Dispose();
                menuController.Dispose();
                settingsController.Dispose();
                topbarController.Dispose();
            };

            void StartGame()
            {
                levelController.TryBuyLevel(levelRepository.GetCurrentLevel().Price);

                _gameBootstrap.Dispose();
                // subscribe again after dispose

                _gameBootstrap.OnRestarted += StartGame;

                _uiContainer.MenuView.Close();

                _gameBootstrap.Configure(levelRepository, _gameSettingsSO, _uiContainer, walletController);
                _gameBootstrap.Configure(levelRepository, _gameSettingsSO, _uiContainer, _soundController);
            }
        }

        private void OnDestroy()
        {
            OnDisposed?.Invoke();
            OnDisposed = null;
        }
    }
}
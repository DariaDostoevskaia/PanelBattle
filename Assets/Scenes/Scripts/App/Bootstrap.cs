using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.Infrastructure.Repository;
using LegoBattaleRoyal.Infrastructure.Unity.Ads;
using LegoBattaleRoyal.Presentation.Controllers.Levels;
using LegoBattaleRoyal.Presentation.Controllers.Menu;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
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

        private void Start()
        {
            _uiContainer.CloseAll();

            var adsProvider = new UnityAdsProvider();
            adsProvider.InitializeAds();

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

            OnDisposed += () =>
            {
                menuController.OnGameStarted -= StartGame;
                _gameBootstrap.OnRestarted -= StartGame;

                menuController.Dispose();
                saveService.Dispose();
                levelController.Dispose();
            };

            void StartGame()
            {
                levelController.TryBuyLevel(levelRepository.GetCurrentLevel().Price);

                _gameBootstrap.Dispose();
                // subscribe again after dispose

                _gameBootstrap.OnRestarted += StartGame;

                _uiContainer.MenuView.Close();

                _gameBootstrap.Configure(levelRepository, _gameSettingsSO, _uiContainer, walletController);
            }
        }

        private void OnDestroy()
        {
            OnDisposed?.Invoke();
            OnDisposed = null;
        }
    }
}
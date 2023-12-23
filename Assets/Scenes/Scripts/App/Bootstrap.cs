using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.Infrastructure.Repository;
using LegoBattaleRoyal.Infrastructure.Unity.Ads;
using LegoBattaleRoyal.Presentation.Controllers.Levels;
using LegoBattaleRoyal.Presentation.Controllers.Menu;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using LegoBattaleRoyal.Presentation.UI.Container;
using LegoBattaleRoyal.Presentation.UI.General;
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
            var levelController = new LevelController(levelRepository, saveService, walletController, adsProvider);

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
                GeneralPopup generalPopup = null;
                var level = levelRepository.GetCurrentLevel();
                if (!levelController.TryBuyLevel(level.Price))
                {
                    var button = generalPopup.CreateButton("Show Ads");
                    button.onClick.AddListener(() =>
                    {
                        button.interactable = false;
                        ShowRewardedAdsAsync().Forget();
                    });
                    generalPopup.SetText("");
                    generalPopup.SetTitle("");

                    generalPopup.Show();
                    return;
                }

                _gameBootstrap.Dispose();
                // subscribe again after dispose

                _gameBootstrap.OnRestarted += StartGame;
                menuController.CloseMenu();
                _gameBootstrap.Configure(levelRepository, _gameSettingsSO, _uiContainer, walletController);

                async UniTask ShowRewardedAdsAsync()
                {
                    var result = await adsProvider.ShowRewarededAsync();
                    generalPopup.Close();
                    if (!result)
                        return;

                    levelController.EarnCoins(level.Price);

                    StartGame();
                }
            }
        }

        private void OnDestroy()
        {
            OnDisposed?.Invoke();
            OnDisposed = null;
        }
    }
}
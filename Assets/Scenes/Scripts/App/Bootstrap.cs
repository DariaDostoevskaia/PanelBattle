using Cysharp.Threading.Tasks;
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

        private static readonly string NumberInputs = nameof(NumberInputsPlayer);

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
                var level = levelRepository.GetCurrentLevel();
                var generalPopup = _uiContainer.GeneralPopup;

                var isBuyLevel = false;

                if (!levelController.TryBuyLevel(level.Price))
                {
                    var showButton = generalPopup.CreateButton("Show Ads");
                    showButton.onClick.AddListener(() =>
                    {
                        showButton.interactable = false;
                        ShowRewardedAdsAsync().Forget();
                    });

                    isBuyLevel = true;

                    generalPopup.SetTitle("Not enough energy.");
                    generalPopup.SetText("There is not enough energy to buy the next level. Watch an advertisement to replenish energy.");

                    generalPopup.Show();

                    if (!adsProvider.RewardedSuccesShown())
                        StartGame();

                    return;
                }
                else
                {
                    var numberInputs = NumberInputsPlayer();

                    if (numberInputs % 4 == 0
                        && !isBuyLevel)
                        adsProvider.ShowInterstitial();
                }

                generalPopup.Close();
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

        private int NumberInputsPlayer()
        {
            int numberInputs = PlayerPrefs.GetInt(NumberInputs);
            numberInputs++;

            PlayerPrefs.SetInt(NumberInputs, numberInputs);
            PlayerPrefs.Save();
            Debug.Log(NumberInputs + " " + numberInputs);
            return numberInputs;
        }

        private void OnDestroy()
        {
            OnDisposed?.Invoke();
            OnDisposed = null;
        }
    }
}
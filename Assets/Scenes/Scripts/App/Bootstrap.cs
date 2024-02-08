using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.Infrastructure.Firebase.Analytics;
using LegoBattaleRoyal.Infrastructure.Repository;
using LegoBattaleRoyal.Infrastructure.Unity.Ads;
using LegoBattaleRoyal.Presentation.Controllers.Levels;
using LegoBattaleRoyal.Presentation.Controllers.Menu;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.Controllers.Topbar;
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
        [SerializeField] private SoundController _soundController;

        private void Start()
        {
            ConfigureAsync().Forget();
        }

        private async UniTaskVoid ConfigureAsync()
        {
            _uiContainer.LoadingScreen.SetActive(true);

            var analyticsProvider = new FirebaseAnalyticsProvider();
            await analyticsProvider.InitAsync();

            _uiContainer.CloseAll();
            _soundController.Play(_gameSettingsSO.MainMusic);

            var adsProvider = new UnityAdsProvider();
            adsProvider.InitializeAds();

            var levelsSO = _gameSettingsSO.Levels;

            var levelRepository = new LevelRepository();
            var saveService = new SaveService();

            var walletController = new WalletController(saveService, _gameSettingsSO);
            var levelController = new LevelController(levelRepository, saveService, walletController, adsProvider);

            levelController.CreateLevels(levelsSO);
            walletController.LoadWalletData();

            var topbarController = new TopbarController(_uiContainer.TopbarScreenPanel);
            topbarController.ShowTopbar();

            var menuController = new MenuController(_uiContainer.MenuView, analyticsProvider);
            menuController.OnGameStarted += StartGame;
            menuController.ShowMenu();

            var settingsController = new SettingsController(topbarController, _uiContainer.SettingsPopup, _soundController);

            _uiContainer.LoadingScreen.SetActive(false);

            OnDisposed += () =>
            {
                menuController.OnGameStarted -= StartGame;
                _gameBootstrap.OnRestarted -= StartGame;

                saveService.Dispose();
                levelController.Dispose();
                menuController.Dispose();
                settingsController.Dispose();
                topbarController.Dispose();
                adsProvider.Dispose();
            };

            void StartGame()
            {
                var generalPopup = _uiContainer.GeneralPopup;

                var level = levelRepository.GetCurrentLevel();
                var numberEntriesGame = NumberInputsPlayer();

                // TODO

                //var continueButton = generalPopup.CreateButton("Continue");
                //continueButton.onClick.AddListener(() =>
                //{
                //    continueButton.interactable = false;
                //    generalPopup.Close();
                //});

                //var exitButton = generalPopup.CreateButton("Exit");
                //exitButton.onClick.AddListener(() =>
                //{
                //    exitButton.interactable = false;
                //    generalPopup.Close();
                //    menuController.ShowMenu();
                //});

                //generalPopup.SetTitle("Economic and awards.");
                //generalPopup.SetText($"There are {_gameSettingsSO.Money} energy in your wallet. " +
                //    $"The reward for the next level is {level.Reward} energy.");

                //generalPopup.Show();

                //TODO

                if (!levelController.TryBuyLevel(level.Price))
                {
                    var showButton = generalPopup.CreateButton("Show Ads");
                    showButton.onClick.AddListener(() =>
                    {
                        showButton.interactable = false;
                        ShowRewardedAdsAsync().Forget();
                    });

                    generalPopup.SetTitle("Not enough energy.");
                    generalPopup.SetText("There is not enough energy to buy the next level. Watch an advertisement to replenish energy.");

                    generalPopup.Show();

                    return;
                }

                if (numberEntriesGame % 4 == 0)
                    adsProvider.ShowInterstitial();

                generalPopup.Close();
                _gameBootstrap.Dispose();

                // subscribe again after dispose
                _gameBootstrap.OnRestarted += StartGame;

                _uiContainer.LoadingScreen.SetActive(false);
                _uiContainer.MenuView.Close();

                _gameBootstrap.Configure(levelRepository, _gameSettingsSO, _uiContainer, walletController, _soundController);

                async UniTask ShowRewardedAdsAsync()
                {
                    var result = await adsProvider.ShowRewarededAsync();

                    if (!result)
                        return;

                    generalPopup.Close();

                    levelController.EarnCoins(level.Price);
                    numberEntriesGame--;

                    StartGame();
                }

                void ShowEconomicAndAward()
                {
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
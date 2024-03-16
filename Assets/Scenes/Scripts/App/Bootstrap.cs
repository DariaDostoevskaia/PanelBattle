using Cysharp.Threading.Tasks;
using EasyButtons;
using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.ApplicationLayer.Analytics;
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

        private static readonly string NumberInputs = nameof(GetNumberInputsPlayer);

        [SerializeField] private GameBootstrap _gameBootstrap;
        [SerializeField] private GameSettingsSO _gameSettingsSO;
        [SerializeField] private SoundController _soundController;
        [SerializeField] private UIContainer _uiContainer;

        private LevelController _levelController;

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
            var adsProvider = new UnityAdsProvider(analyticsProvider);
            adsProvider.InitializeAds();

            var levelsSO = _gameSettingsSO.Levels;

            var levelRepository = new LevelRepository();
            var saveService = new SaveService();
            var walletController = new WalletController(saveService, _gameSettingsSO);
            var levelController = new LevelController(levelRepository, saveService, walletController, adsProvider);

            levelController.CreateLevels(levelsSO);
            _levelController = levelController;

            walletController.LoadWalletData();

            var topbarController = new TopbarController(_uiContainer.TopbarScreenPanel);
            topbarController.ShowTopbar();

            var menuController = new MenuController(_uiContainer.MenuView, analyticsProvider);
            menuController.OnGameStarted += StartGame;
            menuController.ShowMenu();

            var settingsPopup = _uiContainer.SettingsPopup;
            var settingsController = new SettingsController(topbarController, _uiContainer.SettingsPopup, _soundController);

            topbarController.ShowTopbar();

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
                var entriesGameNumber = GetNumberInputsPlayer();

                if (!levelController.TryBuyLevel(level.Price))
                {
                    analyticsProvider.SendEvent(AnalyticsEvents.NotEnoughCurrency);
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

                if (entriesGameNumber % 4 == 0)
                {
                    analyticsProvider.SendEvent(AnalyticsEvents.NeedInterstitial);
                    adsProvider.ShowInterstitial();
                    Debug.Log("Intrestitial show.");
                }

                generalPopup.Close();
                _gameBootstrap.Dispose();

                // subscribe again after dispose
                _gameBootstrap.OnRestarted += StartGame;

                _uiContainer.LoadingScreen.SetActive(false);
                menuController.CloseMenu();

                analyticsProvider.SendEvent(AnalyticsEvents.StartGameScene);
                _gameBootstrap.Configure(levelRepository, _gameSettingsSO, _uiContainer, walletController, _soundController, analyticsProvider);

                async UniTask ShowRewardedAdsAsync()
                {
                    var result = await adsProvider.ShowRewarededAsync();

                    if (!result)
                        return;

                    generalPopup.Close();

                    levelController.EarnCoins(level.Price);
                    entriesGameNumber--;
                    SaveNumberInputs(entriesGameNumber);

                    StartGame();
                }
            }
        }

        private int GetNumberInputsPlayer()
        {
            int numberInputs = PlayerPrefs.GetInt(NumberInputs);
            numberInputs++;

            SaveNumberInputs(numberInputs);

            Debug.Log(NumberInputs + " " + numberInputs);
            return numberInputs;
        }

        private void SaveNumberInputs(int numberInputs)
        {
            PlayerPrefs.SetInt(NumberInputs, numberInputs);
            PlayerPrefs.Save();
        }

        private void OnDestroy()
        {
            OnDisposed?.Invoke();
            OnDisposed = null;
        }

#if DEBUG

        [Button]
        private void RemoveProgress()
        {
            _levelController.RemoveAllProgress();
        }

#endif
    }
}
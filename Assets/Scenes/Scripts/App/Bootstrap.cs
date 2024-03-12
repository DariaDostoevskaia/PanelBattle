using Cysharp.Threading.Tasks;
using EasyButtons;
using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Infrastructure.Firebase.Analytics;
using LegoBattaleRoyal.Infrastructure.Repository;
using LegoBattaleRoyal.Infrastructure.Unity.Ads;
using LegoBattaleRoyal.Presentation.Controllers.General;
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
            _uiContainer.Background.SetActive(true);
            _uiContainer.TopbarScreenPanel.Show();
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
            var settingsPopup = _uiContainer.SettingsPopup;
            var settingsController = new SettingsController(topbarController, _uiContainer.SettingsPopup, _soundController);

            var generalPopup = _uiContainer.GeneralPopup;
            var generalController = new GeneralController(generalPopup, walletController, levelRepository);

            var menuController = new MenuController(_uiContainer.MenuView);
            menuController.OnGameStarted += StartGame;
            topbarController.ShowTopbar();
            menuController.ShowMenu();

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
                var level = levelRepository.GetCurrentLevel();
                var entriesGameNumber = GetNumberInputsPlayer();

                if (!levelController.TryBuyLevel(level.Price))
                {
                    analyticsProvider.SendEvent(AnalyticsEvents.NotEnoughCurrency);
                    generalController.ShowAdsPopup(() => ShowRewardedAdsAsync().Forget());

                    return;
                }

                //if (entriesGameNumber % 4 == 0)
                //{
                //    adsProvider.ShowInterstitial();
                //    Debug.Log("Intrestitial show.");
                //    analyticsProvider.SendEvent(AnalyticsEvents.NeedInterstitial);
                //}

                generalPopup.Close();
                _gameBootstrap.Dispose();

                // subscribe again after dispose
                _gameBootstrap.OnRestarted += StartGame;

                _uiContainer.LoadingScreen.SetActive(false);
                menuController.CloseMenu();
                _uiContainer.Background.SetActive(false);

                analyticsProvider.SendEvent(AnalyticsEvents.StartGameScene);
                _gameBootstrap.Configure(levelRepository, _gameSettingsSO, walletController, _soundController, analyticsProvider);

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
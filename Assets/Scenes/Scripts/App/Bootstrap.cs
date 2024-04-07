using Cysharp.Threading.Tasks;
using EasyButtons;
using IngameDebugConsole;
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
#if DEBUG
        [SerializeField] private DebugLogManager _debugLogManagerPrefab;
        private DebugLogManager _debugLogManager;
#endif
        private LevelController _levelController;

        private void Start()
        {
            ConfigureAsync().Forget();
        }

        private async UniTaskVoid ConfigureAsync()
        {
#if DEBUG && !UNITY_EDITOR
            if (_debugLogManager == null)
            {
                _debugLogManager = Instantiate(_debugLogManagerPrefab);

                DebugLogConsole.AddCommandInstance(nameof(RemoveProgress), "Remove all progress", nameof(RemoveProgress), this);
                DebugLogConsole.AddCommandInstance(nameof(_gameBootstrap.WinGame), "Win level", nameof(_gameBootstrap.WinGame), _gameBootstrap);
                DebugLogConsole.AddCommandInstance(nameof(_gameBootstrap.LoseLevel), "Lose level", nameof(_gameBootstrap.LoseLevel), _gameBootstrap);
                DebugLogConsole.AddCommandInstance(nameof(_gameBootstrap.InvokeLevel), "Summon a level: ", nameof(_gameBootstrap.InvokeLevel), _gameBootstrap);

                DontDestroyOnLoad(_debugLogManager.gameObject);
            }
#endif
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
            var settingsController = new SettingsController(topbarController, _uiContainer.SettingsPopup, _soundController);
            var generalPopup = _uiContainer.GeneralPopup;
            var generalController = new GeneralController(generalPopup, walletController, levelRepository);

            var menuController = new MenuController(_uiContainer.MenuView, analyticsProvider);
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

                if (entriesGameNumber % 4 == 0)
                {
                    analyticsProvider.SendEvent(AnalyticsEvents.NeedInterstitial);
                    adsProvider.ShowInterstitial();
                    Debug.Log("Intrestitial show.");
                    entriesGameNumber = 0;
                }
                else
                {
                    entriesGameNumber++;
                }
                SaveNumberInputs(entriesGameNumber);

                generalPopup.Close();
                _gameBootstrap.Dispose();

                // subscribe again after dispose
                _gameBootstrap.OnRestarted += StartGame;

                _uiContainer.LoadingScreen.SetActive(false);
                _uiContainer.Background.SetActive(false);
                menuController.CloseMenu();

                analyticsProvider.SendEvent(AnalyticsEvents.StartGameScene);
                _gameBootstrap.Configure(levelRepository, _gameSettingsSO, walletController, _soundController, analyticsProvider);

                async UniTask ShowRewardedAdsAsync()
                {
                    var result = await adsProvider.ShowRewarededAsync();

                    if (!result)
                        return;

                    generalPopup.Close();

                    walletController.EarnCoins(level.Price);
                    entriesGameNumber = Math.Max(0, --entriesGameNumber);
                    SaveNumberInputs(entriesGameNumber);

                    StartGame();
                }
            }
        }

        private int GetNumberInputsPlayer()
        {
            int numberInputs = PlayerPrefs.GetInt(NumberInputs);

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
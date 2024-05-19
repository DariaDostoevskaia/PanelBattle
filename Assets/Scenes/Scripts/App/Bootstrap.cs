using Cinemachine;
using Cysharp.Threading.Tasks;
using EasyButtons;
using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Infrastructure.Firebase.Analytics;
using LegoBattaleRoyal.Infrastructure.Repository;
using LegoBattaleRoyal.Infrastructure.Unity.Ads;
using LegoBattaleRoyal.Infrastructure.Unity.Authentification;
using LegoBattaleRoyal.Infrastructure.Unity.Leaderboard;
using LegoBattaleRoyal.Presentation.Controllers.General;
using LegoBattaleRoyal.Presentation.Controllers.Leaderboard;
using LegoBattaleRoyal.Presentation.Controllers.Levels;
using LegoBattaleRoyal.Presentation.Controllers.LevelSelect;
using LegoBattaleRoyal.Presentation.Controllers.Loading;
using LegoBattaleRoyal.Presentation.Controllers.Menu;
using LegoBattaleRoyal.Presentation.Controllers.Settings;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.Controllers.Topbar;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using LegoBattaleRoyal.Presentation.UI.Container;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
#if DEBUG && ! UNITY_EDITOR
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

                DontDestroyOnLoad(_debugLogManager.gameObject);
            }
#endif
            _uiContainer.CloseAll();
            _uiContainer.Background.SetActive(true);

            var loadingController = new LoadingController(_uiContainer.LoadingScreen);
            loadingController.ResetLoadingPopup();
            loadingController.ShowLoadingPopup();
            IProgress<int> progress = new Progress<int>((progressValue) =>
            {
                float percent = (float)progressValue / 100;
                loadingController.SetProgress(percent);
            });

            var analyticsProvider = new FirebaseAnalyticsProvider();
            progress.Report(35);
            await analyticsProvider.InitAsync();

            _soundController.Play(_gameSettingsSO.MainMusic);
            var adsProvider = new UnityAdsProvider(analyticsProvider);
            adsProvider.InitializeAds();

            var levelRepository = new LevelRepository();
            var saveService = new SaveService();
            var walletController = new WalletController(saveService, _gameSettingsSO);
            var levelController = new LevelController(levelRepository, saveService, walletController);

            var levelsSO = _gameSettingsSO.Levels;
            levelController.CreateLevels(levelsSO);
            _levelController = levelController;
            walletController.LoadWalletData();

            var camera = Camera.main;
            var raycaster = camera.GetComponent<PhysicsRaycaster>();
            var cinemachine = camera.GetComponent<CinemachineBrain>();
            var cameraController = new CameraController(raycaster, cinemachine);
            cameraController.ShowRaycaster();

            var authentificationController = new AuthentificationController();
            progress.Report(65);
            await authentificationController.SignInAsync();

            var gameSettingsController = new SettingsController(_uiContainer.GameSettingsPopup, _soundController, loadingController, cameraController);
            var topbarController = new TopbarController(_uiContainer.TopbarScreenPanel, gameSettingsController, walletController);

            var generalPopup = _uiContainer.GeneralPopup;
            var generalController = new GeneralController(_uiContainer.GeneralPopup, walletController, levelRepository, cameraController);

            var mainSettingsController = new SettingsController(_uiContainer.MainMenuSettingsPopup, _soundController, loadingController, cameraController);

            var levelSelectController = new LevelSelectController(_uiContainer.LevelSelectView, levelRepository, _gameSettingsSO);
            levelSelectController.ShowLevelSelect();

            var leaderboardProvider = new UnityLeaderboardProvider();
            var leaderboardController = new LeaderboardController(leaderboardProvider);
            progress.Report(100);
            await leaderboardController.InitAsync();

            var menuController = new MenuController(_uiContainer.MenuView, analyticsProvider, mainSettingsController, cameraController, leaderboardController);
            menuController.OnGameStarted += StartGame;
            menuController.OnGameProgressRemoved += RemoveProgress;

            levelSelectController.OnLevelInvoked += StartGame;

            menuController.ShowMenu();

            await loadingController.WaitAsync();
            loadingController.CloseLoadingPopup();

            OnDisposed += () =>
            {
                menuController.OnGameStarted -= StartGame;
                menuController.OnGameProgressRemoved -= RemoveProgress;
                _gameBootstrap.OnRestarted -= StartGame;

                saveService.Dispose();
                levelController.Dispose();
                menuController.Dispose();

                mainSettingsController.Dispose();
                gameSettingsController.Dispose();

                topbarController.Dispose();
                walletController.Dispose();
                adsProvider.Dispose();
                levelSelectController.Dispose();
                generalController.Dispose();
            };

            void StartGame()
            {
                var level = levelRepository.GetCurrentLevel();
                var entriesGameNumber = GetNumberInputsPlayer();

                if (!levelController.TryBuyLevel(level.Price))
                {
                    analyticsProvider.SendEvent(AnalyticsEvents.NotEnoughCurrency);
                    generalController.ShowAdsPopup(ShowRewardedAdsAsync);

                    return;
                }

                entriesGameNumber++;

                if (entriesGameNumber % 4 == 0)
                {
                    analyticsProvider.SendEvent(AnalyticsEvents.NeedInterstitial);
                    adsProvider.ShowInterstitial();
                    Debug.Log("Intrestitial show.");
                    entriesGameNumber = 0;
                }
                SaveNumberInputs(entriesGameNumber);

                generalPopup.Close();
                _gameBootstrap.Dispose();

                // subscribe again after dispose
                _gameBootstrap.OnRestarted += StartGame;

                _uiContainer.Background.SetActive(false);
                levelSelectController.CloseLevelSelect();
                loadingController.CloseLoadingPopup();
                menuController.CloseMenu();
                levelSelectController.CloseLevelSelect();

                topbarController.ShowTopbar();

                analyticsProvider.SendEvent(AnalyticsEvents.StartGameScene);

                _gameBootstrap.Configure
                    (levelRepository,
                    _gameSettingsSO,
                    walletController,
                    _soundController,
                    analyticsProvider,
                    adsProvider,
                    cameraController,
                    leaderboardController,
                    loadingController);

                async UniTask<bool> ShowRewardedAdsAsync()
                {
                    var result = await adsProvider.ShowRewarededAsync();

                    if (!result)
                        return false;

                    generalPopup.Close();

                    walletController.EarnCoins(level.Price);
                    entriesGameNumber = Math.Max(0, --entriesGameNumber);
                    SaveNumberInputs(entriesGameNumber);

                    StartGame();
                    return true;
                }
            }

            void RemoveProgress()
            {
                generalController.ShowRefinementRemovePanel(Remove);
                levelSelectController.ShowLevelSelect();
            }

            void Remove()
            {
                levelController.RemoveAllProgress();
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
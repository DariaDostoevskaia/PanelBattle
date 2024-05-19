using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Core.Levels;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.Infrastructure.Unity.Ads;
using LegoBattaleRoyal.Presentation.Controllers.General;
using LegoBattaleRoyal.Presentation.Controllers.Leaderboard;
using LegoBattaleRoyal.Presentation.Controllers.Loading;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace LegoBattaleRoyal.Presentation.Controllers.EndGame
{
    public class EndGameController : IDisposable
    {
        public event Action OnGameRestarted;

        private readonly CharacterRepository _characterRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly WalletController _walletController;
        private readonly UnityAdsProvider _adsProvider;
        private readonly LeaderboardController _leaderboardController;
        private readonly SoundController _soundController;
        private readonly GeneralController _generalController;
        private readonly LoadingController _loadingController;

        public EndGameController(CharacterRepository characterRepository,
            ILevelRepository levelRepository,
            SoundController soundController,
            WalletController walletController,
            GeneralController generalController,
            UnityAdsProvider adsProvider,
            LeaderboardController leaderboardController,
            LoadingController loadingController)
        {
            _levelRepository = levelRepository;
            _characterRepository = characterRepository;
            _walletController = walletController;
            _soundController = soundController;
            _generalController = generalController;
            _loadingController = loadingController;
            _adsProvider = adsProvider;
            _leaderboardController = leaderboardController;
        }

        private void ExitMainMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void RestartGame()
        {
            OnGameRestarted?.Invoke();
        }

        public void LoseGame()
        {
            _soundController.PlayLoseGameMusic();
            var currentLevel = _levelRepository.GetCurrentLevel();

            _generalController.ShowLosePopup(RestartGame, ExitMainMenu);
        }

        public bool TryWinGame()
        {
            var opponents = _characterRepository.GetOpponents();

            if (opponents.Any())
                return false;

            var currentLevel = _levelRepository.GetCurrentLevel();

            var isLastLevel = _levelRepository.Count == currentLevel.Order;

            var popupText = isLastLevel

                ? $"You earn {currentLevel.Reward}. " +
                $"Restart for {_levelRepository.Get(_levelRepository.GetAll().Min(level => level.Order)).Price}"

                : $"You earn {currentLevel.Reward}. " +
                $"Next for {_levelRepository.GetNextLevel().Price}.";

            currentLevel.Win();
            _walletController.EarnCoins(currentLevel.Reward);

            _soundController.PLayWinGameMusic();

            _leaderboardController.AddScore(currentLevel.Reward);

            var popup = _generalController.CreatePopup("You Win!", popupText);

            LevelModel nextLevel;

            if (isLastLevel)
            {
                currentLevel.Exit();

                var firstLevelOrder = _levelRepository.GetAll().Min(level => level.Order);
                nextLevel = _levelRepository.Get(firstLevelOrder);

                var restartButton = popup.CreateButton($"Restart");
                restartButton.onClick.AddListener(() =>
                {
                    restartButton.interactable = false;
                    popup.Close();

                    nextLevel.Launch();

                    RestartGame();
                });
            }
            else
            {
                nextLevel = _levelRepository.GetNextLevel();
                currentLevel.Exit();

                var nextButton = popup.CreateButton($"Next");
                nextButton.onClick.AddListener(() =>
                {
                    nextButton.interactable = false;
                    popup.Close();

                    nextLevel.Launch();

                    RestartGame();
                });
            }
            var exitButton = popup.CreateButton("Exit");
            exitButton.onClick.AddListener(() =>
            {
                exitButton.interactable = false;
                popup.Close();

                nextLevel.Launch();

                ExitMainMenu();
            });

            var showAdsButton = popup.CreateButton("x2 coins");
            showAdsButton.onClick.AddListener(() =>
            {
                showAdsButton.interactable = false;

                ShowRewarededAsync(currentLevel.Reward)
                .ContinueWith((result) => showAdsButton.interactable = !result)
                .Forget();
            });
            popup.Show();

            return true;
        }

        private async UniTask<bool> ShowRewarededAsync(int reward)
        {
            var result = await _adsProvider.ShowRewarededAsync();

            if (!result)
                return false;

            _walletController.EarnCoins(reward);
            return true;
        }

        public void Dispose()
        {
            OnGameRestarted = null;
        }
    }
}
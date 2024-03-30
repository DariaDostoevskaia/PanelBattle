using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.Infrastructure.Unity.Ads;
using LegoBattaleRoyal.Presentation.Controllers.General;
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
        private readonly GeneralController _generalController;
        private readonly UnityAdsProvider _adsProvider;
        private readonly SoundController _soundController;

        public EndGameController(CharacterRepository characterRepository,
            ILevelRepository levelRepository,
            SoundController soundController,
            WalletController walletController,
            GeneralController generalController,
            UnityAdsProvider adsProvider)
        {
            _levelRepository = levelRepository;
            _characterRepository = characterRepository;
            _walletController = walletController;
            _soundController = soundController;
            _generalController = generalController;
            _adsProvider = adsProvider;
        }

        private void ExitMainMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //loadingScreen
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
            currentLevel.Win();

            _walletController.EarnCoins(currentLevel.Reward);

            var isLastLevel = _levelRepository.Count == currentLevel.Order;

            _soundController.PLayWinGameMusic();

            if (!isLastLevel)
            {
                var nextLevel = _levelRepository.GetNextLevel();

                var popup = _generalController.CreatePopup("You Win!", $"You earn {currentLevel.Reward}.");

                var nextButton = popup.CreateButton($"Next for {nextLevel.Price}");
                nextButton.onClick.AddListener(() =>
                {
                    nextButton.interactable = false;
                    popup.Close();
                    var nextLevel = _levelRepository.GetNextLevel();
                    currentLevel.Exit();
                    nextLevel.Launch();
                    RestartGame();
                });

                var exitButton = popup.CreateButton("Exit");
                exitButton.onClick.AddListener(() =>
                {
                    exitButton.interactable = false;
                    popup.Close();
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
                //TODO ShowWinGamePOpup
                popup.Show();
                return true;
            }

            _generalController.ShowWinGamePopup(() =>
            {
                currentLevel.Exit();
                var firstLevelOrder = _levelRepository.GetAll().Min(level => level.Order);
                var firstLevel = _levelRepository.Get(firstLevelOrder);
                firstLevel.Launch();
                RestartGame();
            }, ExitMainMenu);

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
using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Core.Levels.Contracts;
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

        public event Action OnProgressRemoved;

        private readonly CharacterRepository _characterRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly WalletController _walletController;
        private readonly GeneralController _generalController;

        private readonly SoundController _soundController;

        public EndGameController(CharacterRepository characterRepository,
            ILevelRepository levelRepository,
            SoundController soundController,
            WalletController walletController,
            GeneralController generalController)
        {
            _levelRepository = levelRepository;
            _characterRepository = characterRepository;
            _walletController = walletController;
            _soundController = soundController;
            _generalController = generalController;
        }

        private void Remove()
        {
            OnProgressRemoved?.Invoke();
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
                _generalController.ShowWinLevelPopup(() =>
                {
                    var nextLevel = _levelRepository.GetNextLevel();
                    currentLevel.Exit();
                    nextLevel.Launch();
                    RestartGame();
                }, ExitMainMenu);

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

        public void Dispose()
        {
            OnGameRestarted = null;
            OnProgressRemoved = null;
        }
    }
}
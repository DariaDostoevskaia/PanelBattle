using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using LegoBattaleRoyal.Presentation.UI.GamePanel;
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
        private readonly GamePanelUI _endGamePopup;
        private readonly SoundController _soundController;

        public EndGameController(GamePanelUI endGamePopup, CharacterRepository characterRepository,
            ILevelRepository levelRepository, SoundController soundController, WalletController walletController)
        {
            _endGamePopup = endGamePopup;

            _levelRepository = levelRepository;
            _characterRepository = characterRepository;
            _walletController = walletController;
            _soundController = soundController;

            _endGamePopup.OnRestartClicked += RestartGame;
            _endGamePopup.OnNextLevelClicked += RestartGame;
            _endGamePopup.OnRemoveAllProgressClicked += Remove;
            _endGamePopup.OnExitMainMenuClicked += ExitMainMenu;
        }

        private void Remove()
        {
            _endGamePopup.Close();

            OnProgressRemoved?.Invoke();
        }

        private void ExitMainMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void RestartGame()
        {
            _endGamePopup.Close();

            OnGameRestarted?.Invoke();
        }

        public void LoseGame()
        {
            _endGamePopup.SetTitle("You Lose!");
            _endGamePopup.SetActiveRestartButton(true);
            _endGamePopup.SetActiveNextLevelButton(false);
            _endGamePopup.SetActiveRemoveAllProgress(false);

            _soundController.PlayLoseGameMusic();

            _endGamePopup.Show();
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

            _endGamePopup.SetTitle("You Win!");
            _endGamePopup.SetActiveRestartButton(false);
            _endGamePopup.SetActiveRemoveAllProgress(false);
            _endGamePopup.SetActiveNextLevelButton(!isLastLevel);

            _soundController.PLayWinGameMusic();

            if (!isLastLevel)
            {
                var nextLevel = _levelRepository.GetNextLevel();
                currentLevel.Exit();
                nextLevel.Launch();

                _endGamePopup.Show();

                return true;
            }

            _endGamePopup.SetTitle("You Won this game!");
            _endGamePopup.SetActiveRemoveAllProgress(true);
            _endGamePopup.Show();
            return true;
        }

        public void Dispose()
        {
            OnGameRestarted = null;
            OnProgressRemoved = null;

            _endGamePopup.OnRestartClicked -= RestartGame;
            _endGamePopup.OnNextLevelClicked -= RestartGame;
            _endGamePopup.OnRemoveAllProgressClicked -= Remove;
            _endGamePopup.OnExitMainMenuClicked -= ExitMainMenu;
        }
    }
}
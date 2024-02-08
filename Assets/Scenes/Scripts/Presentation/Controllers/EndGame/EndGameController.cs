using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using LegoBattaleRoyal.Presentation.UI.Container;
using LegoBattaleRoyal.Presentation.UI.GamePanel;
using LegoBattaleRoyal.Presentation.UI.General;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace LegoBattaleRoyal.Presentation.Controllers.EndGame
{
    public class EndGameController : IDisposable
    {
        public event Action OnGameRestarted;

        private readonly ILevelRepository _levelRepository;
        private readonly UIContainer _uiContainer;
        private readonly CharacterRepository _characterRepository;
        private readonly WalletController _walletController;
        private readonly GamePanelUI _endGamePopup;
        private readonly GeneralPopup _generalPopup;
        private readonly SoundController _soundController;

        public EndGameController(UIContainer uiContainer, CharacterRepository characterRepository,
            ILevelRepository levelRepository, SoundController soundController, WalletController walletController)
        {
            _uiContainer = uiContainer;
            _endGamePopup = _uiContainer.EndGamePopup;
            _generalPopup = _uiContainer.GeneralPopup;

            _levelRepository = levelRepository;
            _characterRepository = characterRepository;
            _walletController = walletController;
            _soundController = soundController;

            _endGamePopup.OnRestartClicked += RestartGame;
            _endGamePopup.OnNextLevelClicked += RestartGame;
            _endGamePopup.OnExitMainMenuClicked += ExitMainMenu;
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

            _endGamePopup.ShowLastLevel();
            return true;
        }

        public void Dispose()
        {
            OnGameRestarted = null;

            _endGamePopup.OnRestartClicked -= RestartGame;
            _endGamePopup.OnNextLevelClicked -= RestartGame;
            _endGamePopup.OnExitMainMenuClicked -= ExitMainMenu;
        }
    }
}
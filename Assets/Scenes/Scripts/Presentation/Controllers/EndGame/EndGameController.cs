using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Presentation.UI.GamePanel;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace LegoBattaleRoyal.Presentation.Controllers.EndGame
{
    public class EndGameController : IDisposable
    {
        public event Action OnGameRestarted;

        public event Action OnGameNexted;

        private readonly CharacterRepository _characterRepository;
        private readonly GamePanelUI _endGamePopup;

        public EndGameController(GamePanelUI endGamePopup, CharacterRepository characterRepository)
        {
            _endGamePopup = endGamePopup;
            _characterRepository = characterRepository;

            _endGamePopup.OnRestartClicked += RestartGame;
            _endGamePopup.OnNextLevelClicked += NextLevelGame;
            _endGamePopup.OnExitMainMenuClicked += ExitMainMenu;
        }

        private void NextLevelGame()
        {
            _endGamePopup.Close();

            OnGameNexted?.Invoke();
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
            _endGamePopup.Show();
        }

        public bool TryWinGame()
        {
            var opponents = _characterRepository.GetOpponents();

            if (opponents.Any())
                return false;

            _endGamePopup.SetTitle("You Win!");
            _endGamePopup.SetActiveRestartButton(true);
            _endGamePopup.SetActiveNextLevelButton(true);
            _endGamePopup.Show();

            return true;
        }

        public void Dispose()
        {
            OnGameRestarted = null;
            OnGameNexted = null;

            _endGamePopup.OnRestartClicked -= RestartGame;
            _endGamePopup.OnNextLevelClicked -= NextLevelGame;
            _endGamePopup.OnExitMainMenuClicked -= ExitMainMenu;
        }
    }
}
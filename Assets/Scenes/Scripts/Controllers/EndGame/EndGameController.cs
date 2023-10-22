using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.UI.GamePanel;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace LegoBattaleRoyal.Controllers.EndGame
{
    public class EndGameController : IDisposable
    {
        public event Action OnGameRestarted;

        private readonly CharacterRepository _characterRepository;
        private GamePanelUI _endGamePopup;

        public EndGameController(GamePanelUI endGamePopup, CharacterRepository characterRepository)
        {
            _endGamePopup = endGamePopup;
            _characterRepository = characterRepository;

            _endGamePopup.OnRestartClicked += RestartGame;
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
            _endGamePopup.Show();
        }

        public bool TryWinGame()
        {
            var opponents = _characterRepository.GetOpponents();

            if (opponents.Any())
                return false;

            _endGamePopup.SetTitle("You Win!");
            _endGamePopup.SetActiveRestartButton(true);
            _endGamePopup.Show();

            return true;
        }

        public void Dispose()
        {
            OnGameRestarted = null;

            _endGamePopup.OnExitMainMenuClicked -= ExitMainMenu;
            _endGamePopup.OnRestartClicked -= RestartGame;
        }
    }
}
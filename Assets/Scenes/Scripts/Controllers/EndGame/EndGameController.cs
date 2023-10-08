using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.UI;
using System;
using System.Linq;

namespace LegoBattaleRoyal.Controllers.EndGame
{
    public class EndGameController
    {
        private readonly GamePanel _gamePanel;
        private readonly CharacterRepository _characterRepository;
        public event Action OnGameRestarted;

        public EndGameController(GamePanel gamePanel, CharacterRepository characterRepository)
        {
            _gamePanel = gamePanel;
            _characterRepository = characterRepository;

            _gamePanel.OnRestartClicked += RestartGame;
        }

        private void RestartGame()
        {
            _gamePanel.Close();
            OnGameRestarted?.Invoke();
        }

        public void LoseGame()
        {
            _gamePanel.SetTitle("You Lose!");
            _gamePanel.SetActiveRestartButton(true);
            _gamePanel.Show();
        }

        public bool TryWinGame()
        {
            var opponents = _characterRepository.GetOpponents();

            if (opponents.Any())
                return false;

            _gamePanel.SetTitle("You Win!");
            _gamePanel.SetActiveRestartButton(true);
            _gamePanel.Show();

            return true;
        }
    }
}

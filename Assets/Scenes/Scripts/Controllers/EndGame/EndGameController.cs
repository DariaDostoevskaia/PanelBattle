using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.UI.GamePanel;
using LegoBattaleRoyal.UI.MainMenu;
using System;
using System.Linq;

namespace LegoBattaleRoyal.Controllers.EndGame
{
    public class EndGameController : IDisposable
    {
        public event Action OnGameRestarted;

        public event Action OnExitedMenu;

        private readonly GamePanelUI _gamePanel;
        private readonly MainMenuPanel _menuPanel;
        private readonly CharacterRepository _characterRepository;

        public EndGameController(GamePanelUI gamePanel, MainMenuPanel menuPanel, CharacterRepository characterRepository)
        {
            _gamePanel = gamePanel;
            _menuPanel = menuPanel;
            _characterRepository = characterRepository;

            _gamePanel.OnRestartClicked += RestartGame;
            _gamePanel.OnExitMainMenuClicked += ExitMainMenu;
        }

        private void ExitMainMenu()
        {
            _menuPanel.Show();
            _gamePanel.Close();

            OnExitedMenu?.Invoke();
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

        public void Dispose()
        {
            OnGameRestarted = null;
            OnExitedMenu = null;

            _gamePanel.OnRestartClicked -= RestartGame;

            _gamePanel.OnExitMainMenuClicked -= ExitMainMenu;
        }
    }
}
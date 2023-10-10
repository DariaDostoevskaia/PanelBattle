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

        public event Action OnMainMenuExited;

        public event Action OnEndGame;

        private readonly GamePanelUI _gamePanel;
        private readonly MainMenuPanel _menuPanel;
        private readonly CharacterRepository _characterRepository;

        public EndGameController(GamePanelUI gamePanel, MainMenuPanel menuPanel, CharacterRepository characterRepository)
        {
            _gamePanel = gamePanel;
            _menuPanel = menuPanel;
            _characterRepository = characterRepository;

            _gamePanel.OnRestartClicked += RestartGame;
            _gamePanel.OnExitMainMenu += ExitMainMenu;
        }

        private void ExitMainMenu()
        {
            _gamePanel.Close();
            _menuPanel.gameObject.SetActive(true);
            OnMainMenuExited?.Invoke();
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

            OnEndGame?.Invoke();
        }

        public bool TryWinGame()
        {
            var opponents = _characterRepository.GetOpponents();

            if (opponents.Any())
                return false;

            _gamePanel.SetTitle("You Win!");
            _gamePanel.SetActiveRestartButton(true);
            _gamePanel.Show();

            OnEndGame?.Invoke();

            return true;
        }

        public void Dispose()
        {
            OnGameRestarted = null;
            OnMainMenuExited = null;
            OnEndGame = null;

            _gamePanel.OnRestartClicked -= RestartGame;
            _gamePanel.OnExitMainMenu -= ExitMainMenu;
        }
    }
}
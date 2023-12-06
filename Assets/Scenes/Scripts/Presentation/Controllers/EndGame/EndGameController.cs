using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.Presentation.UI.GamePanel;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LegoBattaleRoyal.Presentation.Controllers.EndGame
{
    public class EndGameController : IDisposable
    {
        public event Action OnGameRestarted;

        private readonly CharacterRepository _characterRepository;
        private readonly ILevelRepository _levelRepository;

        private readonly GamePanelUI _endGamePopup;

        private readonly GameSettingsSO _gameSettingsSO;
        private readonly AudioSource _audioSource;

        public EndGameController(GamePanelUI endGamePopup, CharacterRepository characterRepository,
            ILevelRepository levelRepository, GameSettingsSO gameSettingsSO)
        {
            _endGamePopup = endGamePopup;
            _characterRepository = characterRepository;
            _levelRepository = levelRepository;
            _gameSettingsSO = gameSettingsSO;

            _endGamePopup.OnRestartClicked += RestartGame;
            _endGamePopup.OnNextLevelClicked += RestartGame;
            _endGamePopup.OnExitMainMenuClicked += ExitMainMenu;

            _audioSource.loop = false;
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

            var loseMusic = _gameSettingsSO.LoseGameMusic;
            PlaySound(loseMusic);

            _endGamePopup.Show();
        }

        public bool TryWinGame()
        {
            var opponents = _characterRepository.GetOpponents();

            if (opponents.Any())
                return false;

            var currentLevel = _levelRepository.GetCurrentLevel();
            currentLevel.Win();
            var isLastLevel = _levelRepository.Count == currentLevel.Order;

            _endGamePopup.SetTitle("You Win!");
            _endGamePopup.SetActiveRestartButton(false);
            _endGamePopup.SetActiveNextLevelButton(!isLastLevel);

            var winMusic = _gameSettingsSO.WinGameMusic;
            PlaySound(winMusic);

            if (!isLastLevel)
            {
                var nextLevel = _levelRepository.GetNextLevel();
                currentLevel.Exit();
                nextLevel.Launch();

                _endGamePopup.Show();
                return true;
            }

            _endGamePopup.Show();
            return true;
        }

        private void PlaySound(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
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
using LegoBattaleRoyal.Core.Levels;
using LegoBattaleRoyal.Infrastructure.Repository;
using LegoBattaleRoyal.Presentation.UI.LevelSelect;
using System;
using System.Linq;

namespace LegoBattaleRoyal.Presentation.Controllers.LevelSelect
{
    public class LevelSelectController : IDisposable
    {
        public event Action OnLevelInvoked;

        private readonly LevelSelectView _levelSelectView;
        private readonly LevelRepository _levelRepository;

        public LevelSelectController(LevelSelectView levelSelectView, LevelRepository levelRepository)
        {
            _levelSelectView = levelSelectView;
            _levelRepository = levelRepository;
        }

        public void ShowLevelSelect()
        {
            _levelSelectView.SetLevels(_levelRepository.GetAll().ToArray());

            _levelSelectView.Selected += OnLevelSelected;
        }

        private void OnLevelSelected(LevelModel level)
        {
            var currentLevel = _levelRepository.GetCurrentLevel();

            if (level.IsFinished
                || level == currentLevel)
            {
                currentLevel.Exit();
                level.Launch();

                OnLevelInvoked?.Invoke();
            }
        }

        public void CloseLevelSelect()
        {
            _levelSelectView.Close();
        }

        public void Dispose()
        {
            OnLevelInvoked = null;
            _levelSelectView.Selected -= OnLevelSelected;
        }
    }
}
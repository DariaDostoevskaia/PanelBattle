using LegoBattaleRoyal.Core.Levels;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;

namespace LegoBattaleRoyal.Infrastructure.Repository
{
    public class LevelRepository : ILevelRepository
    {
        private readonly List<LevelModel> _levelModels = new();
        private LevelSO[] _levelsSO;

        public LevelRepository(LevelSO[] levelsSO)
        {
            _levelsSO = levelsSO;
        }

        public int Count => _levelsSO.Count();

        public void Add(LevelModel levelModel)
        {
            _levelModels.Add(levelModel);
        }

        public IEnumerable<LevelModel> GetAll()
        {
            return _levelModels;
        }

        public LevelModel GetCurrentLevel()
        {
            return _levelModels.First(level => level.IsCurrent);
        }

        public IEnumerable<LevelModel> GetFinishedLevels()
        {
            return _levelModels.Where(level => level.IsFinished);
        }

        public LevelModel GetNextLevel()
        {
            var current = GetCurrentLevel();
            var nextCurrent = current.Order + 1;

            var nextLevel = _levelModels.FirstOrDefault(level => level.Order == nextCurrent);

            return nextLevel;
        }
    }
}
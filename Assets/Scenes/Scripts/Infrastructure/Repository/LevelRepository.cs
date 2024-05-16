using LegoBattaleRoyal.Core.Levels;
using LegoBattaleRoyal.Core.Levels.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace LegoBattaleRoyal.Infrastructure.Repository
{
    public class LevelRepository : ILevelRepository
    {
        private readonly List<LevelModel> _levelModels = new();

        public int Count => _levelModels.Count;

        public void Add(LevelModel levelModel)
        {
            _levelModels.Add(levelModel);
        }

        public LevelModel Get(int levelOrder)
        {
            return _levelModels.First(level => level.Order == levelOrder);
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
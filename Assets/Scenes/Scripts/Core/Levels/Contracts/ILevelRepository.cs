using System.Collections.Generic;

namespace LegoBattaleRoyal.Core.Levels.Contracts
{
    public interface ILevelRepository
    {
        int Count { get; }

        IEnumerable<LevelModel> GetAll();

        void Add(LevelModel levelModel);

        LevelModel GetCurrentLevel();

        LevelModel GetNextLevel();

        IEnumerable<LevelModel> GetFinishedLevels();
    }
}
using LegoBattaleRoyal.App.DTO;
using LegoBattaleRoyal.ApplicationLayer.SaveSystem;
using LegoBattaleRoyal.Core.Levels;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Linq;

namespace LegoBattaleRoyal.Presentation.Controllers.Levels
{
    public class LevelController : IDisposable
    {
        private readonly ILevelRepository _levelRepository;
        private readonly ISaveService _saveService;

        public LevelController(ILevelRepository levelRepository, ISaveService saveService)
        {
            _levelRepository = levelRepository;
            _saveService = saveService;
        }

        public void CreateLevels(LevelSO[] levelSettings)
        {
            var levelDTO = _saveService.Exists<LevelDTO>()
                ? _saveService.Load<LevelDTO>()
                : new LevelDTO();

            for (int i = 0; i < levelSettings.Length; i++)
            {
                var order = i + 1;
                var isFinished = levelDTO.FinishedOrders.Contains(order);

                var levelSO = levelSettings[i];

                var level = new LevelModel(order, levelSO.Price, levelSO.Reward, isFinished);

                if (order == levelDTO.CurrentOrder)
                    level.Launch();

                level.OnSuccessEnded += OnSuccessEnded;

                _levelRepository.Add(level);
            }
        }

        public void RemoveAllProgress()
        {
            _saveService.DeleteAllLocal();
        }

        private void OnSuccessEnded()
        {
            var currentLevel = _levelRepository.GetCurrentLevel();

            var finishedLevels = _levelRepository
                .GetFinishedLevels()
                .Select(level => level.Order)
                .ToArray();

            _saveService.Save(new LevelDTO()
            {
                FinishedOrders = finishedLevels,
                CurrentOrder = currentLevel.Order + 1
            });
        }

        public void Dispose()
        {
            foreach (var level in _levelRepository.GetAll())
            {
                level.OnSuccessEnded -= OnSuccessEnded;
                level.Dispose();
            }
        }
    }
}
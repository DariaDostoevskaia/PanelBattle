using LegoBattaleRoyal.App.DTO;
using LegoBattaleRoyal.ApplicationLayer.SaveSystem;
using LegoBattaleRoyal.Core.Levels;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.ScriptableObjects;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.Controllers.Levels
{
    public class LevelController : MonoBehaviour
    {
        private readonly ILevelRepository _levelRepository;
        private readonly ISaveService _saveService;
        private LevelModel _level;

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

            var levels = levelSettings.Select((levelSO, i) =>
            {
                var order = i + 1;
                var isFinished = levelDTO.FinishedOrders.Contains(order);
                _level = new LevelModel(order, levelSO.Price, levelSO.Reward, isFinished);

                if (order == levelDTO.CurrentOrder)
                    _level.Launch();

                _level.OnSuccessEnded += OnSuccessEnded;

                return _level;
            });
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

        private void OnDestroy()
        {
            _level.OnSuccessEnded -= OnSuccessEnded;
            _level.Dispose();
        }
    }
}
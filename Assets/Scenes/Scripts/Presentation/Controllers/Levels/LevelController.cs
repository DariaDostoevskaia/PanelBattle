using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.App.DTO.Level;
using LegoBattaleRoyal.ApplicationLayer.SaveSystem;
using LegoBattaleRoyal.Core.Levels;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.Controllers.Levels
{
    public class LevelController : IDisposable
    {
        private readonly ILevelRepository _levelRepository;
        private readonly ISaveService _saveService;
        private readonly WalletController _walletController;
        private LevelDTO _levelDTO;

        public LevelController(
            ILevelRepository levelRepository,
            ISaveService saveService,
            WalletController walletController)
        {
            _levelRepository = levelRepository;
            _saveService = saveService;
            _walletController = walletController;
        }

        public void CreateLevels(LevelSO[] levelSettings)
        {
            _levelDTO = _saveService.Exists<LevelDTO>()
               ? _saveService.Load<LevelDTO>()
               : new LevelDTO();

            for (int i = 0; i < levelSettings.Length; i++)
            {
                var order = i + 1;
                var isFinished = _levelDTO.FinishedOrders.Contains(order);

                var levelSO = levelSettings[i];

                var level = new LevelModel(order, levelSO.Price, levelSO.Reward, isFinished);

                if (order == _levelDTO.CurrentOrder)
                    level.Launch();

                level.OnSuccessEnded += OnSuccessEnded;

                _levelRepository.Add(level);
            }
        }

        public bool TryBuyLevel(int price)
        {
            if (!_walletController.CanBuy(price))
                return false;
            _walletController.SpendCoins(price);

            return true;
        }

        public void RemoveAllProgress()
        {
            try
            {
                var currentLevel = _levelRepository.GetCurrentLevel();
                currentLevel.Exit();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            var firstLevelOrder = _levelRepository.GetAll().Min(level => level.Order);
            var firstLevel = _levelRepository.Get(firstLevelOrder);
            firstLevel.Launch();

            _saveService.DeleteAllLocal();
            _saveService.Save(firstLevel);
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
                CurrentOrder = currentLevel.Order >= _levelRepository.Count
                ? 1
                : currentLevel.Order + 1,
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
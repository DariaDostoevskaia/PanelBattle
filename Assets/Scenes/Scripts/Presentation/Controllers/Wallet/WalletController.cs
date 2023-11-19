using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.App.DTO;
using LegoBattaleRoyal.Core.Wallet;
using LegoBattaleRoyal.ScriptableObjects;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Wallet
{
    public class WalletController
    {
        private WalletModel _walletModel;
        private SaveService _saveService;
        private LevelSO _levelSO;

        //void Start()
        //{
        //    LoadWalletData();
        //}

        public bool CanUnlockLevel(LevelDTO levelData)
        {
            return _walletModel.Money >= levelData.levelCost;
        }

        public void SpendCoins(int amount)
        {
            _walletModel.Money -= amount;
            SaveWalletData();
        }

        public void EarnCoins(int amount)
        {
            _walletModel.Money += amount;
            SaveWalletData();
        }

        public void LoadWalletData()
        {
            // Используйте SaveService для загрузки данных кошелька из сохраненных данных.
            var initValue = _saveService.Exists<PlayerWalletDto>()
                 ? _saveService.Load<PlayerWalletDto>().WalletValue
                 : /*_gameSettingsSO.Money; */ 100;

            _walletModel = new WalletModel(initValue);
        }

        public void SaveWalletData()
        {
            // Используйте SaveService для сохранения данных кошелька.

            var playerDTO = new PlayerWalletDto()
            {
                WalletValue = _walletModel.Money
            };
            _saveService.Save(playerDTO);
        }
    }
}

[Serializable]
public class PlayerWalletDto
{
    public int WalletValue { get; set; }
}
using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.App.DTO.Level;
using LegoBattaleRoyal.App.DTO.Wallet;
using LegoBattaleRoyal.Core.Wallet;
using LegoBattaleRoyal.ScriptableObjects;

namespace LegoBattaleRoyal.Presentation.Controllers.Wallet
{
    public class WalletController
    {
        private WalletModel _walletModel;
        private SaveService _saveService;
        private LevelSO _levelSO;
        private GameSettingsSO _gameSettingsSO;

        public WalletController(SaveService saveService, GameSettingsSO gameSettingsSO)
        {
            _saveService = saveService;
            _gameSettingsSO = gameSettingsSO;
        }

        public bool CanUnlockLevel(LevelDTO levelData)
        {
            return _walletModel.Money >= levelData.LevelCost;
        }

        public void SpendCoins()
        {
            _walletModel.Money -= _levelSO.Price;
            SaveWalletData();
        }

        public void EarnCoins()
        {
            _walletModel.Money += _levelSO.Reward;
            SaveWalletData();
        }

        public void LoadWalletData()
        {
            var initValue = _saveService.Exists<PlayerWalletDto>()
                 ? _saveService.Load<PlayerWalletDto>().WalletValue
                 : _gameSettingsSO.Money;

            _walletModel = new WalletModel(initValue);
        }

        public void SaveWalletData()
        {
            var playerDTO = new PlayerWalletDto()
            {
                WalletValue = _walletModel.Money
            };
            _saveService.Save(playerDTO);
        }

        public void LookAdvertisement()
        {
            //Look Advertisement & get money for level
            var needCountMoney = _levelSO.Price;
            _walletModel.Money = needCountMoney;
        }
    }
}
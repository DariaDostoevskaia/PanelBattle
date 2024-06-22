using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.App.DTO.Wallet;
using LegoBattaleRoyal.ApplicationLayer.SaveSystem;
using LegoBattaleRoyal.Core.Wallet;
using LegoBattaleRoyal.ScriptableObjects;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Wallet
{
    public class WalletController : IDisposable
    {
        public event Action<int> Changed;

        private WalletModel _walletModel;
        private readonly ISaveService _saveService;
        private readonly GameSettingsSO _gameSettingsSO;

        public WalletController(SaveService saveService, GameSettingsSO gameSettingsSO)
        {
            _saveService = saveService;
            _gameSettingsSO = gameSettingsSO;
        }

        public bool CanBuy(int price)
        {
            return _walletModel.Money >= price;
        }

        public void SpendCoins(int price)
        {
            _walletModel.Money -= price;
            SaveWalletData();
        }

        public void EarnCoins(int reward)
        {
            _walletModel.Money += reward;
            SaveWalletData();
        }

        public int GetCurrentMoney()
        {
            return _walletModel.Money;
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
                WalletValue = _walletModel.Money,
            };
            _saveService.Save(playerDTO);
            Changed?.Invoke(playerDTO.WalletValue);
        }

        public void Dispose()
        {
            Changed = null;
        }
    }
}
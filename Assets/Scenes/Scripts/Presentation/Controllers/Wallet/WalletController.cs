using LegoBattaleRoyal.App.AppService;
using LegoBattaleRoyal.App.DTO;
using LegoBattaleRoyal.Core.Wallet;
using LegoBattaleRoyal.ScriptableObjects;

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
            // ����������� SaveService ��� �������� ������ �������� �� ����������� ������.
            _walletModel = _saveService.LoadWalletData();
        }

        public void SaveWalletData()
        {
            // ����������� SaveService ��� ���������� ������ ��������.
            _saveService.SaveWalletData(_walletModel);
        }
    }
}
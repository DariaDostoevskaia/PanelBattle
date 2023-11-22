namespace LegoBattaleRoyal.Core.Wallet
{
    public class WalletModel
    {
        private int _money;
        private int _initValue;

        public WalletModel(int initValue)
        {
            _initValue = initValue;
        }

        public int Money { get; set; }
    }
}
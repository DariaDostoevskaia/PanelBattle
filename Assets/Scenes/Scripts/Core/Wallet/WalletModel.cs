namespace LegoBattaleRoyal.Core.Wallet
{
    public class WalletModel
    {
        private int _money;
        private int initValue;

        public WalletModel(int initValue)
        {
            this.initValue = initValue;
        }

        public int Money => _money;
    }
}
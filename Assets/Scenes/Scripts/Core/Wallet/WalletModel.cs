using System;

namespace LegoBattaleRoyal.Core.Wallet
{
    public class WalletModel
    {
        public int Money { get; set; }

        public WalletModel(int initValue)
        {
            if (initValue < 0)
                throw new Exception("Init value don't be a negative.");

            Money = initValue;
        }
    }
}
using System;

namespace LegoBattaleRoyal.Round
{
    public class RoundController
    {
        public event Action OnRoundChanged;

        public void ChangeRound()
        {
            OnRoundChanged?.Invoke();
        }
    }
}
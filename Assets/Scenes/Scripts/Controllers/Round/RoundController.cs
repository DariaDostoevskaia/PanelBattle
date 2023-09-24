using System;

namespace LegoBattaleRoyal.Controllers.Round
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
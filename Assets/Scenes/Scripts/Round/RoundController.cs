using System;

namespace LegoBattaleRoyal.Round
{
    public class RoundController : IDisposable
    {
        public event Action OnRoundChanged;

        public void ChangeRound()
        {
            OnRoundChanged?.Invoke();
        }

        public void RoundTransition()
        {
        }

        public void Dispose()
        {
            OnRoundChanged = null;
        }
    }
}
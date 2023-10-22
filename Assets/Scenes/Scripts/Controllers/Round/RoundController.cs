using System;

namespace LegoBattaleRoyal.Controllers.Round
{
    public class RoundController : IDisposable
    {
        public event Action OnRoundChanged;

        public void ChangeRound()
        {
            OnRoundChanged?.Invoke();
        }

        public void Dispose()
        {
            OnRoundChanged = null;
        }
    }
}
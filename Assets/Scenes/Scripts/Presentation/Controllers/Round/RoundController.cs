using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Round
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
using LegoBattaleRoyal.AI;
using LegoBattaleRoyal.Characters.Models;
using System;
using UnityEditor.Build.Player;

namespace LegoBattaleRoyal.Round
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
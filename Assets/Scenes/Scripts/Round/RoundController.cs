using LegoBattaleRoyal.AI;
using LegoBattaleRoyal.Characters.Models;
using System;
using UnityEditor.Build.Player;

namespace LegoBattaleRoyal.Round
{
    public class RoundController : IDisposable
    {
        private AIController _aicontroller;
        private AICharacterModel _aicharacterModel;

        public event Action OnRoundChanged;

        public void ChangeRound()
        {
            OnRoundChanged?.Invoke();
            _aicontroller.ProcessRoundState(_aicharacterModel);
        }

        public void Dispose()
        {
            OnRoundChanged = null;
        }
    }
}
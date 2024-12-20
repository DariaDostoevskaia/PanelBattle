using System;
using System.Collections.Generic;
using System.Linq;

namespace LegoBattaleRoyal.Core.Panels.Models
{
    public class PanelModel : IDisposable
    {
        public Action<Guid> OnReleased;

        private readonly Dictionary<Guid, State> _stateForCharacters = new();

        public GridPosition GridPosition { get; }

        public bool IsExternalPanel { get; }

        public bool IsJumpBlock { get; }

        public bool IsBase => _stateForCharacters.Values.Any(state => state.IsBase);

        public PanelModel(bool isJumpBlock, GridPosition gridPosition, bool isExternalPanel)
        {
            IsJumpBlock = isJumpBlock;
            GridPosition = gridPosition;
            IsExternalPanel = isExternalPanel;
        }

        public bool IsCaptured(Guid characterId)
        {
            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            return state.IsCaptured;
        }

        public bool IsOccupated(Guid characterId)
        {
            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            return state.IsOccupated;
        }

        public void Capture(Guid characterId)
        {
            foreach (var stateForCharacter in _stateForCharacters)
            {
                stateForCharacter.Value.SetCapture(false);

                stateForCharacter.Value.Occupate(false);

                OnReleased?.Invoke(stateForCharacter.Key);
            }

            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            state.SetCapture(true);
        }

        public void Occupate(Guid characterId)
        {
            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            state.Occupate(true);
        }

        public bool IsVisiting(Guid characterId)
        {
            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            return state.IsVisiting;
        }

        public bool IsAvailable(Guid characterId)
        {
            if (!IsJumpBlock)
                return false;

            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            return state.IsAvailable;
        }

        public void BuildBase(Guid characterId)
        {
            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            state.BuildBase();
        }

        public void SetAvailable(Guid characterId)
        {
            if (!IsJumpBlock)
                throw new Exception("Block is not available for jump.");

            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            state.SetAvailable(true);
        }

        public void SetUnavailable(Guid characterId)
        {
            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            state.SetAvailable(false);
        }

        public void Add(Guid characterId)
        {
            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            if (state.IsVisiting)
                throw new Exception("Block has player already.");

            state.AddVisitor();
        }

        public void Remove(Guid characterId)
        {
            if (!_stateForCharacters.TryGetValue(characterId, out State state))
                state = _stateForCharacters[characterId] = new State();

            if (!state.IsVisiting)
                throw new Exception("Block has not player yet.");

            state.RemoveVisitor();
        }

        public void Dispose()
        {
            OnReleased = null;
        }
    }
}
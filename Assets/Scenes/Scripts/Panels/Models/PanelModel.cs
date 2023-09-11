using System;
using System.Collections.Generic;

namespace LegoBattaleRoyal.Panels.Models
{
    public class PanelModel
    {
        private readonly Dictionary<Guid, State> _stateForCharacters = new();

        public bool IsJumpBlock { get; }

        public GridPosition GridPosition { get; }

        public PanelModel(bool isJumpBlock, GridPosition gridPosition)
        {
            IsJumpBlock = isJumpBlock;
            GridPosition = gridPosition;
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
            Add(characterId);

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
    }
}
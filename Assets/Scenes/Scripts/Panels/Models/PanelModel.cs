using LegoBattaleRoyal.Panels.Controllers;
using System;

namespace LegoBattaleRoyal.Panels.Models
{
    public class PanelModel
    {
        private readonly State _state;

        public bool IsJumpBlock { get; }

        public bool IsAvailable => _state.IsAvailable;

        public bool IsVisiting => _state.IsVisiting;

        public GridPosition GridPosition { get; }

        public PanelModel(bool isJumpBlock, GridPosition gridPosition)
        {
            IsJumpBlock = isJumpBlock;
            GridPosition = gridPosition;
            _state = new State();
        }

        public void BuildBase()
        {
            Add();
            _state.BuildBase();
        }

        public void SetAvailable()
        {
            if (!IsJumpBlock)
                throw new Exception("Block is not available for jump.");
            _state.SetAvailable(true);
        }

        public void SetUnavailable()
        {
            _state.SetAvailable(false);
        }

        public void Add()
        {
            if (_state.IsVisiting)
                throw new Exception("Block has player already.");

            _state.AddVisitor();
        }

        public void Remove()
        {
            if (!_state.IsVisiting)
                throw new Exception("Block has not player yet.");

            _state.RemoveVisitor();
        }
    }
}
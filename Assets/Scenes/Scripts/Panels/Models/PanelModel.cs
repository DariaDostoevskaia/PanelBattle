using System;

namespace LegoBattaleRoyal.Panels.Models
{
    public class PanelModel
    {
        private readonly State _state;

        public int[] GridPosition { get; private set; }

        public bool IsJumpBlock { get; }

        public bool IsAvailable => _state.IsAvailable;

        public bool IsVisiting => _state.IsVisiting;

        public PanelModel(bool isJumpBlock)
        {
            IsJumpBlock = isJumpBlock;
            _state = new State();
        }

        public void BuildBase()
        {
            Add();
            _state.BuildBase();
        }

        public int[] GetGridPosition(int[] gridPosition)
        {
            GridPosition = gridPosition;
            return gridPosition;
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
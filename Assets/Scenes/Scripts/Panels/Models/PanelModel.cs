using System;
using Unity.VisualScripting;

namespace LegoBattaleRoyal.Panels.Models
{
    public class PanelModel
    {
        private readonly State _state;

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
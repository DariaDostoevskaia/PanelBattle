using System;

namespace LegoBattaleRoyal.Panels.Models
{
    public class PanelModel
    {
        public bool IsBase { get; private set; }
        public bool IsAvailable { get; private set; }
        public bool IsJumpBlock { get; }

        public PanelModel(bool isJumpBlock)
        {
            IsJumpBlock = isJumpBlock;
        }

        public void BuildBase()
        {
            IsBase = true;
        }

        public void SetAvailable()
        {
            IsAvailable = true;
        }

        public void SetUnavailable()
        {
            IsAvailable = false;
        }
    }
}
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.GameView.Character
{
    public static class AnimationConstants
    {
        private static readonly int _jumpTrigger = Animator.StringToHash("Jump");

        public static int JumpTriggerHash => _jumpTrigger;
    }
}
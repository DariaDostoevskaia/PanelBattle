using UnityEngine;

namespace LegoBattaleRoyal.Presentation.GameView.Character
{
    public static class AnimationConstants
    {
        public static int JumpTriggerHash => _jumpTrigger;

        private static readonly int _jumpTrigger = Animator.StringToHash("Jump");
    }
}
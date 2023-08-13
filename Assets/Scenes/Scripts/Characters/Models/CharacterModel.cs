namespace LegoBattaleRoyal.Characters.Models
{
    public class CharacterModel
    {
        public float MoveDuration { get; }

        public float JumpHeight { get; }

        public float Speed { get; }

        public CharacterModel(float moveDuration, float jumpHeight, float speed)
        {
            MoveDuration = moveDuration;
            JumpHeight = jumpHeight;
            Speed = speed;
        }
    }
}
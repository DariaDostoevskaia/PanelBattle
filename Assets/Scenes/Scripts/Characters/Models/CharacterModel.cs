namespace LegoBattaleRoyal.Characters.Models
{
    public class CharacterModel
    {
        public float MoveDuration { get; }

        public float JumpHeight { get; }

        public CharacterModel(float moveDuration, float jumpHeight)
        {
            MoveDuration = moveDuration;
            JumpHeight = jumpHeight;
        }
    }
}
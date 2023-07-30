namespace LegoBattaleRoyal.Characters.Models
{
    public class CharacterModel
    {
        public float MoveDuration { get; }

        public CharacterModel(float moveDuration)
        {
            MoveDuration = moveDuration;
        }
    }
}
namespace LegoBattaleRoyal.Characters.Domain
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
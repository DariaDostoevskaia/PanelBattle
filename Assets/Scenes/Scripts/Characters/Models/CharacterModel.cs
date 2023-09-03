using System;

namespace LegoBattaleRoyal.Characters.Models
{
    public class CharacterModel
    {
        public int JumpLenght { get; }
        public Guid Id { get; }

        public CharacterModel(int jumpLenght)
        {
            JumpLenght = jumpLenght;
            Id = Guid.NewGuid();
        }
    }
}
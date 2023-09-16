using System;

namespace LegoBattaleRoyal.Characters.Models
{
    public class CharacterModel
    {
        public int JumpLenght { get; }

        public Guid Id { get; }

        public CharacterModel(int jumpLenght)
        {
            Id = Guid.NewGuid();

            if (jumpLenght < 0)
            {
                throw new ArgumentOutOfRangeException("The value of the jump length can not be negative or equal to zero. " +
                    "Enter a jump length value greater than zero.");
            }

            JumpLenght = jumpLenght;
        }
    }
}
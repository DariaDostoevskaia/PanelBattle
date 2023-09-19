using LegoBattaleRoyal.Panels.Models;
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

            if (jumpLenght <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(jumpLenght), jumpLenght, "Exepted > 0");
            }

            JumpLenght = jumpLenght;
        }

        public void Capture(PanelModel panelModel)
        {
        }
    }
}
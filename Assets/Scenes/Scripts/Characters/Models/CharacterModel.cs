using LegoBattaleRoyal.Panels.Models;
using System;

namespace LegoBattaleRoyal.Characters.Models
{
    public class CharacterModel : IDisposable
    {
        public int JumpLenght { get; }
        public Guid Id { get; }

        public event Action<Guid> OnEndCaptured;

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
            if (panelModel == null)
                throw new ArgumentNullException(nameof(panelModel));

            panelModel.Capture(Id);

            OnEndCaptured?.Invoke(Id);
        }

        public void Dispose()
        {
            OnEndCaptured = null;
        }
    }
}
using LegoBattaleRoyal.Panels.Models;
using System;

namespace LegoBattaleRoyal.Characters.Models
{
    public class CharacterModel : IDisposable
    {
        public event Action<Guid> OnEndCaptured;

        public int JumpLenght { get; }

        public Guid Id { get; }

        protected GridPosition CurrentPosition { get; private set; }

        public CharacterModel(int jumpLenght)
        {
            if (jumpLenght <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(jumpLenght), jumpLenght, "Exepted > 0");
            }

            Id = Guid.NewGuid();
            JumpLenght = jumpLenght;
            CurrentPosition = new GridPosition(0, 0);
        }

        public void Capture(PanelModel panelModel)
        {
            if (panelModel == null)
                throw new ArgumentNullException(nameof(panelModel));

            panelModel.Capture(Id);

            OnEndCaptured?.Invoke(Id);
        }

        public void Move(PanelModel panelModel)
        {
            CurrentPosition.Change(panelModel.GridPosition);
            panelModel.Add(Id);
        }

        public void Dispose()
        {
            OnEndCaptured = null;
        }
    }
}
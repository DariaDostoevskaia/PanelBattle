using LegoBattaleRoyal.Panels.Models;
using System;
using System.Collections.Generic;

namespace LegoBattaleRoyal.Characters.Models
{
    public class CharacterModel
    {
        public int JumpLenght { get; }
        public Guid Id { get; }

        public List<PanelModel> _panelModels = new();
        public bool IsBasePanel { get; set; }

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

            _panelModels.Add(panelModel);

            if (_panelModels.Count > 0)
                for (int i = 1; i < _panelModels.Count; i++)
                {
                    if (_panelModels[i] == _panelModels[0])
                    {
                        IsBasePanel = true;
                        OnEndCaptured?.Invoke(Id);
                        _panelModels.Clear();
                    }
                }
        }
    }
}
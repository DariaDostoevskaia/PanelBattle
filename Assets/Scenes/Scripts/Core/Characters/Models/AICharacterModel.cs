using LegoBattaleRoyal.Core.AI.AIStrategy;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Strategy;
using LegoBattaleRoyal.Strategy.Difficulty;
using System;

namespace LegoBattaleRoyal.Characters.Models
{
    public class AICharacterModel : CharacterModel
    {
        private readonly AIMovementStrategy _aiMovementStrategy;

        public AICharacterModel(int jumpLenght, int blocksToCapture, Difficulty difficulty, PanelModel[] panelModels)
            : base(jumpLenght)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    _aiMovementStrategy = new EasyAIMovementStrategy(blocksToCapture, CurrentPosition, panelModels, Id);
                    break;

                case Difficulty.Medium:
                    _aiMovementStrategy = new MediumAIMovement(blocksToCapture, CurrentPosition, panelModels, Id);
                    break;

                case Difficulty.Hard:
                    break;
            }
        }
        public override void Occupate(PanelModel panelModel)
        {
            base.Occupate(panelModel);

            if (!_aiMovementStrategy.ReturnWhenLosingCombatZone)
                return;

            panelModel.OnReleased += OnPanelRealised;

            void OnPanelRealised(Guid guid)
            {
                panelModel.OnReleased -= OnPanelRealised;
                _aiMovementStrategy.LoseCapturePath();
            }
        }


        public PanelModel DecideMove()
        {
            return _aiMovementStrategy.Dicide();
        }
    }
}
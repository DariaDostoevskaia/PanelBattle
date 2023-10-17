using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Strategy;
using LegoBattaleRoyal.Strategy.Difficulty;
using LegoBattaleRoyal.Strategy.Easy;

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
                    break;

                case Difficulty.Hard:
                    break;
            }
        }

        public PanelModel DecideMove()
        {
            return _aiMovementStrategy.Dicide();
        }
    }
}
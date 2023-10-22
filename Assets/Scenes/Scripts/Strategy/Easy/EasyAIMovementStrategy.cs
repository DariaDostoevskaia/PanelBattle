using LegoBattaleRoyal.Panels.Models;
using System;

namespace LegoBattaleRoyal.Strategy.Easy
{
    public class EasyAIMovementStrategy : AIMovementStrategy
    {
        public EasyAIMovementStrategy(int blocksToCapture, GridPosition currentPosition, PanelModel[] panelModels, Guid ownerId)
            : base(blocksToCapture, currentPosition, panelModels, ownerId)
        {
        }

        public override PanelModel Dicide()
        {
            return UseRandomStrategy();
        }
    }
}
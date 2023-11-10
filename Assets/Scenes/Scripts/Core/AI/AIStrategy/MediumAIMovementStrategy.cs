using LegoBattaleRoyal.Core.Panels.Models;
using System;

namespace LegoBattaleRoyal.Core.AI.AIStrategy
{
    public class MediumAIMovementStrategy : AIMovementStrategy
    {
        public MediumAIMovementStrategy(int blocksToCapture, GridPosition currentPosition, PanelModel[] panelModels, Guid ownerId)
            : base(blocksToCapture, currentPosition, panelModels, ownerId)
        {
            ReturnWhenLosingCombatZone = true;
        }

        public override PanelModel Dicide()
        {
            if (TryUsePathfindingStrategy(out var panel))
                return panel;

            if (TryUseToCaptureStrategy(out panel))
                return panel;

            panel = UseRandomStrategy();
            return panel;
        }
    }
}
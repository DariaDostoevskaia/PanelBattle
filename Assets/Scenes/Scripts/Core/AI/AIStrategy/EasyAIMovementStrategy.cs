using LegoBattaleRoyal.Core.Panels.Models;
using System;

namespace LegoBattaleRoyal.Core.AI.AIStrategy
{
    public class EasyAIMovementStrategy : AIMovementStrategy
    {
        public EasyAIMovementStrategy(int blocksToCapture, GridPosition currentPosition, PanelModel[] panelModels, Guid ownerId)
            : base(blocksToCapture, currentPosition, panelModels, ownerId)
        {
            ReturnWhenLosingCombatZone = false;
        }

        public override PanelModel Dicide()
        {
            return UseRandomStrategy();
        }
    }
}
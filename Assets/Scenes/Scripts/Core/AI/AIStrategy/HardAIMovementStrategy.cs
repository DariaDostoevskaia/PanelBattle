using LegoBattaleRoyal.Core.Panels.Models;
using System;
using System.Linq;

namespace LegoBattaleRoyal.Core.AI.AIStrategy
{
    public class HardAIMovementStrategy : AIMovementStrategy
    {
        private readonly PanelModel[] _panelModels;

        public HardAIMovementStrategy(int blocksToCapture, GridPosition currentPosition, PanelModel[] panelModels, Guid ownerId)
              : base(blocksToCapture, currentPosition, panelModels, ownerId)
        {
            ReturnWhenLosingCombatZone = true;
            _panelModels = panelModels;
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

        private bool TryUseToCaptureStrategy(out PanelModel panelModel)
        {
            panelModel = null;

            var occupyPanelsCount = _panelModels.Count(panel => panel.IsOccupated(OwnerId));

            var ownCapturePanelsCount = _panelModels
                .Count(panel => panel.IsOccupated(OwnerId)
                && panel.IsCaptured(OwnerId));

            if (occupyPanelsCount >= BlocksToCapture)
            {
                CreateNewPathToHome();
                return TryUsePathfindingStrategy(out panelModel);
            }

            if (ownCapturePanelsCount < BlocksToCapture
                && occupyPanelsCount == 0)
            {
                LoseCapturePath();
                return TryUsePathfindingStrategy(out panelModel);
            }

            return false;
        }
    }
}
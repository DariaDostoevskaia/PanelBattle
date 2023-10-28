using LegoBattaleRoyal.Core.Panels.Models;
using System;
using System.Linq;

namespace LegoBattaleRoyal.Core.AI.AIStrategy
{
    public class MediumAIMovement : AIMovementStrategy
    {
        private int _blocksToCapture;
        private readonly PanelModel[] _panelModels;

        public MediumAIMovement(int blocksToCapture, GridPosition currentPosition, PanelModel[] panelModels, Guid ownerId)
            : base(blocksToCapture, currentPosition, panelModels, ownerId)
        {
            ReturnWhenLosingCombatZone = true;
            _blocksToCapture = blocksToCapture;
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

            var occupatePanels = _panelModels.Where(panelModel => panelModel.IsOccupated(OwnerId)).ToArray();

            if (occupatePanels.Length == _blocksToCapture)
            {
                panelModel = occupatePanels.First();
                CreateNewPathToHome();
                return true;
            }

            return false;
        }
    }
}
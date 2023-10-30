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
            GetCapturedPanels();

            if (TryUsePathfindingStrategy(out var panel))
                return panel;

            if (TryUseToCaptureStrategy(out panel))
                return panel;

            if (TryUseToCaptureZoneStrategy(out panel))
                return panel;

            panel = UseRandomStrategy();
            return panel;
        }

        private int GetCapturedPanels()
        {
            var capturePanels = _panelModels
                .Count(panelModel => panelModel != null
                && panelModel.IsCaptured(OwnerId));

            return capturePanels;
        }

        private bool TryUseToCaptureZoneStrategy(out PanelModel panelModel)
        {
            panelModel = null;

            var capturedPanels = _panelModels.Count(panelModel => panelModel.IsCaptured(OwnerId));

            if (capturedPanels < GetCapturedPanels())
            {
                CreateNewPathToHome();
                return TryUsePathfindingStrategy(out panelModel);
            }

            return false;
        }
    }
}
using LegoBattaleRoyal.Panels.Models;
using System;
using System.Linq;

namespace LegoBattaleRoyal.Strategy
{
    public abstract class AIMovementStrategy
    {
        private readonly int _maxX;

        private readonly int _maxY;

        public bool ReturnWhenLosingCombatZone { get; protected set; }

        protected Pathfinding Pathfinding { get; private set; }

        protected int BlocksToCapture { get; }

        protected GridPosition CurrentPosition { get; }

        protected PanelModel[] PanelModels { get; }

        protected Guid OwnerId { get; }



        public AIMovementStrategy(int blocksToCapture, GridPosition currentPosition,
            PanelModel[] panelModels, Guid ownerId)
        {
            BlocksToCapture = blocksToCapture;
            CurrentPosition = currentPosition;
            PanelModels = panelModels;
            OwnerId = ownerId;

            _maxX = panelModels.Max(panelModel => panelModel.GridPosition.Row);
            _maxY = panelModels.Max(panelModel => panelModel.GridPosition.Column);
        }

        public abstract PanelModel Dicide();

        public void RestoreCapturePath()
        {
            Pathfinding = null;
        }

        public void LoseCapturePath()
        {
            if (Pathfinding != null
                && Pathfinding.HasPath)
                return;

            CreateNewPathToHome();
        }

        protected bool TryUsePathfindingStrategy(out PanelModel panelModel)
        {
            panelModel = null;

            var position = Pathfinding?.Next();
            if (position == null)
                return false;

            panelModel = PanelModels.First(panelModel => panelModel.GridPosition.Equals(position));
            return true;

        }

        protected PanelModel UseRandomStrategy()
        {
            return PanelModels
                .Where(panelModel => panelModel.IsAvailable(OwnerId))
                .OrderBy(panelModel => Guid.NewGuid())
                .First();
        }

        protected void CreateNewPathToHome()
        {
            var capturedPanel = PanelModels.LastOrDefault(panelModel => panelModel.IsCaptured(OwnerId));
            if (capturedPanel == null)
                return;

            CreateNewPath(capturedPanel.GridPosition);
        }

        private void CreateNewPath(GridPosition target)
        {
            Pathfinding = new Pathfinding(0, 0, _maxX, _maxY);
            Pathfinding.FindPath(CurrentPosition, target, (position) =>
            {
                var panel = PanelModels
                .FirstOrDefault(panel => panel.GridPosition
                .Equals(position));

                return panel != null
                && panel.IsJumpBlock;
            });
        }
    }
}
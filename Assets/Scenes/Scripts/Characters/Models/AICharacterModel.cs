using LegoBattaleRoyal.Panels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LegoBattaleRoyal.Characters.Models
{
    public class PriorityQueue<T, P>
    {
        private List<(T item, P priority)> items = new();
        private IComparer<P> priorityComparer;

        public int Count => items.Count;

        public PriorityQueue(IComparer<P> priorityComparer = null)
        {
            this.priorityComparer = priorityComparer ?? Comparer<P>.Default;
        }

        public void Enqueue(T item, P priority)
        {
            items.Add((item, priority));
            var childIndex = items.Count - 1;

            while (childIndex > 0)
            {
                var parentIndex = (childIndex - 1) / 2;

                if (priorityComparer.Compare(items[childIndex].priority, items[parentIndex].priority) >= 0)
                    break;

                Swap(childIndex, parentIndex);
                childIndex = parentIndex;
            }
        }

        public T Dequeue()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("Queue is empty");

            var (item, _) = items[0];
            var lastIndex = items.Count - 1;
            items[0] = items[lastIndex];
            items.RemoveAt(lastIndex);

            lastIndex--;

            var parentIndex = 0;

            while (true)
            {
                var leftChildIndex = (parentIndex * 2) + 1;
                if (leftChildIndex > lastIndex)
                    break;

                var rightChildIndex = leftChildIndex + 1;
                if (rightChildIndex <= lastIndex && priorityComparer.Compare(items[rightChildIndex].priority, items[leftChildIndex].priority) < 0)
                    leftChildIndex = rightChildIndex;

                if (priorityComparer.Compare(items[parentIndex].priority, items[leftChildIndex].priority) <= 0)
                    break;

                Swap(parentIndex, leftChildIndex);
                parentIndex = leftChildIndex;
            }

            return item;
        }

        private void Swap(int indexA, int indexB)
        {
            (items[indexB], items[indexA]) = (items[indexA], items[indexB]);
        }
    }

    public class Pathfinding
    {
        private readonly int _minX;
        private readonly int _minY;
        private readonly int _maxX;
        private readonly int _maxY;

        private readonly List<GridPosition> _path;

        public bool HasPath => _path.Count > 0;

        public Pathfinding(int minX, int minY, int maxX, int maxY)
        {
            //TODO validation minX and minY    < max

            _minX = minX;
            _minY = minY;
            _maxX = maxX;
            _maxY = maxY;

            _path = new List<GridPosition>();
        }

        public GridPosition Next()
        {
            if (_path.Count == 0)
                return null;

            var next = _path[0];
            _path.Remove(next);

            return next;
        }

        public List<GridPosition> FindPath(GridPosition startPosition, GridPosition goal, Func<GridPosition, bool> isWalkable)
        {
            var openSet = new PriorityQueue<GridPosition, float>();
            var cameFrom = new Dictionary<GridPosition, GridPosition>();
            var gScore = new Dictionary<GridPosition, float>();

            openSet.Enqueue(startPosition, 0f);
            cameFrom[startPosition] = startPosition;
            gScore[startPosition] = 0f;

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current.Equals(goal))
                {
                    // Reconstruct the path.
                    _path.Clear();
                    while (!current.Equals(startPosition))
                    {
                        _path.Add(current);
                        current = cameFrom[current];
                    }

                    _path.Reverse();
                    return _path;
                }

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (!isWalkable(neighbor))
                        continue;

                    var tentativeGScore = gScore[current] + 1f; // Assuming constant cost for each step

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        var fScore = tentativeGScore + CalculateHeuristic(neighbor, goal);
                        openSet.Enqueue(neighbor, fScore);
                    }
                }
            }

            // No path found.
            return _path;
        }

        private List<GridPosition> GetNeighbors(GridPosition position)
        {
            var neighbors = new List<GridPosition>();

            for (var row = position.Row - 1; row <= position.Row + 1; row++)
            {
                for (var col = position.Column - 1; col <= position.Column + 1; col++)
                {
                    if (row >= _minY && row <= _maxY && col >= _minX && col <= _maxX && !(row == position.Row && col == position.Column))
                    {
                        neighbors.Add(new GridPosition(row, col));
                    }
                }
            }

            return neighbors;
        }

        private static int CalculateHeuristic(GridPosition a, GridPosition b)
        {
            return Math.Abs(a.Row - b.Row) + Math.Abs(a.Column - b.Column);
        }
    }


    public abstract class AIMovementStrategy
    {
        private readonly int _maxX;

        private readonly int _maxY;

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

    public class AICharacterModel : CharacterModel
    {
        private readonly AIMovementStrategy _aiMovementStrategy;

        public AICharacterModel(int jumpLenght, int blocksToCapture, ScriptableObjects.Difficulty difficulty, PanelModel[] panelModels)
            : base(jumpLenght)
        {
            switch (difficulty)
            {
                case ScriptableObjects.Difficulty.Easy:
                    _aiMovementStrategy = new EasyAIMovementStrategy(blocksToCapture, CurrentPosition, panelModels, Id);
                    break;

                case ScriptableObjects.Difficulty.Medium:
                    break;

                case ScriptableObjects.Difficulty.Hard:
                    break;
            }
        }

        public PanelModel DecideMove()
        {
            return _aiMovementStrategy.Dicide();
        }
    }

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
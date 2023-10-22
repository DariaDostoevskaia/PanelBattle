using LegoBattaleRoyal.Common;
using LegoBattaleRoyal.Panels.Models;
using System;
using System.Collections.Generic;

namespace LegoBattaleRoyal.Strategy
{
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
            if (minX < maxX
                && minY < maxY)
            {
                _minX = minX;
                _minY = minY;
                _maxX = maxX;
                _maxY = maxY;
            }
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

                    var tentativeGScore = gScore[current] + 1f;
                    // Assuming constant cost for each step

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
                    if (row >= _minY
                        && row <= _maxY
                        && col >= _minX
                        && col <= _maxX
                        && !(row == position.Row && col == position.Column))
                    {
                        neighbors.Add(new GridPosition(row, col));
                    }
                }
            }

            return neighbors;
        }

        private static int CalculateHeuristic(GridPosition a, GridPosition b)
        {
            return Math.Abs(a.Row - b.Row)
                + Math.Abs(a.Column - b.Column);
        }
    }
}
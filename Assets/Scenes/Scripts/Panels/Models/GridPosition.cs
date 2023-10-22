using System;

namespace LegoBattaleRoyal.Panels.Models
{
    public class GridPosition : IEquatable<GridPosition>
    {
        public int Row { get; private set; }

        public int Column { get; private set; }

        public GridPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool Equals(GridPosition other)
        {
            return other != null
                && Row == other.Row
                && Column == other.Column;
        }

        public override string ToString()
        {
            return $"{Row}:{Column}";
        }

        public void Change(GridPosition gridPosition)
        {
            Row = gridPosition.Row;
            Column = gridPosition.Column;
        }
    }
}
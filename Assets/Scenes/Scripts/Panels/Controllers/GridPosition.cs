namespace LegoBattaleRoyal.Panels.Controllers
{
    public class GridPosition
    {
        private int _neighborRow;
        private int _neighborColumn;
        public int Row { get; set; }
        public int Column { get; set; }

        public GridPosition(int neighborRow, int neighborColumn)
        {
            _neighborRow = neighborRow;
            _neighborColumn = neighborColumn;
        }
    }
}
namespace LegoBattaleRoyal.Panels.Controllers
{
    public class GridPosition
    {
        private int _neighborRow;
        private int _neighborColumn;

        public int Row => _neighborRow;

        public int Column => _neighborColumn;

        public GridPosition(int neighborRow, int neighborColumn)
        {
            _neighborRow = neighborRow;
            _neighborColumn = neighborColumn;
        }
    }
}
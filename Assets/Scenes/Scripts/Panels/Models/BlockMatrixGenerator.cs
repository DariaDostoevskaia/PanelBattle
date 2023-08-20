using System.Collections.Generic;

namespace LegoBattaleRoyal.Panels.Models
{
    public static class BlockMatrixGenerator
    {
        public static List<int[]> GenerateGrid(int[] rect)
        {
            var cells = new List<int[]>();
            var width = rect[0];
            var height = rect[1];

            for (int col = 0; col < width; col++)
            {
                var y = col;
                for (int row = 0; row < height; row++)
                {
                    var x = row;
                    cells.Add(new int[] { x, y });
                }
            }
            return cells;
        }

        public static List<float[]> GeneratePolygon(float[] startedPosition, int[] rect, float spacing)
        {
            var positions = new List<float[]>();
            var width = rect[0];
            var height = rect[1];

            var startedX = startedPosition[0];
            var startedZ = startedPosition[1];

            for (int col = 0; col < width; col++)
            {
                //проверить на правильность  - +
                // позиции должны быть от 0 до бесконечности - не в минус
                var z = startedZ + (col * (1 + spacing));
                for (int row = 0; row < height; row++)
                {
                    var x = startedX + (row * (1 + spacing));
                    positions.Add(new float[] { x, z });
                }
            }
            return positions;
        }
    }
}
using System;

namespace LegoBattaleRoyal.App.DTO
{
    public class LevelDTO
    {
        public int CurrentOrder { get; set; } = 1;

        public int[] FinishedOrders { get; set; } = Array.Empty<int>();
    }
}
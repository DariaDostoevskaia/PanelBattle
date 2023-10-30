using System;
using UnityEngine;

namespace LegoBattaleRoyal.App.DTO
{
    public class LevelDTO : MonoBehaviour
    {
        public int CurrentOrder { get; set; } = 1;

        public int[] FinishedOrders { get; set; } = Array.Empty<int>();
    }
}
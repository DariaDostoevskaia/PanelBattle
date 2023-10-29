using System;
using UnityEngine;

public class LevelDTO : MonoBehaviour
{
    public int CurrenOrder { get; set; } = 1;

    public int[] FinishedOrders { get; set; } = Array.Empty<int>();
}
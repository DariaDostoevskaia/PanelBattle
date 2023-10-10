using System;
using UnityEngine;

namespace LegoBattaleRoyal.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToColor(this Guid guid)
        {
            var hashCode = guid.GetHashCode();

            var r = Mathf.Abs((hashCode & 0xFF0000) >> 16) / 255f;
            var g = Mathf.Abs((hashCode & 0x00FF00) >> 8) / 255f;
            var b = Mathf.Abs(hashCode & 0x0000FF) / 255f;

            return new Color(r, g, b);
        }
    }
}
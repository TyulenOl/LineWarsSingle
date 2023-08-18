using UnityEngine;

namespace LineWars.Extensions
{
    public static class ColorExtension
    {
        public static Color WithAlpha(this Color color, float newAlpha)
        {
            return new Color(color.r, color.g, color.b, newAlpha);
        }
    }
}
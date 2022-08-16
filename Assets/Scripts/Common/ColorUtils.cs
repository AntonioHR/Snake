using System.Collections;
using UnityEngine;

namespace Common
{
    public static class ColorUtils
    {
        public static Color WithAlpha(this Color c, float alpha) => new Color(c.r, c.g, c.b, alpha);
    }
}
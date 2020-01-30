using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NerScript
{
    /// <summary>
    /// 拡張メソッド
    /// </summary>
    public static class ColorLib
    {
        #region Get
        public static Vector3 rgb(this Color color) => new Vector3(color.r, color.g, color.b);
        public static Vector4 rgba(this Color color) => new Vector4(color.r, color.g, color.b, color.a);

        #endregion

        #region Set
        /// <summary>
        /// 色の設定
        /// 設定しない場合は-1
        /// </summary>
        public static Color SetedColor(this Color c, float r, float g, float b, float a)
        {
            if (0 <= r)
            {
                c = new Color(r, c.g, c.b, c.a);
            }
            if (0 <= g)
            {
                c = new Color(c.r, g, c.b, c.a);
            }
            if (0 <= b)
            {
                c = new Color(c.r, c.g, b, c.a);
            }
            if (0 <= a)
            {
                c = new Color(c.r, c.g, c.b, a);
            }
            return c;
        }
        public static Color SetedRGB(this Color color, Vector3 rgb)
        {
            color.r = rgb.x;
            color.g = rgb.y;
            color.b = rgb.z;
            return color;
        }
        public static Color SetedRed(this Color c, float r)
        {
            c = new Color(r, c.g, c.b, c.a);
            return c;
        }
        public static Color SetedGreen(this Color c, float g)
        {
            c = new Color(c.r, c.g, c.b, c.a);
            return c;
        }
        public static Color SetedBlue(this Color c, float b)
        {
            c = new Color(c.r, c.g, b, c.a);
            return c;
        }
        public static Color SetedAlpha(this Color c, float a)
        {
            c = new Color(c.r, c.g, c.b, a);
            return c;
        }


        public static void SetRed(this ref Color c, float r) => c.r = r;
        public static void SetGreen(this ref Color c, float g) => c.g = g;
        public static void SetBlue(this ref Color c, float b) => c.b = b;
        public static void SetAlpha(this ref Color c, float a) => c.a = a;
        #endregion

        #region Add
        public static Color AddedColor(this Color c, float r, float g, float b, float a)
        {
            c = c + new Color(r, g, b, a);
            return c;
        }
        public static Color AddedRGB(this Color c, Vector3 rgb)
        {
            c = new Color(c.r + rgb.x, c.g + rgb.y, c.b + rgb.z, c.a);
            return c;
        }
        public static Color AddedRed(this Color c, float r)
        {
            c = new Color(c.r + r, c.g, c.b, c.a);
            return c;
        }
        public static Color AddedGreen(this Color c, float g)
        {
            c = new Color(c.r, c.g + g, c.b, c.a);
            return c;
        }
        public static Color AddedBlue(this Color c, float b)
        {
            c = new Color(c.r, c.g, c.b + b, c.a);
            return c;
        }
        public static Color AddedAlpha(this Color c, float a)
        {
            c = new Color(c.r, c.g, c.b, c.a + a);
            return c;
        }
        #endregion
    }
}
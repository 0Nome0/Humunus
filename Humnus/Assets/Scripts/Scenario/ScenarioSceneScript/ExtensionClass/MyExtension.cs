using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmyCustom
{
    public static class MyExtension
    {
        #region Color

        /// <summary>
        /// Redの変更
        /// </summary>
        /// <param name="c"></param>
        /// <param name="r"></param>
        public static Color Red(this Color c,float r)
        {
            return new Color(r, c.g, c.b, c.a);
        }

        /// <summary>
        /// Greenの変更
        /// </summary>
        /// <param name="c"></param>
        /// <param name="g"></param>
        public static Color Green(this Color c,float g)
        {
            return new Color(c.r, g, c.b, c.a);
        }

        /// <summary>
        /// Blueの変更
        /// </summary>
        /// <param name="c"></param>
        /// <param name="b"></param>
        public static Color Blue(this Color c,float b)
        {
            return new Color(c.r, c.g, b, c.a);
        }

        /// <summary>
        /// Alphaの変更
        /// </summary>
        /// <param name="c"></param>
        /// <param name="alpha"></param>
        public static Color Alpha(this Color c, float alpha)
        {
            return new Color(c.r, c.g, c.b, alpha);
        }

        /// <summary>
        /// 色の加算
        /// </summary>
        /// <param name="c">元の色</param>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">透過度</param>
        public static Color AddColor(this Color c,float r,float g,float b,float a)
        {
            return new Color(c.r + r, c.g + g, c.b + b, c.a + a);
        }

        /// <summary>
        /// 色の減算
        /// </summary>
        /// <param name="c">元の色</param>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">透過度</param>
        public static Color SubColor(this Color c, float r, float g, float b, float a)
        {
            return new Color(c.r - r, c.g - g, c.b - b, c.a - a);
        }

        /// <summary>
        /// 色の乗算
        /// </summary>
        /// <param name="c">元の色</param>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">透過度</param>
        public static Color MulColor(this Color c, float r, float g, float b, float a)
        {
            return new Color(c.r * r, c.g * g, c.b * b, c.a * a);
        }

        /// <summary>
        /// 色の除算
        /// </summary>
        /// <param name="c">元の色</param>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">透過度</param>
        public static Color DivColor(this Color c, float r, float g, float b, float a)
        {
            return new Color(c.r / r, c.g / g, c.b / b, c.a / a);
        }
        #endregion

        /// <summary>
        /// 先頭にあるオブジェクトを削除し、
        /// そのオブジェクトを返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Dequeue<T>(this IList<T> list)
        {
            T result = list[0];
            list.RemoveAt(0);
            return result;
        }

        /// <summary>
        /// 末尾にオブジェクトを追加する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public static void Enqueue<T>(this IList<T> list,T item)
        {
            list.Add(item);
        }

        /// <summary>
        /// 先頭にあるオブジェクトを削除せずに返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Peek<T>(this IList<T> list)
        {
            return list[0];
        }
    }
}

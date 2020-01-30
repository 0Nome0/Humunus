using UnityEngine;

namespace NerScript.Attribute
{
    /// <summary>
    /// 色付き描画
    /// </summary>
    public class ColoringAttribute : PropertyAttribute
    {
        public Color color;

        public ColoringAttribute(float r, float g, float b)
        {
            color = new Color(r, g, b);
        }
    }
}
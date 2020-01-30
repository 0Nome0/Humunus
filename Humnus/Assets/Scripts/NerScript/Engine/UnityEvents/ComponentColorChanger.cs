using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using NerScript.Attribute;


namespace NerScript.UI
{
    public class ComponentColorChanger : MonoBehaviour
    {
        [SerializeField] private Component target = null;
        [SerializeField] private MemberTypes memberType = MemberTypes.Field;
        [SerializeField, AdvancedSetting] private string memberName = "color";
        [SerializeField] private Color color = Color.black;
        [SerializeField] private List<Color> colors = null;
        private MemberInfo colorInfo = null;
        private BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        public Color targetColor
        {
            set {
                if (memberType == MemberTypes.Field) ((FieldInfo)colorInfo)?.SetValue(target, value);
                else if (memberType == MemberTypes.Property) ((PropertyInfo)colorInfo)?.SetValue(target, value);
            }
        }

        private void Awake()
        {
            Type type = target.GetType();
            colorInfo = type.GetMembers(flags).ToList()
                .Where(m => m.MemberType == memberType)
                .Where(m => m.Name.Contains(memberName))
                .ToList()[0];
        }

        private void OnValidate() { colors.InstanceOrNew().Fill(1)[0] = color; }

        public void Change() => targetColor = color;
        public void Change1() => targetColor = colors[1];
        public void Change2() => targetColor = colors[2];
        public void Change3() => targetColor = colors[3];
        public void Change4() => targetColor = colors[4];
        public void Change5() => targetColor = colors[5];
        public void Change(int i) => targetColor = colors[i];
        public void Change(Color color) => targetColor = color;
        public void Change(float r, float g, float b, float a = 1) => targetColor = new Color(r, g, b, a);
        public void ChangeRandom()
        {
            targetColor = RandomLib.Color();
        }
    }
}

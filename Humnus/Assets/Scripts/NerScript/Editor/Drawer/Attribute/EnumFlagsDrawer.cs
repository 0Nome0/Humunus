using UnityEngine;
using UnityEditor;

namespace NerScript.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public sealed class EnumFlagsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            prop.intValue = EditorGUI.MaskField(position, label, prop.intValue, prop.enumNames);
        }
    }
}
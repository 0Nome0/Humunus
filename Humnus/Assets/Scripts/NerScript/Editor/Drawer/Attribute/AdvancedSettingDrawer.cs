using UnityEditor;
using UnityEngine;


namespace NerScript.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(AdvancedSettingAttribute))]
    public class AdvancedSettingDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return 0; }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) { }
    }
}
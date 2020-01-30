using UnityEditor;
using UnityEngine;


namespace NerScript.Attribute.Editor
{
    /// <summary>
    /// 
    /// </summary>
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelOverrideDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var atrbte = (LabelAttribute)attribute;
            label.text = atrbte.label;
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
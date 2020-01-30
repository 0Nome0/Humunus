using UnityEditor;
using UnityEngine;


namespace NerScript.Attribute.Editor
{
    /// <summary>
    /// 表示されるが変更不可なSerializeField
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyOnInspectorAttribute))]
    public class ReadOnlyOnInspectorDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }




        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
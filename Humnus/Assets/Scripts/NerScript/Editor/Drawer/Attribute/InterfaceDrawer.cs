using UnityEditor;
using UnityEngine;


namespace NerScript.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(InterfaceAttribute))]
    public class InterfaceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }




        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var att = (InterfaceAttribute)attribute;

            if (property.propertyType != SerializedPropertyType.ObjectReference) return;

            EditorGUI.ObjectField(position, property);
            Object obj = property.objectReferenceValue;
            GameObject gobj = obj as GameObject;

            if (gobj == null) property.objectReferenceValue = null;

        }
    }
}
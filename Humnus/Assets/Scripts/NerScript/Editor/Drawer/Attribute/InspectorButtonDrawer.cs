using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using Object = UnityEngine.Object;

namespace NerScript.Attribute.Editor
{
    /// <summary>
    /// 
    /// </summary>
    [CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
    public class InspectorButtonDrawer : PropertyDrawer
    {
        static float addHeight = 4;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true) + addHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Boolean)
            {
                property.boolValue = false;
                return;
            }

            Object target = property.serializedObject.targetObject;
            var atrbte = (InspectorButtonAttribute)attribute;
            label.text = atrbte.name;
            position.height -= addHeight / 2;

            if (GUI.Button(position, label))
            {
                property.boolValue = true;
                BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance;
                MethodInfo member = target.GetType().GetMember(atrbte.method, flags)[0] as MethodInfo;
                member.Invoke(target, new object[] { });
            }
        }
    }
}
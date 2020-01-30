using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using NerScript;
using Object = UnityEngine.Object;

namespace NerScript.Attribute.Editor
{
    /// <summary>
    /// 
    /// </summary>
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (Show(property))
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            return -2;
        }

        public bool Show(SerializedProperty property)
        {
            Object target = property.serializedObject.targetObject;
            ShowIfAttribute atrbte = (ShowIfAttribute)attribute;
            BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance;
            MemberInfo[] members = target.GetType().GetMember(atrbte.boolName, flags);
            if (members == null || members.Length == 0) return atrbte.show = false;
            MemberInfo member = members[0];
            if (member is FieldInfo)
            {
                var b = ((FieldInfo)member).GetValue(target);
                if (b is bool)
                {
                    atrbte.show = (bool)b;
                }
            }
            else if (member is PropertyInfo)
            {
                var b = ((PropertyInfo)member).GetValue(target);
                if (b is bool)
                {
                    atrbte.show = (bool)b;
                }
            }
            else if (member is MethodInfo)
            {
                var b = ((MethodInfo)member).Invoke(target, new object[] { });
                if (b is bool)
                {
                    atrbte.show = (bool)b;
                }
            }
            return atrbte.show;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Show(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
}
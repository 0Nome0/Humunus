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
    [CustomPropertyDrawer(typeof(ReadOnlyIfAttribute))]
    public class ReadOnlyIfDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public bool ReadOnly(SerializedProperty property)
        {
            Object target = property.serializedObject.targetObject;
            ReadOnlyIfAttribute atrbte = (ReadOnlyIfAttribute)attribute;
            BindingFlags flags = ReflectionLib.flag;
            MemberInfo[] members = target.GetType().GetMember(atrbte.boolName, flags);
            if (members == null || members.Length == 0) return atrbte.readOnly = false;
            MemberInfo member = members[0];
            if (member is FieldInfo)
            {
                var b = ((FieldInfo)member).GetValue(target);
                if (b is bool)
                {
                    atrbte.readOnly = (bool)b;
                }
            }
            else if (member is PropertyInfo)
            {
                var b = ((PropertyInfo)member).GetValue(target);
                if (b is bool)
                {
                    atrbte.readOnly = (bool)b;
                }
            }
            else if (member is MethodInfo)
            {
                var b = ((MethodInfo)member).Invoke(target, new object[] { });
                if (b is bool)
                {
                    atrbte.readOnly = (bool)b;
                }
            }
            return atrbte.readOnly;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginDisabledGroup(ReadOnly(property));
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndDisabledGroup();

        }
    }
}
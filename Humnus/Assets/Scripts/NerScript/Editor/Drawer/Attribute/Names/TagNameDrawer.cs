using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

namespace NerScript.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(TagNameAttribute))]
    public sealed class TagNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // property.serializedObject.Update();

            if (property.propertyType != SerializedPropertyType.String)
            {
                //stringじゃなかったらReturn
                EditorGUI.LabelField(position, label.text + "(" + property.propertyType + ")");
                property.serializedObject.ApplyModifiedProperties();
                return;
            }

            property.stringValue = EditorGUI.TagField(position, label.text, property.stringValue);
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

namespace NerScript.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(LayerNameAttribute))]
    public sealed class LayerNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // property.serializedObject.Update();

            if (property.propertyType != SerializedPropertyType.Integer)
            {
                //stringじゃなかったらReturn
                EditorGUI.LabelField(position, label.text + "(" + property.propertyType + ")");
                property.serializedObject.ApplyModifiedProperties();
                return;
            }

            property.intValue = EditorGUI.LayerField(position, label.text, property.intValue);
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}

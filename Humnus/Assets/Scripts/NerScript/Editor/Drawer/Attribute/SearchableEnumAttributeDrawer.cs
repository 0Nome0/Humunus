using UnityEditor;
using UnityEngine;
using NerScript.Editor;
using System;
using System.Linq;

namespace NerScript.Attribute.Editor
{
    /// <summary>
    /// 表示されるが変更不可なSerializeField
    /// </summary>
    [CustomPropertyDrawer(typeof(SearchableEnumAttribute))]
    public class SearchableEnumAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }




        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                EditorGUI.PropertyField(position, property);
                return;
            }
            position.width /= 2;
            GUI.Label(position, label);
            position.x += position.width;
            if (GUI.Button(position, property.enumDisplayNames[property.enumValueIndex]))
            {
                SearchablePopupWindow<int>.Show(
                    "Name",
                    ListLib.CreateIntList(0, property.enumDisplayNames.Length - 1),
                    (s) =>
                    {
                        property.enumValueIndex = s;
                        property.serializedObject.ApplyModifiedProperties();
                    },
                    property.enumValueIndex,
                    onTtoString: (i) => property.enumDisplayNames[i]
                    );
            }
            GUILib.DropDownIcon(position);
        }
    }
}
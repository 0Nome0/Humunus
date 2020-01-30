using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

namespace NerScript.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(InputNameAttribute))]
    public sealed class InputNameDrawer : PropertyDrawer
    {
        private SerializedObject inputObject;
        private SerializedProperty axesProperty;

        public InputNameDrawer()
        {
            // InputManager.assetをシリアライズされたオブジェクトとして読み込む
            inputObject = new SerializedObject(AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/InputManager.asset"));
            axesProperty = inputObject.FindProperty("m_Axes");
        }

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


            List<string> inputNameList = new List<string>();
            int axisCount = axesProperty.arraySize;
            int selectIndex = 0;
            for (int i = 0; i < axisCount; ++i)
            {
                var axis = axesProperty.GetArrayElementAtIndex(i);
                var name = axis.FindPropertyRelative("m_Name");

                ///入っていなければ追加
                if (!inputNameList.Contains(name.stringValue)) inputNameList.Add(name.stringValue);
            }
            selectIndex = inputNameList.FindIndex(x => x == property.stringValue);
            selectIndex = Mathf.Max(0, selectIndex);
            selectIndex = EditorGUI.Popup(position, label.text, selectIndex, inputNameList.ToArray());
            property.stringValue = inputNameList[selectIndex];

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}

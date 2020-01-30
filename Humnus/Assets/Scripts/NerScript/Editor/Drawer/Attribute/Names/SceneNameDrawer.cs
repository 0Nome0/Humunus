using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;
using System.IO;

namespace NerScript.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(SceneNameAttribute))]
    public sealed class SceneNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // property.serializedObject.Update();

            if (property.propertyType != SerializedPropertyType.String)
            {
                return;
            }
            int selectIndex = 0;
            string[] names = new string[SceneManager.sceneCountInBuildSettings];
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
            {
                names[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (property.stringValue == names[i])
                {
                    selectIndex = i;
                }
            }
            selectIndex = EditorGUI.Popup(position, label.text, selectIndex, names);
            property.stringValue = names[selectIndex];
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
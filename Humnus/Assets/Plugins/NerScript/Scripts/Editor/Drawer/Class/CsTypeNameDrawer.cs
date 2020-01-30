using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NerScript.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomPropertyDrawer(typeof(CsTypeName))]
    public class CsTypeNameDrawer : PropertyDrawer
    {
        private string[] asms = null;
        private string[] types = null;
        private SerializedProperty assembleNameProperty = null;
        private SerializedProperty typeNameProperty = null;
        private SerializedProperty includeUnityProperty = null;
        private Assembly assembly = null;




        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (assembleNameProperty == null) assembleNameProperty = property.FindPropertyRelative("assemblyName");
            if (typeNameProperty == null) typeNameProperty = property.FindPropertyRelative("typeName");
            if (includeUnityProperty == null) includeUnityProperty = property.FindPropertyRelative("includeUnity");

            if (asms == null)
            {
                asms = AppDomain.CurrentDomain.GetAssemblies()
                       .Select(asms => asms.GetName().Name)
                       .Where(name =>
                               includeUnityProperty.boolValue ||
                               !name.StartsWithAny("Unity", "UnityEngine.", "UnityEditor."))
                       .ToArray();
                string asmName = assembleNameProperty.stringValue;
                if (asmName != "")
                {
                    assembly = AppDomain.CurrentDomain.GetAssemblies().First(asm => asm.GetName().Name == asmName);
                    types = assembly.GetTypes().Select(t => t.Name).ToArray();
                }
            }

            position.width /= 2;
            AssemblyNameField(position);
            position.x += position.width;
            TypeNameField(position);
        }

        private void AssemblyNameField(Rect position)
        {
            int index = asms.ToList().FindIndex(name => name == assembleNameProperty.stringValue);
            if (index < 0 || asms.Length <= index) index = 0;

            string asmName = asms[EditorGUI.Popup(position, "", index, asms)];

            if (assembleNameProperty.stringValue != asmName)
            {
                assembleNameProperty.stringValue = asmName;
                assembly = AppDomain.CurrentDomain.GetAssemblies().First(asm => asm.GetName().Name == asmName);
                types = assembly.GetTypes().Select(t => t.Name).ToArray();
            }
        }

        private void TypeNameField(Rect position)
        {
            if (assembly == null) return;
            int index = types.ToList().FindIndex(name => name == typeNameProperty.stringValue);
            if (index < 0 || types.Length <= index) index = 0;

            string typeName = types[EditorGUI.Popup(position, "", index, types)];

            if (typeNameProperty.stringValue != typeName)
            {
                typeNameProperty.stringValue = typeName;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;// * 2 + EditorGUIUtility.standardVerticalSpacing;
            //return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}

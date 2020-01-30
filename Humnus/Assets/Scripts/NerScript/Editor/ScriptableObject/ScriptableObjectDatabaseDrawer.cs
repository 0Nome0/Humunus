using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript.Attribute;
using Object = UnityEngine.Object;

namespace NerScript.Editor
{
    using UnityEditorInternal;
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(ScriptableObjectDatabase))]
    public class ScriptableObjectDatabaseDrawer : Editor
    {
        private ReorderableListHelper<ScriptableObject> reorder = null;
        private ReorderableListHelper<ScriptableObject> Reorder => reorder ?? GetInitializedHelper();
        private ScriptableObjectDatabase database = null;

        private void OnEnable()
        {
            database = (ScriptableObjectDatabase)target;
            ReLoadObjects();
        }

        public void ReLoadObjects()
        {
            if(!database.enable) return;
            List<ScriptableObject> list = GetScriptableObjects();
            foreach(var item in list)
            {
                if(!database.objects.Contains(item))
                {
                    database.objects.Add(item);
                }
            }
            database.objects.RemoveNullObject();
            NumberID();
            InitializeHelper();
        }

        public void ResetObjects()
        {
            database.objects.Clear();
            NumberID();
            InitializeHelper();
            database.enable = false;
        }

        public override void OnInspectorGUI()
        {
            if(!database.enable)
            {
                EnableButton();
                ReLoadObjects();
                return;
            }
            GUILib.ScriptableObjectField(database);
            GUILib.AssetField(database, "Database");
            DrawReorderableList();
            ButtonField();
        }

        private void EnableButton()
        {
            if(GUILayout.Button("Enable"))
            {
                Undo.RecordObject(database, "ToEnable");
                EditorUtility.SetDirty(database);
                database.enable = true;
            }
        }


        private ReorderableListHelper<ScriptableObject> GetInitializedHelper()
        {
            InitializeHelper();
            return reorder;
        }

        private void InitializeHelper()
        {
            reorder = new ReorderableListHelper<ScriptableObject>(
                database.objects, displayAddButton: false, displayRemoveButton: false);
            reorder.AddOnChangeCallback((list) => NumberID());
            reorder.AddDrawCallback(OnDrawReorderElement);
        }

        private void NumberID()
        {
            for(int i = 0; i < database.objects.Count(); i++)
            {
                ((IManagedByID)database.objects[i]).ID = i;
                EditorUtility.SetDirty(database.objects[i]);
            }
            EditorUtility.SetDirty(database);
        }

        private void DrawReorderableList()
        {
            Reorder.Draw();
        }

        private void ButtonField()
        {
            ReLoadButtonField();
            ReSetButtonField();
        }
        private void ReLoadButtonField()
        {
            if(GUILayout.Button("ReLoad"))
            {
                ReLoadObjects();
            }
        }
        private void ReSetButtonField()
        {
            if(GUILayout.Button("Reset"))
            {
                ResetObjects();
            }
        }

        private void OnDrawReorderElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.width = 100;
            GUI.Label(rect, Reorder.TList[index].name);
            rect.x += rect.width;

            rect.width = 30;
            GUI.Label(rect, "Data");
            rect.x += rect.width;

            EditorGUI.BeginDisabledGroup(true);
            rect.width = 60;
            EditorGUI.ObjectField(rect, Reorder.TList[index], typeof(ScriptableObject), false);
            rect.x += rect.width;
            EditorGUI.EndDisabledGroup();

            rect.x += 10;

            rect.width = 20;
            GUI.Label(rect, "ID");
            rect.x += rect.width;

            EditorGUI.BeginDisabledGroup(true);
            rect.width = 50;
            EditorGUI.IntField(rect, "", ((IManagedByID)Reorder.TList[index]).ID);
            rect.x += rect.width; //!
            EditorGUI.EndDisabledGroup();
        }




        public string SelfPath => AssetDatabase.GetAssetPath(database);

        public List<ScriptableObject> GetScriptableObjects()
        {
            return
            AssetDatabase.FindAssets("", new string[] { SelfPath.GetDirectoryPath() })
                         .Select(guids => AssetDatabase.GUIDToAssetPath(guids))
                         .Distinct()
                         .Select(path =>
                          AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject)) as ScriptableObject)
                         .Where(asset => asset != null && asset is IManagedByID && asset != database)
                         .ToList();
        }
    }
}

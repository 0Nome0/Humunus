using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System;

namespace NerScript.Editor
{
    using Editor = UnityEditor.Editor;



    [CustomEditor(typeof(TransformMemory))]
    public class TransformMemoryDrawer : Editor
    {
        private TransformMemory memory = null;

        public override void OnInspectorGUI()
        {
            memory = (TransformMemory)target;

            GUILib.MonoBehaviourField(memory);

            if (!HasMemorys) { NewMemorys(); return; }


            SelectObjects();
            for (int i = 0; i < memory.MemoryCount; i++)
            {
                DrawMemorys(i);
            }
            NewMemoryField();
        }


        private bool HasMemorys => 0 <= memory.gameObjects.Count;
        private void NewMemorys()
        {
            if (SelectionLib.HasSelectedGameObjects)
            {
                if (GUILayout.Button("+ NewMemorys"))
                {
                    memory.SetGameObject(SelectionLib.SelectedGameObjects.ToList());
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Please Select GameObjects", MessageType.Info);
            }
        }
        private void SelectObjects()
        {
            if (GUILayout.Button("SelectObjects"))
            {
                SelectionLib.SelectedGameObjects = memory.gameObjects.ToArray();
                //WindowRepainter.RepaintHierarchy();
            }
        }
        private void DrawMemorys(int index)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            MemoryNameField(index);
            GUILayout.BeginHorizontal();


            if (GUILayout.Button("Orverride", GUILayout.Width(70)))
            {
                if (EditorUtility.DisplayDialog("TransformMemory", "現在の状態で上書きしますか？", "Yes", "No"))
                {
                    memory.Save(index);
                }
            }
            if (GUILayout.Button("Load"))
            {
                memory.Load(index);
            }
            if (GUILayout.Button("Delete", GUILayout.Width(50)))
            {
                if (EditorUtility.DisplayDialog("TransformMemory", "状態を削除してよろしいですか？", "Yes", "No"))
                {
                    memory.memoryGroup.RemoveAt(index);
                }
            }


            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        private void MemoryNameField(int index)
        {
            TransformMemory.Memorys memorys = memory.memoryGroup[index];
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                string name = EditorGUILayout.TextField("name", memorys.name);
                if (change.changed)
                {
                    UndoRecord("Change memoryName");
                    memorys.name = name;
                    memory.memoryGroup[index] = memorys;
                }
            }
        }

        private void NewMemoryField() { if (GUILayout.Button("+ NewMemoy")) { memory.AddMemory(); } }


        private void UndoRecord(string msg) { Undo.RecordObject(memory, msg); }
    }
}
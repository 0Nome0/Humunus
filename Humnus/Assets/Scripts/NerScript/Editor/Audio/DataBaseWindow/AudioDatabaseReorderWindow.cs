using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using NerScript;
using NerScript.Editor;
using NerScript.Anime.Builder;
using System;
using System.Linq;
using Object = UnityEngine.Object;


namespace NerScript.Resource.Editor
{
    public class AudioDatabaseReorderWindow : ReorderWindow<AudioData>
    {
        public static void ShowWindow(Object obj, List<AudioData> datas)
        {
            AudioDatabaseReorderWindow w = GetWindow<AudioDatabaseReorderWindow>("AudioDatabaseReorderWindow");

            Info info = new Info() { obj = obj, list = datas };
            w.Initialize(info);
        }

        protected void Initialize(Info info)
        {
            reorder = new ReorderableListHelper<AudioData>(info.list, true, false, false);
            reorder.AddDrawHeaderCallback((rect) => { EditorGUI.LabelField(rect, "Animations"); });
            reorder.AddElementBGCallback(DrawElementBackground);
            reorder.AddDrawCallback(DrawElement);
            reorder.AddOnChangeCallback(OnChanged);
            Init(reorder, info);
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }

        private void DrawElementBackground(Rect rect, int index, bool isActive, bool isFocused)
        {
            Texture2D tex = new Texture2D(1, 1);
            if (isFocused)
            {
                tex.SetPixel(0, 0, new Color(0.8f, 0.8f, 1, 0.5f));
            }
            else if (index % 2 == 0)
            {
                tex.SetPixel(0, 0, new Color(0.8f, 0.8f, 0.8f, 0.5f));
            }
            else
            {
                tex.SetPixel(0, 0, new Color(1f, 0.8f, 0.8f, 0.5f));
            }
            tex.Apply();
            GUI.DrawTexture(rect, tex as Texture);
        }
        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            using (new EditorGUI.DisabledScope())
            {
                rect.width /= 2;
                EditorGUI.LabelField(rect, new GUIContent($"({index + 1})" + List[index].name));
                rect.x += rect.width;
                EditorGUI.ObjectField(rect, "", List[index].clip, typeof(AudioClip), false);
            }
        }

        private void OnChanged(ReorderableList list) { }
    }
}
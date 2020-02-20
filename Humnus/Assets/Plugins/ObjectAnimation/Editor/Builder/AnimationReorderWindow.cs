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


namespace NerScript.Anime.Builder.Editor
{
    public class AnimationReorderWindow : ReorderWindow<BuilderObjectAnim>
    {
        public static void ShowWindow(Object obj, List<BuilderObjectAnim> anims)
        {
            AnimationReorderWindow w = GetWindow<AnimationReorderWindow>();

            Info info = new Info() { obj = obj, list = anims };
            w.Initialize(info);
        }

        protected void Initialize(Info info)
        {
            reorder = new ReorderableListHelper<BuilderObjectAnim>(info.list, displayHeader: false);
            reorder.AddDrawHeaderCallback((rect) => { EditorGUI.LabelField(rect, "Animations"); });
            reorder.AddHeightCallback(GetElementHeight);
            reorder.AddElementBGCallback(DrawElementBackground);
            reorder.AddDrawCallback(DrawElement);
            reorder.AddOnAddCallback(AddButton);
            reorder.AddOnChangeCallback(OnChanged);
            Init(reorder, info);
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }


        protected override float GetElementHeight(int index) => elementHeight * 2;

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
            GUI.DrawTexture(rect, tex);
        }
        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y -= elementHeight / 2;
            EditorGUI.LabelField(rect, new GUIContent($"({index + 1})" + List[index].type));
            rect.y += elementHeight;
            EditorGUI.LabelField(rect, new GUIContent($"({index + 1})" + List[index].type));
        }

        private void AddButton(ReorderableList list)
        {
            AnimationType[] animTypes = (AnimationType[])Enum.GetValues(typeof(AnimationType));
            var types = animTypes.Where(t => t != AnimationType.None);

            adding = true;

            SearchablePopupWindow<AnimationType>.Show("AnimationType", types.ToList(), type =>
            {
                UndoRecord("AddAnim");
                List.Add(new BuilderObjectAnim(type));
                maxSize = minSize = GetWindowSize();
            });
        }

        private void OnChanged(ReorderableList list)
        {

        }
    }
}
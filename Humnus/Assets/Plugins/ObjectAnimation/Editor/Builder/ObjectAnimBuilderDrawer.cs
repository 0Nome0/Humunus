using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Object = UnityEngine.Object;
using NerScript.Editor;

namespace NerScript.Anime.Builder.Editor
{
    [CustomEditor(typeof(ObjectAnimBuilder))]
    [CanEditMultipleObjects]
    public class ObjectAnimBuilderDrawer : UnityEditor.Editor
    {
        public static BuilderObjectAnim copy = null;

        public static string nullAnimName = "-null-";

        public ObjectAnimBuilder builder;
        public ListIndexSelector indexSelector = null;


        public ObjectAnimBuilderDrawer()
        {
            indexSelector = new ListIndexSelector(new List<int>());

        }


        public override void OnInspectorGUI()
        {
            //serializedObject.Update();
            ScriptField();
            builder = (ObjectAnimBuilder)target;
            indexSelector.List = builder.anims;
            BuildedAnimField();
            Draw();
        }

        private void ScriptField()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script",
                MonoScript.FromMonoBehaviour((ObjectAnimBuilder)target), typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();
        }
        private void BuildedAnimField()
        {
            if (builder.buildedAnim == null && 0 < builder.anims.Count) return;
            ObjectAnimBuilder anim = (ObjectAnimBuilder)EditorGUILayout.ObjectField(
                "Reference", builder.buildedAnim, typeof(ObjectAnimBuilder), true);
            if (anim != builder.buildedAnim)
            {
                UndoRecord("BuildedAnim");
                builder.buildedAnim = anim;
                if (anim != null)
                {
                    ReferenceAnimation(builder.buildedAnim, builder);
                }
            }
            if (builder.buildedAnim != null && GUILayout.Button("ReferenceAnimation"))
            {
                ReferenceAnimation(builder.buildedAnim, builder);
            }
        }
        private void ReferenceAnimation(ObjectAnimBuilder original, ObjectAnimBuilder pref)
        {
            pref.animName = original.animName;
            pref.anims = new List<BuilderObjectAnim>();
            foreach (var anim in original.anims)
            {
                pref.anims.Add(anim.GetCopy());
            }
            pref.onEnd = original.onEnd;
        }

        private void Draw()
        {
            if (builder.animName == nullAnimName)
            {
                EditorGUILayout.HelpBox("ObjectAnimation is not set.", MessageType.Info);
                if (GUILayout.Button("＋新規作成"))
                {
                    builder.animName = "new Animation";
                }
                return;
            }
            DrawAnimations();
        }
        private void DrawAnimations()
        {
            bool usingBuildedAnim = builder.buildedAnim != null;
            if (usingBuildedAnim) EditorGUI.BeginDisabledGroup(true);
            DrawName();
            DrawAnims();
            if (usingBuildedAnim) EditorGUI.EndDisabledGroup();
        }
        private void DrawName()
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                string name = EditorGUILayout.TextField("Anim Name", builder.animName);
                if (change.changed)
                {
                    Undo.RecordObject(builder, "ChangeAnimName");
                    builder.animName = name;
                }
            }
        }
        private void DrawAnims()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    DrawTitle();
                    AnimationCommandButton();
                }
                if (IsShowAnimations) return;
                DrawAnimationsGUI();
                DrawAnimIOButtons();
            }
        }

        private void DrawTitle()
        {
            string title = "ObjectAnimation" + $" {ObjectAnimation.Version}";
            if (builder.showAnims) title = "▼" + title;
            else title = "▶" + title;

            bool usingBuildedAnim = builder.buildedAnim != null;
            if (usingBuildedAnim) EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(title, GUIStyle.none)) { builder.showAnims = !builder.showAnims; }
            GUILayout.FlexibleSpace();
            if (usingBuildedAnim) EditorGUI.BeginDisabledGroup(true);
        }
        private void AnimationCommandButton()
        {
            if (GUILayout.Button(EditorGUIUtility.IconContent("_Popup"), GUIStyle.none))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem("All FoldOut", () => { builder.anims.ForEach(a => a.foldOut = true); });
                menu.AddItem(copy == null, "Add Copied Anim", () => { UndoRecord("AddCopiedAnim"); builder.anims.Add(copy); });
                menu.AddItem("Remove All Anim", () => { UndoRecord("Remove All Anim"); builder.anims.Clear(); });
                menu.AddItem("Clear AnimBuilder", () => { UndoRecord("Clear AnimBuilder"); builder.Init(nullAnimName); });
                menu.AddItem(new GUIContent("Use OnEnd"), builder.onEnd.enabled, () => { builder.onEnd.enabled = !builder.onEnd.enabled; });
                menu.AddItem("ApplyAnimation", () => ApplyAnimation());
                menu.ShowAsContext();
            }
        }
        private void ApplyAnimation()
        {
            var refs = FindObjectsOfType<ObjectAnimBuilder>().Where(b => b.buildedAnim == builder);
            foreach (var reference in refs) { ReferenceAnimation(builder, reference); }
        }
        private bool IsShowAnimations => !builder.showAnims;
        private void DrawAnimationsGUI()
        {
            Color def = GUI.backgroundColor;
            for (int i = 0; i < builder.anims.Count + (builder.onEnd.enabled ? 1 : 0); i++)
            {
                OnAnimationGUI(builder[i], i);
                GUI.backgroundColor = def;
            }
            GUI.backgroundColor = def;
        }
        private void DrawAnimIOButtons()
        {
            GUILib.Horizontal(() =>
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("＋アニメの追加")) { OnAddAnim(); }
                if (GUILayout.Button("－削除")) { OnRemoveAnim(); }
                if (GUILayout.Button("並替")) { OnReorderAnim(); }
            }, 0);
        }
        private void OnAddAnim()
        {
            var types =
                ((AnimationType[])Enum.GetValues(typeof(AnimationType)))
                .Where(t => t != AnimationType.None);

            SearchablePopupWindow<AnimationType>.Show("AnimationType", types.ToList(), type =>
            {
                UndoRecord($"Add{type.ToString()}Anim");
                builder.anims.Add(new BuilderObjectAnim(type));
            });
        }
        private void OnRemoveAnim()
        {
            builder.anims.RemoveAtI(indexSelector.SelectIndex);
            indexSelector.Optimize();
        }
        private void OnReorderAnim()
        {
            AnimationReorderWindow.ShowWindow(builder, builder.anims);
        }

        private void OnAnimationGUI(BuilderObjectAnim anim, int index)
        {
            DrawAnimBoxBackground(index);
            DrawAnimBox(anim, index);
            SelectAnimBox(index);
        }
        private void DrawAnimBoxBackground(int index)
        {
            if (indexSelector.Contains(index))
            {
                if (builder.anims.Count != 1) GUI.backgroundColor *= new Color(0.91f, 0.91f, 1f);
            }
            else if (index % 2 == 0)
            {
                GUI.backgroundColor *= new Color(0.9f, 0.9f, 0.9f);
            }
            else if (index % 2 == 1)
            {
                GUI.backgroundColor *= new Color(1, 0.95f, 0.95f);
            }
        }
        private void DrawAnimBox(BuilderObjectAnim anim, int index)
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                var info = new AnimationDrawInfo(builder, this);
                AnimationGUI.GUIs[anim.type].Invoke(index, info, serializedObject, AnimDrawOption.Null);
            }
        }
        private void SelectAnimBox(int index)
        {
            bool isMouseHoverBox = GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition);
            bool click = Event.current.type == EventType.MouseDown;
            if (isMouseHoverBox && click)
            {
                indexSelector.Select(index);
                Repaint();
            }
        }

        private void UndoRecord(string msg) { Undo.RecordObject(builder, msg); }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using Object = UnityEngine.Object;

namespace NerScript.Editor
{
    public static class GUILib
    {
        public static void Repaint() { EditorWindow.focusedWindow.Repaint(); }

        public static bool GetKey(KeyCode key)
        {
            Event e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == key) return true;
            return false;
        }

        public static class Load
        {
            public static GUISkin Skin(string path) => AssetDatabase.LoadAssetAtPath<GUISkin>(path);
            public static Texture Texture(string path) => EditorGUIUtility.Load(path) as Texture;
            public static Texture2D Texture2D(string path) => EditorGUIUtility.Load(path) as Texture2D;
        }

        #region =====Style,Skin,Images=============================================================

        public static class Skins
        {
            public const string PATH = "Assets/Editor/NerEditor/Skins/";
            public static GUISkin NerSkin => nerSkin ?? (nerSkin = Load.Skin(PATH + "NerSkin/NerSkin.guiskin"));
            private static GUISkin nerSkin = null;
        }

        public static class Styles
        {
            //public static readonly GUIStyle CancelButton = "ToolbarSeachCancelButton";
            //public static readonly GUIStyle DisabledCancelButton = "ToolbarSeachCancelButtonEmpty";
            //public static readonly GUIStyle SearchBox = "ToolbarSeachTextField";
            //public static readonly GUIStyle Selection = "SelectionRect";
            //public static readonly GUIStyle foldOut = "ShurikenModuleTitle";

            public static GUIStyle FlatTab => flatTab ?? (flatTab = Skins.NerSkin.GetStyle("tab"));
            private static GUIStyle flatTab = null;
            public static GUIStyle FlameTab => flameTab ?? (flameTab = Skins.NerSkin.GetStyle("Tab2"));
            private static GUIStyle flameTab = null;

            public static GUIStyle SelectableField => selectable ?? (selectable = Skins.NerSkin.GetStyle("SelectableField"));
            private static GUIStyle selectable = null;

            public static GUIStyle ExpandLabel => expandLabel ?? (expandLabel = GUI.skin.label.Expand());
            private static GUIStyle expandLabel = null;

            public static GUIStyle None => none ?? (none = GUIStyle.none);
            private static GUIStyle none = null;
        }

        #region GUIStyleExtends
        public static GUIStyle Copy(this GUIStyle style) => new GUIStyle(style);
        public static GUIStyle SetFontSize(this GUIStyle style, int fontSize)
        {
            style.fontSize = fontSize;
            return style;
        }
        public static GUIStyle SetMargin(this GUIStyle style, int margin)
        {
            style.margin = new RectOffset(margin, margin, margin, margin);
            return style;
        }
        public static GUIStyle SetMargin(this GUIStyle style, int marginLR, int marginTB)
        {
            style.margin = new RectOffset(marginLR, marginLR, marginTB, marginTB);
            return style;
        }
        public static GUIStyle SetMargin(this GUIStyle style, RectOffset margin)
        {
            style.margin = margin;
            return style;
        }
        public static GUIStyle SetPadding(this GUIStyle style, int padding)
        {
            style.padding = new RectOffset(padding, padding, padding, padding);
            return style;
        }
        public static GUIStyle SetPadding(this GUIStyle style, int paddingLR, int paddingTB)
        {
            style.margin = new RectOffset(paddingLR, paddingLR, paddingTB, paddingTB);
            return style;
        }
        public static GUIStyle SetPadding(this GUIStyle style, RectOffset padding)
        {
            style.padding = padding;
            return style;
        }
        public static GUIStyle Expand(this GUIStyle style)
        {
            style.padding = new RectOffset(0, 0, 0, 0);
            style.margin = new RectOffset(0, 0, 0, 0);
            return style;
        }
        public static GUIStyle SetTextColor(this GUIStyle style, Color color)
        {
            style.normal = new GUIStyleState()
            {
                background = style.normal.background,
                textColor = color,
                scaledBackgrounds = style.normal.scaledBackgrounds
            };
            return style;
        }
        public static GUIStyle SetTextClip(this GUIStyle style)
        {
            style.clipping = TextClipping.Clip;
            return style;
        }
        public static GUIStyle SetFixedHeight(this GUIStyle style, float fixedHeight)
        {
            style.fixedHeight = fixedHeight;
            return style;
        }
        #endregion

        public static class Images
        {
            public static Texture2D DropDown => dropDown ?? (dropDown = Load.Texture2D("icons/icon dropdown.png"));
            private static Texture2D dropDown = null;
        }

        public class GUISkinScope : IDisposable
        {
            GUISkin old;
            public GUISkinScope(GUISkin skin) { old = GUI.skin; GUI.skin = skin; }
            public void Dispose() { GUI.skin = old; }
        }


        #endregion ================================================================================

        public static void ChangeCheck(Object obj, string msg, Func<Object, object> gui, Action<object, Object> apply)
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                object value = gui(obj);
                if (change.changed)
                {
                    Undo.RecordObject(obj, msg);
                    apply(value, obj);
                }
            }
        }


        #region LabelWidth
        private static float labelWidth = 0.0f;
        public static void SetLabelWidth(float width, bool memory = true)
        {
            if (memory) labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = width;
        }
        public static void ReSetLabelWidth() => EditorGUIUtility.labelWidth = labelWidth;
        #endregion

        #region Horizontal
        public static void Horizontal(Action act)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                act();
                GUILayout.FlexibleSpace();
            }
        }

        public static void Horizontal(Action act, float space)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Space(space);
                act();
                GUILayout.Space(space);
            }
        }
        #endregion

        #region Menu

        public static void AddItem(this GenericMenu menu, string content, GenericMenu.MenuFunction func)
        {
            menu.AddItem(new GUIContent(content), false, func);
        }
        public static void AddItem(this GenericMenu menu, bool disable, string content, GenericMenu.MenuFunction func)
        {
            if (disable) menu.AddDisabledItem(new GUIContent(content));
            else menu.AddItem(new GUIContent(content), false, func);
        }

        #endregion

        #region Color
        public static void Coloring(Color color, Action act)
        {
            Color c = GUI.color;
            GUI.color = color;
            act?.Invoke();
            GUI.color = c;
        }
        public static void BackgroundColoring(Color color, Action act)
        {
            Color c = GUI.backgroundColor;
            GUI.backgroundColor = color;
            act?.Invoke();
            GUI.backgroundColor = c;
        }
        public static void ContentColoring(Color color, Action act)
        {
            Color c = GUI.contentColor;
            GUI.contentColor = color;
            act?.Invoke();
            GUI.contentColor = c;
        }

        public class ColoringScope : IDisposable
        {
            Color old;
            public ColoringScope(Color color) { old = GUI.color; GUI.color = color; }
            public void Dispose() { GUI.color = old; }
        }
        public class BackgroundColoringScope : IDisposable
        {
            Color old;
            public BackgroundColoringScope(Color color, bool changeColor = true)
            {
                old = GUI.backgroundColor;
                if (!changeColor) return;
                GUI.backgroundColor = color;
            }
            public void Dispose() { GUI.backgroundColor = old; }
        }
        public class ContentColoringScope : IDisposable
        {
            Color old;
            public ContentColoringScope(Color color) { old = GUI.contentColor; GUI.contentColor = color; }
            public void Dispose() { GUI.contentColor = old; }
        }
        #endregion

        #region Field

        public static void ScriptableObjectField<T>(T obj) where T : ScriptableObject
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(obj), typeof(T), false);
            EditorGUI.EndDisabledGroup();
        }
        public static void MonoBehaviourField<T>(T obj) where T : MonoBehaviour
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(obj), typeof(T), false);
            EditorGUI.EndDisabledGroup();
        }
        public static void AssetField<T>(T obj, string label) where T : Object
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(label, obj, typeof(T), false);
            EditorGUI.EndDisabledGroup();
        }
        public static (bool change, int value) CrementalIntField(int value, Action onClick = null)
        {
            bool change = false;
            float width = 20;
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("◀", GUI.skin.button.Copy().SetFontSize(12), GUILayout.Width(width))) { value--; change = true; }
                if (GUILayout.Button(value.ToString())) { onClick?.Invoke(); }
                if (GUILayout.Button("▶", GUI.skin.button.Copy().SetFontSize(13), GUILayout.Width(width))) { value++; change = true; }
            }
            value.ClampMin(0);
            return (change, value);
        }
        public static bool OptimizedListField<T>(List<T> list, string label) where T : Object
        {
            var (change, count) = CrementalIntField(list.Count);
            if (change) list.SetCount(count);

            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                string str = item.ToString();
                using (var changeScope = new EditorGUI.ChangeCheckScope())
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUIUtility.labelWidth /= 4;
                        item = (T)EditorGUILayout.ObjectField(label, item, typeof(T), true);
                        str = EditorGUILayout.TextField("name", str);
                        EditorGUIUtility.labelWidth *= 4;
                    }
                    if (changeScope.changed)
                    {
                        list[i] = item;
                        change = true;
                    }
                }
            }
            return change;
        }

        public static void DropDownIcon(float size = 20)
        {
            Rect btnPos = GUILayoutUtility.GetLastRect();
            btnPos.x += btnPos.width - size;
            btnPos.y += (btnPos.height) / 2 - size / 3;
            btnPos.width = btnPos.height = size;

            GUIContent content = new GUIContent(Images.DropDown);
            GUI.Label(btnPos, content, GUIStyle.none);
        }

        public static void DropDownIcon(Rect lastRect, float size = 20)
        {
            lastRect.x += lastRect.width - size;
            lastRect.y += (lastRect.height) / 2 - size / 3;
            lastRect.width = lastRect.height = size;

            GUIContent content = new GUIContent(Images.DropDown);
            GUI.Label(lastRect, content, GUIStyle.none);
        }

        #endregion
    }
}

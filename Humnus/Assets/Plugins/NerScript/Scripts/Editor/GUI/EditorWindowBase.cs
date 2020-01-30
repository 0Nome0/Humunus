using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript;
using Object = UnityEngine.Object;

namespace NerScript.Editor
{
    public interface IEditorWindowMediator
    {
        Object recordable { get; }
    }


    public abstract class EditorWindowBase<T> : EditorWindow where T : EditorWindowBase<T>
    {
        protected Object recordable = null;
        protected static T window = null;
        protected bool dirty = false;


        public abstract string WindowTitle { get; }

        protected static class EditorWindowData
        {
            public static bool UpdateRepaint { get; set; } = false;
            public static int currentInterval = 0;

            public static float Width => window.position.width;
            public static float Height => window.position.height;
            public static Vector2 Center => new Vector2(Width / 2, Height / 2);
            public static Rect rect => new Rect(Vector2.zero, Center * 2);
        }

        protected static class EditorWindowOption
        {
            public static bool UpdateRepaint { get; set; } = true;
            public static int UpdateRepaintInterval
            {
                get => updateRepaintInterval;
                set => updateRepaintInterval = value.ClampMin(0);
            }
            private static int updateRepaintInterval = 0;
        }



        public static void Open(IEditorWindowMediator windowMediator = null)
        {
            window = GetWindow<T>("");
            window.InitBase(windowMediator);
            window.Init(windowMediator);
        }
        private void InitBase(IEditorWindowMediator windowMediator)
        {
            titleContent = new GUIContent(WindowTitle);
            recordable = windowMediator?.recordable;
            OnEnable();
        }

        protected abstract void Init(IEditorWindowMediator windowMediator);
        protected abstract void OnGUI();
        private void OnEnable()
        {
            window = (T)this;
            OnEnabled();
        }
        protected virtual void OnEnabled() { }

        private void Update()
        {
            if (!EditorWindowOption.UpdateRepaint) return;
            if (EditorWindowOption.UpdateRepaintInterval <= EditorWindowData.currentInterval)
            {
                RepaintWindow();
                EditorWindowData.currentInterval = 0;
            }
            else
            {
                EditorWindowData.currentInterval++;
            }
        }

        private void RepaintWindow()
        {
            EditorWindowData.UpdateRepaint = true;
            Repaint();
            EditorWindowData.UpdateRepaint = false;
        }

        protected void DrawBackGround(Color color)
        {
            GUILib.Coloring(color, () => GUI.Box(EditorWindowData.rect, ""));
        }




        protected void RecordObject(string msg) { Undo.RecordObject(recordable, msg); SetWindowDirty(); }
        protected void SetWindowDirty() { EditorUtility.SetDirty(recordable); dirty = true; }
    }
}
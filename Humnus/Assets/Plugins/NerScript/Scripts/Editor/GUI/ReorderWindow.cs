using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript;
using System;
using System.Linq;
using Object = UnityEngine.Object;



namespace NerScript.Editor
{
    public class ReorderWindow<T> : EditorWindow
    {
        protected static ReorderWindow<T> window = null;

        protected ReorderableListHelper<T> reorder = null;
        public class Info
        {
            public Object obj = null;
            public List<T> list = null;
        }
        protected Info info = null;
        protected List<T> List => info.list;

        protected float width = 300;
        protected float elementHeight = 21.0f;
        protected float scroll = 0;
        protected bool adding = false;


        protected void Init(ReorderableListHelper<T> _reorder, Info _info)
        {
            window = this;
            reorder = _reorder;
            info = _info;
            maxSize = minSize = GetWindowSize();
        }

        protected Vector2 GetWindowSize() => new Vector2(width, (List.Count + 1) * elementHeight * 2).Clamp().ClampMaxY(650);
        protected virtual float GetElementHeight(int index) => elementHeight;

        public virtual void OnGUI()
        {
            if (ShouldClose()) Close();
            AutoScroll();
            DrawReorder();
        }

        public void DrawReorder()
        {
            using (var scrollView = new GUILayout.ScrollViewScope(new Vector2(0, scroll)))
            {
                scroll = scrollView.scrollPosition.y;
                reorder.Draw(true);
            }
        }

        protected virtual void AutoScroll()
        {
            if (new EventLib().MousePos.y < 10) { scroll -= 10; }
            else if (GetWindowSize().y - 10 < new EventLib().MousePos.y) { scroll += 10; }
        }

        protected virtual bool ShouldClose()
        {
            bool close =
                window == null ||
                focusedWindow != this;

            return
                close &&
                !adding;
        }

        protected void UndoRecord(string msg) { Undo.RecordObject(info.obj, msg); }
    }
}
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NerScript.Editor
{
    public class SearchablePopupWindow<T> : PopupWindowContent
    {
        private readonly string name = "";
        private const float Width = 130;
        private const float Height = 200;
        private const int TitleHeight = 18;
        private const int LineHeight = 15;
        private const int CountOfContentInRect = (int)((Height - TitleHeight * 2) / LineHeight);
        private Vector2 scroll = Vector2.zero;

        private readonly GUIStyle style = new GUIStyle() {fontSize = 13};

        private readonly List<(string name, T item)> list = null;
        private List<(string name, T item)> showList = null;
        private string filter = "";
        private int filteredCount = 0;

        private readonly Action<T> onSubmit = null;
        private readonly Action onClose = null;



        public SearchablePopupWindow(string _name, List<T> _list, Action<T> _onSubmit,
            Action _onClose = null, Func<T, string> onTtoString = null)
        {
            name = _name;
            onSubmit = _onSubmit;
            onClose = _onClose;
            list = _list.Select(t => (onTtoString?.Invoke(t) ?? t.ToString(), t)).ToList();
        }

        public void Show() { PopupWindow.Show(new Rect(new EventLib().MousePos, Vector2.zero), this); }
        public void Show(Rect rect) { PopupWindow.Show(rect, this); }
        public static void Show(string name, List<T> list, Action<T> onSubmit, T select = default,
            Action onClose = null, Func<T, string> onTtoString = null)
        {
            var win = new SearchablePopupWindow<T>(name, list, onSubmit, onClose, onTtoString);
            win.SelectedT(select);
            win.Show();
        }
        public static void Show(Rect rect, string name, List<T> list, Action<T> onSubmit,
            Action onClose = null, Func<T, string> onTtoString = null)
        {
            new SearchablePopupWindow<T>(name, list, onSubmit, onClose, onTtoString).Show(rect);
        }

        /// <summary>
        /// サイズを取得する
        /// </summary>
        public override Vector2 GetWindowSize()
        {
            return new Vector2(Width, Height);
        }

        public void SelectedT(T select)
        {
            int selectIndex = list.FindIndex(val => val.item.Equals(select));
            SelectedT(selectIndex);
        }
        public void SelectedT(int select)
        {
            scroll.y = (select - 2) * LineHeight;
        }

        /// <summary>
        /// GUI描画
        /// </summary>
        public override void OnGUI(Rect rect)
        {
            rect.width = Width;
            rect.height = LineHeight;
            GUILib.BackgroundColoring(Colors.Hinata, () =>
            {
                GUI.Label(new Rect(0, 0, Width, TitleHeight), name, GUI.skin.box.Copy().SetFontSize(11));
                GUILayout.Space(TitleHeight);
            });

            GUI.SetNextControlName("filter");
            filter = GUILayout.TextField(filter, "SearchTextField",GUILayout.Height(LineHeight));
            GUI.FocusControl("filter");

            using(var scrollView = new GUILayout.ScrollViewScope(scroll))
            {
                scroll = scrollView.scrollPosition;

                GUILib.Coloring(Color.cyan, () =>
                {
                    Vector2 mPos = new EventLib().MousePos;
                    rect.y = (int)mPos.y / LineHeight * LineHeight;
                    if(rect.y / LineHeight < filteredCount && mPos.x.ContainsIn(0, Width))
                        GUI.Box(rect, "");
                    rect.y = 0;
                });

                showList
                    = list
                     .Where(item => item.name.Search(filter))
                     .ToList();
                filteredCount = showList.Count();


                foreach(var item in showList)
                {
                    DrawButton(item);
                }
            }
        }

        private void CutShowList()
        {
            int cutIndex = (int)(scroll.y / 15);
            cutIndex -= 5;
            showList.CutBefore(cutIndex);
            GUILayout.Button("", style, GUILayout.Height(cutIndex * LineHeight));
        }


        private void DrawButton((string name, T item) value)
        {
            var (tName, item) = value;
            string label = $" {tName}";

            if(GUILayout.Button(label, style, GUILayout.Height(LineHeight)))
            {
                onSubmit(item);
                editorWindow.Close();
            }
        }

        public override void OnOpen()
        {
            base.OnOpen();
            EditorApplication.update += GUILib.Repaint;
        }


        public override void OnClose()
        {
            base.OnClose();
            EditorApplication.update -= GUILib.Repaint;
            onClose?.Invoke();
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Editor;
using UnityEditor;

namespace NerScript.Resource.Editor
{
    public partial class AudioDatabaseWindow : EditorWindowBase<AudioDatabaseWindow>
    {
        public class ConsoleArea : IArea
        {

            #region =====Field=====================================================================

            public float height = 15;
            public string text = "there is console area.";
            private GUIStyle textStyle = null;

            #endregion

            #region =====Event=====================================================================

            public void OnEnabled()
            {
                textStyle = GUIStyle.none.Copy().SetFontSize((int)height - 3).SetTextColor(Colors.Red);
            }

            public void OnGUIEnabled()
            {

            }

            #endregion

            #region =====Layout====================================================================

            public void ConsoleLog(string _text) { text = _text; }

            public void Layout()
            {
                GUILayout.Box("", GUIStyle.none, GUILayout.Height(height - 2));
                Rect rect = GetConsoleRect();
                GUI.Box(rect, "", GUI.skin.box);
                GUI.Label(rect, text, textStyle);
            }
            public Rect GetConsoleRect()
            {
                Rect rect = EditorWindowData.rect;
                rect.y = EditorWindowData.Center.y * 2;
                rect.y -= height;
                rect.width = EditorWindowData.Center.x * 2;
                rect.height = height;
                return rect;
            }

            #endregion
        }
    }
}
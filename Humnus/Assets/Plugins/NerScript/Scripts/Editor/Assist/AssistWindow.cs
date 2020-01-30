using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace NerScript.Editor
{
    using AssistPopup = SearchablePopupWindow<AssistWindow.AssistType>;

    public class AssistWindow : EditorWindowBase<AssistWindow>
    {
        public enum AssistType
        {
            PingToSelect,

        }

        public override string WindowTitle => "AssistWindow";

        private GUIStyle miniLabel = null;
        private Lazy<List<AssistType>> allAssistType = null;

        [NonSerialized] private bool initGUI = false;

        [MenuItem("Tools/Ner/AssistWindow")]
        public static void ShowAssistWindow() { Open(); }


        protected override void Init(IEditorWindowMediator windowMediator)
        {


            AssistType.PingToSelect.Int();


        }

        protected override void OnEnabled()
        {
            minSize = new Vector2(100, 40);
            maxSize = new Vector2(10000, 40);
        }

        private void GUIInit()
        {
            miniLabel = GUI.skin.label.Copy().SetFontSize(10);
            allAssistType
                = new Lazy<List<AssistType>>(
                    () => EnumLib.GetAllEnum<AssistType>().ToList(),
                    false);
            initGUI = true;
        }

        protected override void OnGUI()
        {
            if(!initGUI) GUIInit();
            using(new GUILayout.HorizontalScope())
            {
                GUILayout.Label(SelectionLib.PathOfSelectObject.GetFileName(true), miniLabel);
            }
            using(new GUILayout.HorizontalScope())
            {
                if(GUILayout.Button("Tools"))
                {
                    ShowAssistPopup();
                }
                if(GUILayout.Button("Tools"))
                {
                    ShowAssistPopup();
                }
            }
        }


        private void ShowAssistPopup()
        {
            AssistPopup tools = new AssistPopup("Assists", allAssistType.Value, Assist);
            tools.Show();
        }


        private void Assist(AssistType type)
        {
            switch(type)
            {
                case AssistType.PingToSelect:
                    PingToSelect();
                    break;
                default:
                    Debug.Log("");
                    break;
            }
        }

        private static void PingToSelect() { EditorLib.SendPingToSelectObject(); }

    }
}

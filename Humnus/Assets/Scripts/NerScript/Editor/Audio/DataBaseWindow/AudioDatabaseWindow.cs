using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript;
using NerScript.Editor;
using Object = UnityEngine.Object;

namespace NerScript.Resource.Editor
{
    //using ResourcesArea = AudioDatabaseWindow.ResourceArea;
    //using AudioDataArea = AudioDatabaseWindow.AudioDataArea;
    //using ConsoleArea = AudioDatabaseWindow.ConsoleArea;


    public partial class AudioDatabaseWindow : EditorWindowBase<AudioDatabaseWindow>
    {
        public override string WindowTitle => "AudioDatabaseWindow";

        public static void OnGUIEnabled()
        {
            guiEnabled = true;

            backGroundStyle = GUI.skin.box.Copy().SetMargin(0, 0);



            foreach (var area in areas)
            {
                area.OnGUIEnabled();
            }
        }
        private static bool guiEnabled = false;

        public static AudioData SelectAudioData
        {
            get =>
                resourcesArea.indexSelector.IsSelect ?
                audioDatas[resourcesArea.SelectIndex] :
                new AudioData();
        }

        #region Areas

        private static List<IArea> areas = null;
        private static ResourcesArea resourcesArea => (ResourcesArea)areas[0];
        private static AudioDataArea audioDataArea => (AudioDataArea)areas[1];
        private static ConsoleArea consoleArea => (ConsoleArea)areas[2];


        public static GUIStyle backGroundStyle = null;


        #endregion




        public static List<AudioData> audioDatas = null;
        public static List<AudioData> ignoreDatas = null;

        public static ListSwitcher<AudioData> currentDataList = null;

        public static EventLib eventLib = null;




        protected override void OnEnabled()
        {
            ReLoadDataBase();
            eventLib = new EventLib();

            EditorWindowOption.UpdateRepaintInterval = 2;

            areas = new List<IArea>();
            areas.Add(new ResourcesArea());
            areas.Add(new AudioDataArea());
            areas.Add(new ConsoleArea());


            foreach (var area in areas)
            {
                area.OnEnabled();

            }
        }

        protected override void Init(IEditorWindowMediator windowMediator)
        {
            minSize = new Vector2(990, 430);
        }

        public static void ReLoadDataBase()
        {
            audioDatas = AudioDatabaseResources.GetAllAudioData();
            ignoreDatas = AudioDatabaseResources.GetAllIgnoredAudioDatas();
        }

        protected override void OnGUI()
        {
            if (!guiEnabled) OnGUIEnabled();

            eventLib.Update();

            DrawBackGround(new Color(.2f, .2f, .2f));

            GUILayout.BeginHorizontal();
            resourcesArea.Layout();
            GUILayout.Label(" ", GUILayout.Width(100));
            audioDataArea.Layout();

            if (GUILayout.Button("box2", GUI.skin.box))
            {

                OnEnabled();
            }

            GUILayout.EndHorizontal();
            consoleArea.Layout();







            //return;
            Rect resizeRect = new Rect(resourcesArea.LayoutWidth - 5, 25, 10, EditorWindowData.Height - 40);
            if (reswidth) resizeRect = EditorWindowData.rect;
            EditorGUIUtility.AddCursorRect(resizeRect, MouseCursor.ResizeHorizontal);

            if (reswidth && new EventLib().MouseDrag)
            {
                resourcesArea.LayoutWidth = new EventLib().MousePos.x;
                //Debug.Log("move");
            }
            if (!reswidth && new EventLib().MouseDown && resizeRect.Contains(new EventLib().MousePos))
            {
                reswidth = true;
                //Debug.Log("cli");
            }
            if (new EventLib().MouseUp)
            {
                reswidth = false;
            }
        }

        bool reswidth = false;
    }
}
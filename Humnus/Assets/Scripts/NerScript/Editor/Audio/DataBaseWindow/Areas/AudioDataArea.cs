using System;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Editor;
using UnityEditor;
using UnityEditorInternal;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace NerScript.Resource.Editor
{
    public partial class AudioDatabaseWindow : EditorWindowBase<AudioDatabaseWindow>
    {
        public class AudioDataArea : IArea
        {

            #region =====Field=====================================================================






            #endregion

            #region =====Event=====================================================================
            public void OnEnabled()
            {

            }

            public void OnGUIEnabled()
            {

            }
            #endregion

            #region =====Layout====================================================================

            public void Layout()
            {
                GUILayout.BeginVertical();
                using (new GUILib.BackgroundColoringScope(new Color(.3f, .3f, .4f)))
                using (new GUILayout.VerticalScope(backGroundStyle, GUILayout.Height(200), GUILayout.ExpandWidth(true)))
                {
                    switch (resourcesArea.selectTab)
                    {
                        case ResourcesArea.Tab.Ignore: LayoutIgnoreAudioData(); break;
                        case ResourcesArea.Tab.Resources: LayoutResourcesAudioData(); break;
                    }
                }
                GUILayout.Label(" ");
                using (new GUILib.BackgroundColoringScope(new Color(.3f, .3f, .4f)))
                using (new GUILayout.VerticalScope(backGroundStyle, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
                {
                    LayoutAudioDetail();
                }
                GUILayout.EndVertical();
            }


            public void LayoutResourcesAudioData()
            {
                AudioData data = SelectAudioData;

                AudioNameField(data);
            }

            public void LayoutIgnoreAudioData()
            {
                AudioData data = SelectAudioData;
                AudioNameField(data);

                AudioGroup ag;
                using (new GUILib.BackgroundColoringScope(Colors.White))
                {
                    ag = (AudioGroup)EditorGUILayout.EnumPopup(data.group, GUILib.Styles.SelectableField, GUILayout.Width(50));
                }

                if (ag != data.group)
                {
                    data.group = ag;
                }
            }

            private void AudioNameField(AudioData data)
            {
                GUIStyle style = GUILib.Styles.ExpandLabel.Copy().SetFontSize(15).SetTextColor(Colors.White);
                style.alignment = TextAnchor.MiddleLeft;
                style.clipping = TextClipping.Clip;
                style.padding.bottom = 4;
                style.normal.background =
                    (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/AudioDatabase/Img/Title_Bar.png", typeof(Texture2D));
                using (new GUILib.BackgroundColoringScope(Colors.White))
                    GUILayout.Label($"　{data.name.OmittedText(45)}", style, GUILayout.ExpandWidth(true));
                style.alignment = TextAnchor.MiddleRight;
                style.normal.background = null;
                style.active.background = null;

                GUILayout.Label(
                    $"({data.address.GetDirectoryPath().OmittedText(25)}/" +
                    $"{data.address.GetFileName().OmittedText(15)}." +
                    $"{data.address.GetExtensionName()})"
                    , style);
            }

            public void AudioPreview(AudioData data)
            {
                Texture2D tex = AssetPreview.GetAssetPreview(data.clip);
                GUIStyle style = new GUIStyle(GUIStyle.none);
                style.normal.background = tex;
                using (new GUILib.BackgroundColoringScope(Colors.White))
                {
                    GUILayout.Label(" ", style, GUILayout.Width(225), GUILayout.Height(75));
                }
            }

            public void LayoutAudioDetail()
            {
                AudioData data = SelectAudioData;

                int second = (int)(data.clip?.length ?? 0);
                int minute = second / 60;
                second %= 60;

                GUILayout.Label(minute + ":" + second);

                AudioPreview(data);
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Play"))
                    {
                        EditorAudioUtility.PlayClip(data.clip);
                    }
                    if (GUILayout.Button("Stop"))
                    {
                        EditorAudioUtility.StopClip();
                    }
                }
            }
            #endregion
        }
    }
}
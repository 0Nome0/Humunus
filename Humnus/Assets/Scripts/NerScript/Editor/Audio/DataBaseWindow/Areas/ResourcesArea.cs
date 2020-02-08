using System;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Editor;
using UnityEditor;
using UniRx;

namespace NerScript.Resource.Editor
{
    public partial class AudioDatabaseWindow : EditorWindowBase<AudioDatabaseWindow>
    {
        public class ResourcesArea : IArea
        {
            #region =====Field=====================================================================
            public enum Tab { Ignore, Resources, }
            public Tab selectTab = Tab.Resources;
            public ListPointerIndexSelector<AudioData> indexSelector = null;

            public AudioData SelectResourcesData => audioDatas[indexSelector.SelectIndex];
            public AudioData SelectDatasData => ignoreDatas[indexSelector.SelectIndex];


            public int SelectIndex => indexSelector.SelectIndex;


            public List<AudioData> GetCurrentDataList()
            {
                switch (selectTab)
                {
                    case Tab.Ignore: return ignoreDatas;
                    case Tab.Resources: return audioDatas;
                };
                return null;
            }
            private GUIStyle GetElementStyle(bool normal)
            {
                if (normal) return GUIs.elementNormalStyle;
                return GUIs.elementSelectedStyle;
            }

            public float LayoutWidth
            {
                get => layoutWidth;
                set => layoutWidth = value.Clamped(300, 350);
            }
            [SerializeField] private float layoutWidth = 200;

            private float audioListScroll = 0;

            private static class GUIs
            {
                public static GUIStyle tabStyle = null;
                public static GUILayoutOption[] tabOptions = null;
                public static GUIStyle elementNormalStyle = null;
                public static GUIStyle elementSelectedStyle = null;
                public static GUIStyle iconStyle = null;
                public static GUILayoutOption[] iconOptions = null;
            }

            #endregion

            #region =====Event=====================================================================
            public void OnEnabled()
            {
                currentDataList = new ListSwitcher<AudioData>(ignoreDatas, audioDatas);
                currentDataList.Index = selectTab.Int();
                indexSelector = new ListPointerIndexSelector<AudioData>(currentDataList);
                GUIs.iconStyle = GUIStyle.none.Copy().SetMargin(new RectOffset(0, 0, 0, 0));
                GUIs.iconOptions = new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) };
                GUIs.tabOptions = new GUILayoutOption[] { GUILayout.Height(22), GUILayout.ExpandWidth(true) };
                GUIs.tabStyle = GUILib.Styles.FlameTab.Copy().SetPadding(0);
            }

            public void OnGUIEnabled()
            {
                GUIs.elementNormalStyle = GUI.skin.label.Copy().SetMargin(0, 2).SetTextColor(Colors.White).SetTextClip();
                GUIs.elementSelectedStyle = GUIs.elementNormalStyle.Copy().SetTextColor(Colors.Black).SetTextClip();
            }
            #endregion

            #region =====Layout====================================================================

            public void Layout()
            {
                using (new GUILib.BackgroundColoringScope(new Color(.3f, .3f, .4f)))
                using (new GUILayout.VerticalScope(backGroundStyle, GUILayout.Width(LayoutWidth), GUILayout.ExpandHeight(true)))
                {
                    ResourcesAreaTab();
                    ResourceList();
                }

                Input();
            }

            private void ResourcesAreaTab()
            {
                using (new GUILayout.HorizontalScope())
                using (new GUILib.GUISkinScope(GUILib.Skins.NerSkin))
                using (new GUILib.BackgroundColoringScope(Colors.White))
                {
                    Tab tab = (Tab)GUILayout.Toolbar((int)selectTab, selectTab.GetAllNames(), GUIs.tabStyle, GUIs.tabOptions);

                    if (selectTab != tab)
                    {
                        selectTab = tab;
                        currentDataList.Index = (int)tab;
                    }
                }
            }
            private void ResourceList()
            {
                using (var scroll = new GUILayout.ScrollViewScope(new Vector2(0, audioListScroll)))
                {
                    audioListScroll = scroll.scrollPosition.y;

                    for (int i = 0; i < currentDataList.Value.Count; i++)
                    {
                        DrawDataListElement(i);
                        ElementEvent(i);
                    }
                    if (eventLib.ContextClick)
                    {
                        switch (selectTab)
                        {
                            case Tab.Ignore: IgnoreDataElementMenu(); break;
                            case Tab.Resources: ResourceDataElementMenu(); break;
                        }
                    }
                }
            }
            private void DrawAudioGroupIcon(int index)
            {
                switch (index % 3)
                {
                    case 0:
                        GUILayout.Label(AudioDatabaseResources.Icon_BGM.Value, GUIs.iconStyle, GUIs.iconOptions);
                        break;
                    case 1:
                        GUILayout.Label(AudioDatabaseResources.Icon_SE.Value, GUIs.iconStyle, GUIs.iconOptions);
                        break;
                    case 2:
                        GUILayout.Label(AudioDatabaseResources.Icon_Voice.Value, GUIs.iconStyle, GUIs.iconOptions);
                        break;
                }

            }
            private void DrawDataListElement(int index)
            {
                GUILayout.BeginHorizontal();
                bool borderBackground = index % 2 == 0;
                bool selectedBackground = indexSelector.Contains(index);

                using (new GUILib.BackgroundColoringScope(new Color(.8f, .8f, .8f), selectedBackground))
                {
                    DrawAudioGroupIcon(index);
                    using (new GUILib.BackgroundColoringScope(new Color(.3f, .4f, .4f, .5f),
                        !selectedBackground && borderBackground))
                    {
                        AudioData data = currentDataList.Value[index];
                        string labelText = (" " + data.name).OmittedText((int)(layoutWidth / 9));

                        GUILayout.Label(
                            new GUIContent(labelText, data.path),
                            GetElementStyle(!selectedBackground)
                            );
                    }
                }
                GUILayout.EndHorizontal();
            }
            private void ElementEvent(int index)
            {
                //if (eventLib.Layout) return;
                //if (eventLib.Repaint) return;
                if (eventLib.WasContainedClickMouse())
                {
                    if (eventLib.MouseLeftDown)
                    {
                        IndexSelect(index);
                        EditorAudioUtility.StopClip();
                        EditorAudioUtility.PlayClip(SelectResourcesData.clip);
                    }
                }
            }
            private void IndexSelect(int index)
            {
                if (eventLib.Shift) { indexSelector.SelectsToIndex(index); }
                else if (eventLib.Control) { indexSelector.AddSelect(index); }
                else if (eventLib.Alt) { indexSelector.RemoveSelect(index); }
                else { indexSelector.Select(index); }
                consoleArea.ConsoleLog($"Index {index} Selected.");
            }
            private void ResourceDataElementMenu()
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(!indexSelector.IsSelect, "Select Clip", () =>
                {
                    EditorLib.SendPing(SelectAudioData.path);
                });
                menu.AddItem(!indexSelector.IsSelect, "Add Data", () =>
                {
                    foreach (var data in indexSelector.GetSelectList<AudioData>())
                    {
                        consoleArea.ConsoleLog($"Add {data.name}");
                        //database.datas.Add(data);
                    }
                    indexSelector.ClearSelect();
                    ReLoadDataBase();
                });
                menu.AddItem("nome/1/1", () => { });
                menu.AddItem("nome/2", () => { });
                menu.ShowAsContext();
            }

            private void IgnoreDataElementMenu()
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(!indexSelector.IsSelect, "Select Clip", () =>
                {
                    EditorLib.SendPing(SelectAudioData.path);
                });
                menu.AddItem(!indexSelector.IsSelect, "Remove Data", () =>
                {
                    //database.datas.RemoveAt(indexSelecter.SelectIndex);
                    indexSelector.ClearSelect();
                    ReLoadDataBase();
                });
                menu.AddItem("nome/1/1", () => { });
                menu.AddItem("nome/2", () => { });
                menu.ShowAsContext();
            }

            private void Input()
            {
                if (eventLib.KeyDown(KeyCode.UpArrow)) { indexSelector.MoveSelect(-1); }
                if (eventLib.KeyDown(KeyCode.DownArrow)) { indexSelector.MoveSelect(1); }
                if (eventLib.e.type == EventType.Ignore) { indexSelector.ClearSelect(); }
            }
            #endregion
        }

    }
}
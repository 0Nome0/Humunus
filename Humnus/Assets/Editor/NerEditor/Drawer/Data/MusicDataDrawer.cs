using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript;
using System.Text.RegularExpressions;

namespace NerScript.Editor
{
    [CustomEditor(typeof(MusicData))]
    [CanEditMultipleObjects]
    public class MusicDataDrawer : ScriptableObjectDrawer<MusicData>
    {
        private static readonly Color openColor = new Color(1, 1, 1);
        private static readonly Color closeColor = new Color(0.3f, 0.3f, 0.3f);
        private static Lazy<GUIStyle> tabStyle = new Lazy<GUIStyle>(
            () => GUILib.Skins.NerSkin.GetStyle("tab").Copy().SetFixedHeight(22), false);
        private static Lazy<string[]> difficultys = new Lazy<string[]>(
            () => Enum.GetNames(typeof(MusicDifficulty)).ToList().RemoveLast().ToArray(), false);
        private static MusicDifficulty selected = MusicDifficulty.NORMAL;

        protected override string assetName => "楽曲データ";

        public override void OnGUI()
        {
            IDField();
            IconField();
            AudioNameField();
            StringsField();
            OpendDifficultyFiled();
            MusicClearDataField();
        }

        private void IDField()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("ID", data.ID);
            EditorGUI.EndDisabledGroup();
        }
        private void IconField()
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                GUILayout.BeginHorizontal();
                Sprite sp = (Sprite)EditorGUILayout.ObjectField("MusicIcon", data.icon, typeof(Sprite), false);
                if (change.changed) { UndoRecord("iconChange"); data.icon = sp; }
                GUILayout.EndHorizontal();
            }
        }
        private void AudioNameField()
        {
            // EditorGUIFields.AudioAddressField(
            //     "音楽データ",
            //     data.audioName,
            //     Resource.AudioGroup.BGM,
            //     (address) => { this.data.audioName = address; });
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                GUILayout.BeginHorizontal();
                AudioClip ac = (AudioClip)EditorGUILayout.ObjectField("AudioClip", data.audiClip, typeof(AudioClip), false);
                if (change.changed) { UndoRecord("AudioClipChange"); data.audiClip = ac; }
                GUILayout.EndHorizontal();
            }
        }
        private void StringsField()
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                string name = EditorGUILayout.TextField("楽曲名", data.musicName);
                string author = EditorGUILayout.TextField("作者", data.authors);
                EditorGUILayout.LabelField("楽曲説明");
                string info = EditorGUILayout.TextArea(data.info);
                GUILayout.Space(15);
                EditorGUILayout.LabelField("解放条件");
                string openConditions = EditorGUILayout.TextArea(data.openConditions);
                if (change.changed)
                {
                    UndoRecord("stringChange");
                    data.musicName = name;
                    data.authors = author;
                    data.info = info;
                    data.openConditions = openConditions;
                }
            }
        }
        private void OpendDifficultyFiled()
        {
            EditorGUILayout.LabelField("開放フラグ");
            int count = MusicDifficulty.Count.Int();


            bool changed = false;
            GUILayout.BeginVertical(GUI.skin.box);
            GUILib.Coloring(new Color(0, 0, 0), () =>
            {
                GUILayout.BeginHorizontal();
                for (int i = 0; i < count; i++)
                {
                    if (data.openFlag[i]) { GUI.color = openColor; }
                    else { GUI.color = closeColor; }

                    if (GUILayout.Button(i.ToEnumName<MusicDifficulty>())) { changed = true; data.openFlag[i] = !data.openFlag[i]; }
                }
                GUILayout.EndHorizontal();
            });
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("全開放")) { for (int i = 0; i < count; i++) { changed = true; data.openFlag[i] = true; } }
            if (GUILayout.Button("全閉鎖")) { for (int i = 0; i < count; i++) { changed = true; data.openFlag[i] = false; } }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            if (changed) UndoRecord("changeOpenedData");
        }
        private void MusicClearDataField()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                DifficultyTab();
                ClearDataField((int)selected);
            }
        }
        private void DifficultyTab()
        {
            GUISkin skin = GUI.skin;
            GUI.skin = GUILib.Skins.NerSkin;
            selected = (MusicDifficulty)GUILayout.Toolbar((int)selected, difficultys.Value, tabStyle.Value);
            GUI.skin = skin;
        }
        private void ClearDataField(int difficulty)
        {
            MusicClearData clearData = data.clearData[difficulty];

            GUILib.Horizontal(() => GUILayout.Label(difficulty.ToEnumName<MusicDifficulty>()));

            using (var change = new EditorGUI.ChangeCheckScope())
            {
                MusicClearRank rank = (MusicClearRank)EditorGUILayout.EnumPopup("ClearRank", clearData.clearRank);
                int maxScore = EditorGUILayout.IntField("MaxScore", clearData.maxScore);
                int maxCombo = EditorGUILayout.IntField("MaxCombo", clearData.maxCombo);
                int clearCount = EditorGUILayout.IntField("クリア回数", clearData.clearCount);


                if (change.changed)
                {
                    UndoRecord("changeClearData");
                    data.clearData[difficulty].clearRank = rank;
                    data.clearData[difficulty].maxScore = maxScore;
                    data.clearData[difficulty].maxCombo = maxCombo;
                    data.clearData[difficulty].clearCount = clearCount;
                }
            }
        }
    }
}

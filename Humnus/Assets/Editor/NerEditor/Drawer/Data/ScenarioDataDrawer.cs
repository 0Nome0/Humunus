using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript;
using System.Text.RegularExpressions;

namespace NerScript.Editor
{
    [CustomEditor(typeof(ScenarioData))]
    [CanEditMultipleObjects]
    public class ScenarioDataDrawer : ScriptableObjectDrawer<ScenarioData>
    {
        protected override string assetName => "シナリオデータ";
        public override void OnGUI()
        {
            IDField();
            IconField();
            EpisodeLabel();
            StringsField();
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
                GUILib.Horizontal(() =>
                {
                    EpisodeLabel();
                    Sprite sp = (Sprite)EditorGUILayout.ObjectField("", data.icon, typeof(Sprite), false, GUILayout.Width(100));
                    if (change.changed) { UndoRecord("iconChange"); data.icon = sp; }
                });
            }
        }
        private void EpisodeLabel()
        {
            GUILayout.Label($"Episode.{data.episode}", GUIStyle.none.Copy().SetFontSize(20));
        }
        private void StringsField()
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                string name = EditorGUILayout.TextField("シナリオ名", data.scenarioTitle);
                EditorGUILayout.LabelField("解放条件");
                //string openConditions = EditorGUILayout.TextArea(data.openConditions);
                if (change.changed)
                {
                    UndoRecord("stringChange");
                    data.scenarioTitle = name;
                    //data.openConditions = openConditions;
                }
            }
        }
        private void MusicClearDataField()
        {
            //using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            //{
            //ClearDataField((int)selected);
            //}
        }
        private void ClearDataField(int difficulty)
        {
            //MusicClearData clearData = data.clearDatas[difficulty];

            GUILib.Horizontal(() => GUILayout.Label(difficulty.ToEnumName<MusicDifficulty>()));

            using (var change = new EditorGUI.ChangeCheckScope())
            {
                //MusicClearRank rank = (MusicClearRank)EditorGUILayout.EnumPopup("ClearRank", clearData.clearRank);
                //int maxScore = EditorGUILayout.IntField("MaxScore", clearData.maxScore);
                //int maxCombo = EditorGUILayout.IntField("MaxCombo", clearData.maxCombo);
                //int clearCount = EditorGUILayout.IntField("クリア回数", clearData.clearCount);


                if (change.changed)
                {
                    UndoRecord("changeClearData");
                    //data.clearDatas[difficulty].clearRank = rank;
                    //data.clearDatas[difficulty].maxScore = maxScore;
                    //data.clearDatas[difficulty].maxCombo = maxCombo;
                    //data.clearDatas[difficulty].clearCount = clearCount;
                }
            }
        }
    }
}

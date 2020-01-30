using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript;
using System.Text.RegularExpressions;

namespace NerScript.Editor
{
    [CustomEditor(typeof(CharacterData))]
    [CanEditMultipleObjects]
    public class CharacterDataDrawer : ScriptableObjectDrawer<CharacterData>
    {
        protected override string assetName => "キャラデータ";

        public override void OnGUI()
        {
            IDField();
            IconField();
            StringsField();
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
                Sprite sp = (Sprite)EditorGUILayout.ObjectField("アイコン", data.icon, typeof(Sprite), false);
                if (change.changed) { UndoRecord("iconChange"); data.icon = sp; }
            }
        }
        private void StringsField()
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                string name = EditorGUILayout.TextField("キャラクター名", data.characterName);
                EditorGUILayout.LabelField("詳細情報");
                string info = EditorGUILayout.TextArea(data.info);
                if (change.changed)
                {
                    UndoRecord("stringChange");
                    data.characterName = name;
                    data.info = info;
                }
            }
        }
    }
}

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
            DataField();
        }
        private void IDField()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("ID", data.ID);
            EditorGUI.EndDisabledGroup();
        }
        private void IconField()
        {
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                Sprite sp = (Sprite)EditorGUILayout.ObjectField("アイコン", data.icon, typeof(Sprite), false);
                if(change.changed)
                {
                    UndoRecord("iconChange");
                    data.icon = sp;
                }
            }
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                Sprite sp = (Sprite)EditorGUILayout.ObjectField("アイコン2", data.icon2, typeof(Sprite), false);
                if(change.changed)
                {
                    UndoRecord("icon2Change");
                    data.icon2 = sp;
                }
            }
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                Sprite sp = (Sprite)EditorGUILayout.ObjectField("アイコン3", data.icon3, typeof(Sprite), false);
                if(change.changed)
                {
                    UndoRecord("icon3Change");
                    data.icon3 = sp;
                }
            }
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                Sprite sp = (Sprite)EditorGUILayout.ObjectField("著作", data.iconAu, typeof(Sprite), false);
                if(change.changed)
                {
                    UndoRecord("iconAuChange");
                    data.iconAu = sp;
                }
            }
        }
        private void DataField()
        {
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                bool open = EditorGUILayout.Toggle("開放フラグ", data.isOpened);
                if(change.changed)
                {
                    UndoRecord("openChange");
                    data.isOpened = open;
                }
            }

            using(var change = new EditorGUI.ChangeCheckScope())
            {
                string name = EditorGUILayout.TextField("キャラクター名", data.characterName);
                EditorGUILayout.LabelField("キャラ詳細情報");
                string info = EditorGUILayout.TextArea(data.info);
                if(change.changed)
                {
                    UndoRecord("stringChange");
                    data.characterName = name;
                    data.info = info;
                }
            }

            using(var change = new EditorGUI.ChangeCheckScope())
            {
                int hp = EditorGUILayout.IntField("HP", data.hp);
                if(change.changed)
                {
                    UndoRecord("stringChange");
                    data.hp = hp;
                }
            }

            using(var change = new EditorGUI.ChangeCheckScope())
            {
                string skillName = EditorGUILayout.TextField("スキル名", data.skillName);
                EditorGUILayout.LabelField("スキル詳細情報");
                string info = EditorGUILayout.TextArea(data.skillInfo);
                if(change.changed)
                {
                    UndoRecord("stringChange");
                    data.skillName = skillName;
                    data.skillInfo = info;
                }
            }

            using(var change = new EditorGUI.ChangeCheckScope())
            {
                AudioClip ac = (AudioClip)EditorGUILayout.ObjectField("スキルボイス", data.SVoice, typeof(AudioClip), false);
                if(change.changed)
                {
                    UndoRecord("SVoiceChange");
                    data.SVoice = ac;
                }
            }
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                AudioClip ac = (AudioClip)EditorGUILayout.ObjectField("ボイス1", data.voice1, typeof(AudioClip), false);
                if(change.changed)
                {
                    UndoRecord("voice1Change");
                    data.voice1 = ac;
                }
            }
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                AudioClip ac = (AudioClip)EditorGUILayout.ObjectField("ボイス2", data.voice2, typeof(AudioClip), false);
                if(change.changed)
                {
                    UndoRecord("voice2Change");
                    data.voice2 = ac;
                }
            }
            using(var change = new EditorGUI.ChangeCheckScope())
            {
                AudioClip ac = (AudioClip)EditorGUILayout.ObjectField("ボイス3", data.voice3, typeof(AudioClip), false);
                if(change.changed)
                {
                    UndoRecord("voice3Change");
                    data.voice3 = ac;
                }
            }

        }
    }
}
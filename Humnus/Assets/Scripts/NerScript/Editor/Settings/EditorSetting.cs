using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace NerScript.Editor
{
    public class EditorSetting : ScriptableObject
    {
        public readonly static string PATH = "Assets/Editor/Settings/EditorSetting.asset";
        private static EditorSetting setting;
        public static EditorSetting Setting => setting ?? SetAndGetEditorSetting();



        private static EditorSetting SetAndGetEditorSetting()
        {
            setting = AssetDatabase.LoadAssetAtPath<EditorSetting>(PATH);
            return setting;
        }


        public bool autoSave = true;
        public static bool AutoSave { get { return Setting.autoSave; } set { Setting.autoSave = value; } }

        //AudioPreviewを有効にするか
        //public bool audioPreview = false;
    }
}
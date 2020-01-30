using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using NerScript.Editor;

namespace NerScript.Resource.Editor
{

    public class AudioDataBaseProvider : SettingsProvider
    {
        public readonly static string ASSET_PATH = "Assets/Audios/AudioDataBase.asset";
        private readonly static string SETTING_PATH = "Project/AudioDataBase";

        public AudioDataBaseProvider(string path, SettingsScope scope) : base(path, scope) { }
        [SettingsProvider]
        private static SettingsProvider Create() => new AudioDataBaseProvider(SETTING_PATH, SettingsScope.Project);
        public static AudioDatabase GetDatabase() => AssetDatabase.LoadAssetAtPath<AudioDatabase>(ASSET_PATH);
        public override void OnActivate(string searchContext, VisualElement rootElement) { Init(); }
        private void Init() { }
        public class AudioAudioAudio : IEditorWindowMediator { public UnityEngine.Object recordable { get; set; } }
        public override void OnGUI(string searchContext)
        {
            if (GUILayout.Button("開く")) AudioDatabaseWindow.Open(new AudioAudioAudio() { recordable = GetDatabase() });
        }
    }
}
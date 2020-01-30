using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using NerScript.Editor;
using Object = UnityEngine.Object;
using System.Text;

namespace NerScript.Resource.Editor
{
    public static class AudioDatabaseResources
    {
        private const string ASSET_PATH = "Assets/Audios/AudioDataBase.asset";
        private const string RESOURCE_PATH = "Assets/Editor/AudioDatabase/";
        private const string IMAGE_PATH = RESOURCE_PATH + "Img/";

        private const string AUDIO_FOLDER_PATH = "Assets/Audios/";
        private const string BGM_FOLDER_PATH = AUDIO_FOLDER_PATH + "BGM";
        private const string SE_FOLDER_PATH = AUDIO_FOLDER_PATH + "SE";
        private const string VOICE_FOLDER_PATH = AUDIO_FOLDER_PATH + "Voice";

        public static string GetAudioPath(AudioGroup group, bool ignore = false)
        {
            StringBuilder str = new StringBuilder();
            str.Append(AUDIO_FOLDER_PATH);
            if (ignore) str.Append("Ignore/");
            str.Append(group.ToString()).Append("/");
            return str.ToString();
        }


        #region AudioTypeIcon
        private static string ICON_NAME(string t) => $"Icon-{t}.png";
        private static string BGM = "B";
        private static string SE = "S";
        private static string Voice = "V";
        public static Lazy<Texture> Icon_BGM = new Lazy<Texture>(() => GUILib.Load.Texture(IMAGE_PATH + ICON_NAME(BGM)), false);
        public static Lazy<Texture2D> Icon_BGM2 = new Lazy<Texture2D>(() => GUILib.Load.Texture2D(IMAGE_PATH + ICON_NAME(BGM)), false);
        public static Lazy<Texture> Icon_SE = new Lazy<Texture>(() => GUILib.Load.Texture(IMAGE_PATH + ICON_NAME(SE)), false);
        public static Lazy<Texture> Icon_Voice = new Lazy<Texture>(() => GUILib.Load.Texture(IMAGE_PATH + ICON_NAME(Voice)), false);
        #endregion








        public static AddressableAssetGroup AudioGroup
            => audioGroup ?? (audioGroup = AddressableSettingUtility.GetGroup("Audios"));
        public static AddressableAssetGroup audioGroup = null;

        public static IEnumerable<AddressableAssetEntry> AudioDataEntries
            => AudioGroup.GetAllEntries().Where(e => e.MainAsset is AudioClip);


        public static List<AudioData> GetAllAudioData()
            => AudioDataEntries.Select(e => new AudioData(e)).ToList();

        public static List<string> GetAllAudioAddress()
            => AudioDataEntries.Select(e => e.address).ToList();
        public static List<string> GetAllAudioAddress(AudioGroup group)
            => AudioDataEntries.Select(e => e.address).Where(ad => ad.StartsWith(group.ToString())).ToList();


        public static AudioDatabase Database
            => dataBase ?? (dataBase = AssetDatabase.LoadAssetAtPath<AudioDatabase>(ASSET_PATH));
        public static AudioDatabase dataBase = null;


        /// <summary>
        /// プロジェクト内に存在するすべてのオーディオファイルをAudioDataに変換して取得する
        /// </summary>
        public static List<AudioData> GetAllIgnoredAudioDatas()
        {
            return
            AssetDatabase.FindAssets("t:AudioClip")
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => (path, clip: (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip))))
            .Where(data => data.clip != null)
            .Where(data => !data.path.StartsWithAny(BGM_FOLDER_PATH, SE_FOLDER_PATH, VOICE_FOLDER_PATH))
            .Select(data => new AudioData() { path = data.path, clip = data.clip, name = data.path.GetFileName() })
            .ToList();
        }
    }
}

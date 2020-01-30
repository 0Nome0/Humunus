using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using NerScript.Editor;
using Object = UnityEngine.Object;

namespace NerScript.Editor
{
    public static class AddressableSettingUtility

    {
        private const string SETTING_ASSET_PATH
            = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";




        public static AddressableAssetSettings Settings => settings ?? (settings = GetSettings());
        private static AddressableAssetSettings settings;






        private static AddressableAssetSettings GetSettings()
        => AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(SETTING_ASSET_PATH);

        public static AddressableAssetGroup GetGroup(string name)
           => Settings.groups.Find(s => s.Name == name);

        public static List<AddressableAssetEntry> GetAllEntries(this AddressableAssetGroup group,
            bool includeSelf = false, bool recurseAll = true, bool includeSubObjects = true,
            Func<AddressableAssetEntry, bool> entryFilter = null)
        {
            List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>();
            group.GatherAllAssets(entries, includeSelf, recurseAll, includeSubObjects, entryFilter);
            return entries;
        }


        public static void AddEntry(this AddressableAssetGroup group, string guid)
            => Settings.CreateOrMoveEntry(guid, group);
        [Obsolete("Heavy processing. Use AddEntry(string guid).")]
        public static void AddEntry(this AddressableAssetGroup group, Object obj)
            => group.AddEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
        public static void RemoveEntry(string guid) => Settings.RemoveAssetEntry(guid);
        public static void RemoveEntry(AddressableAssetEntry entry) => RemoveEntry(entry.guid);



        public static AddressableAssetGroup CreateGroup(string groupName)
        {


            //スキーマ生成
            List<AddressableAssetGroupSchema> schema =
                new List<AddressableAssetGroupSchema>() {
                    new BundledAssetGroupSchema(),
                    new ContentUpdateGroupSchema()
                };
            //グループの作成
            AddressableAssetGroup group = Settings.groups.Find((g) => { return g.name == groupName; });
            if (group == null)
            {
                return Settings.CreateGroup(groupName, false, false, true, schema);
            }
            return group;
        }




        public static void SetLabelToAsset(List<string> assetGuidList, string label, bool flag)
        {
            //ラベルを追加するように呼んでおく。追加されていないと設定されない。
            Settings.AddLabel(label);
            List<AddressableAssetEntry> assetList = new List<AddressableAssetEntry>();
            Settings.GetAllAssets(assetList, true);
            foreach (var assetGuid in assetGuidList)
            {
                var asset = assetList.Find((a) => { return a.guid == assetGuid; });
                if (asset != null)
                {
                    asset.SetLabel(label, flag);
                }
            }
        }


        static void BuildPlayerContent()
        {
            AddressableAssetSettings.BuildPlayerContent();
        }
    }
}

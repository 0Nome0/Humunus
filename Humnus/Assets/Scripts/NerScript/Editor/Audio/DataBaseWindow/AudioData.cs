using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace NerScript.Resource.Editor
{
    [Serializable]
    public class AudioData
    {
        public AudioData() { }
        public AudioData(AddressableAssetEntry addressableAssetEntry)
        {
            entry = addressableAssetEntry;
            address = entry.address;
            path = entry.AssetPath;
            name = address.GetFileName();
            group.SetEnum(address.Split('/')[0]);
            if (entry.MainAsset is AudioClip)
            {
                clip = (AudioClip)entry.MainAsset;
            }
        }


        public AddressableAssetEntry entry = null;

        public string address = "none/none.audio";
        public string path = "Assets/none.audio";
        public string name = "none";
        public AudioGroup group = AudioGroup.None;
        public AudioClip clip = null;
    }
}

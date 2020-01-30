using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Attribute;
using NerScript.Anime;
using Random = UnityEngine.Random;

namespace NerScript.Resource
{
    public class VoicePlayer : MonoBehaviour
    {
        [SerializeField] private List<VoiceName> voiceNames = null;
        [SerializeField, Range(0, 1)] float volume = 1.0f;
        [SerializeField] private VoiceChannel channel = VoiceChannel.ch0;
        [SerializeField, InspectorButton("再生", "Play")] private bool play = false;

        public void Play()
        {
            string name = voiceNames[Random.Range(0, voiceNames.Count - 1)].name;
            AudioAPI.PlayVoice(name, volume, channel);
        }

        public void Play(int index)
        {
            string name = voiceNames[index % voiceNames.Count].name;
            AudioAPI.PlayVoice(name, volume, channel);
        }
    }

    [Serializable]
    public struct VoiceName {[SerializeField, VoiceName] public string name; public VoiceName(string _name) { name = _name; } }
}
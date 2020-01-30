using System;
using UnityEngine;

namespace NerScript.Resource
{
    public class AudioClipLoader : AddressablesUser<AudioClip>
    {
        public IObservable<AudioClip> LoadBGM(string address) => Load("BGM/" + address);
        public IObservable<AudioClip> LoadSE(string address) => Load("SE/" + address);
        public IObservable<AudioClip> LoadVoice(string address) => Load("Voice/" + address);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Attribute;
using NerScript.Anime;

namespace NerScript.Resource
{
    public class BGMPlayer : MonoBehaviour
    {
        //[SerializeField, BGMName] private string bgmName = "";
        //[SerializeField, Range(0, 1)] private float volume = 1.0f;
        //[SerializeField] private BGMChannel channel = BGMChannel.main;
        //[SerializeField] private bool playOnStart = false;
        public AudioSource audio = null;
        public AudioClip clip = null;

        private void Start()
        {
            //if (playOnStart) AudioAPI.PlayBGM(bgmName, volume, channel);
        }

        public void Play()
        {
            //AudioAPI.PlayBGM(bgmName, volume, channel);
            audio.Play();
        }

        public void OnValidate()
        {
            if(audio != null) audio.clip = clip;
        }

        // public void FadePlay(int frame) => FadePlay(frame, channel);
        //
        // public void FadePlay(int frame, BGMChannel channel = BGMChannel.main)
        // {
        //     AudioAPI.PlayBGM(bgmName, 0, channel);
        //     AudioManager.Instance.gameObject
        //     .ObjectAnimation()
        //     .FloatLeapAnim(frame, (f) => AudioAPI.SetVolume(Mathf.Lerp(0, volume, f), AudioGroup.BGM, channel.Int()))
        //     .AnimationStart();
        // }
        //
        // public void FadeStop(int frame) => FadeStop(frame, channel);
        //
        // public void FadeStop(int frame, BGMChannel channel = BGMChannel.main)
        // {
        //     float _volume = AudioAPI.GetVolume(AudioGroup.BGM);
        //     AudioManager.Instance.gameObject
        //     .ObjectAnimation()
        //     .FloatLeapAnim(frame, (f) => AudioAPI.SetVolume(Mathf.Lerp(_volume, 0, f), AudioGroup.BGM, channel.Int()))
        //     .AnimationStart(() => AudioAPI.StopBgm(channel));
        //
        //     gameObject
        //     .ObjectAnimation()
        //     .MoveAbs(0, 0, 0, 0, EasingTypes.BackIn)
        //     .MoveAbs(0, 0, 0, 0, EasingTypes.BackIn)
        //     .AnimationStart();
        // }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using NerScript.Games;

namespace NerScript.Resource
{

    public class AudioData
    {
        public string name;
        public AudioClip clip;
        public AudioGroup group;
    }

    public enum BGMChannel
    {
        main = -1,
        sub,
        BGSE,
        Count,
    }


    public enum SEChannel
    {
        ch0 = -1,
        ch1,
        ch2,
        ch3,
        ch4,
        Count,
    }

    public enum VoiceChannel
    {
        ch0 = -1,
        ch1,
        Count,
    }

    /// <summary>
    /// 
    /// </summary>
    public class AudioManager : SingletonMonoBehaviour<AudioManager>, IAudio
    {
        private readonly int BGM_CHANNEL = (int)BGMChannel.Count;
        private readonly int SE_CHANNEL = (int)SEChannel.Count; // SEチャンネル数
        private readonly int VOICE_CHANNEL = (int)VoiceChannel.Count;

        // サウンドリソース
        private AudioSource sourceBGMMain = null; // BGM
        private AudioSource[] sourceBGMArray = null;
        private AudioSource sourceSEDefault = null;
        private AudioSource[] sourceSEArray = null;
        private AudioSource sourceVoiceDefault = null;
        private AudioSource[] sourceVoiceArray = null;

        private static AudioClipLoader audioLoader = null;

        //現在流れているBGM
        private string nowBGMName = "";

        private void Initialize()
        {
            Camera.main.gameObject.RemoveComponent<AudioListener>();
            audioLoader = new AudioClipLoader();
            InitBGMChannels();
            InitSEChannels();
            InitVoiceChannels();            
        }

        ~AudioManager() { _Finalize(); }

        private void _Finalize()
        {
            //Debug.Log("AudioManager Finalize");
        }
        private void OnDestroy()
        {
            //Debug.Log("AudioManager Destroy");
        }


        private void InitBGMChannels()
        {
            GameObject bgmPlayer = gameObject.GetChild(0);
            sourceBGMMain = bgmPlayer.GetComponent<AudioSource>();
            InitSettingSource(sourceBGMMain, AudioGroup.BGM);
            sourceBGMArray = new AudioSource[BGM_CHANNEL];
            for (int i = 0; i < BGM_CHANNEL; i++)
            {
                sourceBGMArray[i] = bgmPlayer.AddComponent<AudioSource>();
                InitSettingSource(sourceBGMArray[i], AudioGroup.BGM);
            }
        }

        private void InitSEChannels()
        {
            GameObject sePlayer = gameObject.GetChild(1);
            sourceSEDefault = sePlayer.GetComponent<AudioSource>();
            InitSettingSource(sourceSEDefault, AudioGroup.BGM);
            sourceSEArray = new AudioSource[SE_CHANNEL];
            for (int i = 0; i < SE_CHANNEL; i++)
            {
                sourceSEArray[i] = sePlayer.AddComponent<AudioSource>();
                InitSettingSource(sourceSEArray[i], AudioGroup.BGM);
            }
        }

        private void InitVoiceChannels()
        {
            GameObject voicePlayer = gameObject.GetChild(2);
            sourceVoiceDefault = voicePlayer.GetComponent<AudioSource>();
            InitSettingSource(sourceVoiceDefault, AudioGroup.BGM);
            sourceVoiceArray = new AudioSource[VOICE_CHANNEL];
            for (int i = 0; i < VOICE_CHANNEL; i++)
            {
                sourceVoiceArray[i] = voicePlayer.AddComponent<AudioSource>();
                InitSettingSource(sourceVoiceArray[i], AudioGroup.BGM);
            }
        }

        private void InitSettingSource(AudioSource source, AudioGroup type)
        {
            source.playOnAwake = false;

            if (type == AudioGroup.BGM)
            {
                source.loop = true;
            }
        }

        /// <summary>
        /// サウンドマネージャーの作成
        /// </summary>
        public static void Create(GameObject prefab)
        {
            GameObject obj = Instantiate(prefab);
            obj.DontDestroyOnLoad();
            instance = obj.GetComponent<AudioManager>();
            instance.Initialize();
        }

        /// <summary>
        /// AudioSourceを取得する
        /// </summary>
        /// <param name="type"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        private AudioSource GetAudioSource(AudioGroup type, int channel = -1)
        {
            switch (type)
            {
                case AudioGroup.BGM:
                    if (channel == -1) return sourceBGMMain;
                    return sourceBGMArray[channel];
                case AudioGroup.SE:
                    if (channel == -1) return sourceSEDefault;
                    return sourceSEArray[channel];
                case AudioGroup.Voice:
                    if (channel == -1) return sourceVoiceDefault;
                    return sourceVoiceArray[channel];
                default: return null;
            }
        }

        #region BGM
        /// <summary>
        /// BGMの再生
        /// </summary>
        /// <param name="name"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        public void PlayBGM(string name, float volume = 1.0f, BGMChannel channel = BGMChannel.main)
        {
            if (nowBGMName == name) return; //同じ曲なら何もしない。

            audioLoader
            .LoadBGM(name)
            .Subscribe(clip =>
            {
                StopBgm(channel);
                PlayBGM(clip, volume, channel);
                if (channel == BGMChannel.main) nowBGMName = name;
            });
        }
        public void PlayBGM(AudioClip clip, float volume = 1.0f, BGMChannel channel = BGMChannel.main)
        {
            AudioSource source;

            if (0 <= channel)
            {
                source = GetAudioSource(AudioGroup.BGM, channel.Int());
            }
            else
            {
                source = GetAudioSource(AudioGroup.BGM);
            }

            source.clip = clip;
            source.volume = volume;
            source.Play();
        }
        public bool IsPlayingBGM => nowBGMName == "";
        /// <summary>
        /// BGMの停止
        /// </summary>
        /// <returns></returns>
        public void StopBgm(BGMChannel channel = BGMChannel.main)
        {
            if (channel == BGMChannel.main) nowBGMName = "";
            GetAudioSource(AudioGroup.BGM, channel.Int()).Stop();
        }
        #endregion

        #region SE
        public void PlaySE(string name, float volume = 1.0f, SEChannel channel = SEChannel.ch0)
        {
            audioLoader
            .LoadSE(name)
            .Subscribe(clip =>
            {
                PlaySE(clip, volume, channel);
            });
        }
        public void PlaySE(AudioClip clip, float volume = 1.0f, SEChannel channel = SEChannel.ch0)
        {
            if (0 <= channel)
            {
                // チャンネル指定
                AudioSource source = GetAudioSource(AudioGroup.SE, channel.Int());
                source.volume = volume;
                source.clip = clip;
                source.Play();
            }
            else
            {
                // デフォルトで再生
                AudioSource source = GetAudioSource(AudioGroup.SE);
                source.volume = volume;
                source.PlayOneShot(clip);
            }
        }
        #endregion

        #region Voice
        public void PlayVoice(string name, float volume = 1.0f, VoiceChannel channel = VoiceChannel.ch0)
        {
            audioLoader
            .LoadVoice(name)
            .Subscribe(clip =>
            {
                PlayVoice(clip, volume, channel);
            });
        }
        public void PlayVoice(AudioClip clip, float volume = 1.0f, VoiceChannel channel = VoiceChannel.ch0)
        {
            if (0 <= channel)
            {
                AudioSource source = GetAudioSource(AudioGroup.Voice, channel.Int());
                source.volume = volume;
                source.clip = clip;
                source.Play();
            }
            else
            {
                AudioSource source = GetAudioSource(AudioGroup.Voice);
                source.volume = volume;
                source.PlayOneShot(clip);
            }
        }
        #endregion

        #region Volume
        public void SetVolume(float volume, AudioGroup type, int channel = -1)
        => GetAudioSource(type, channel).volume = volume;
        public float GetVolume(AudioGroup type, int channel = -1)
            => GetAudioSource(type, channel).volume;
        #endregion
    }
}
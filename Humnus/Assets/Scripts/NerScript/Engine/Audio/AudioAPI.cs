using UnityEngine;
using NerScript.Resource;

public static class AudioAPI
{
    public static void PlayBGM(string name, float volume = 1, BGMChannel channel = BGMChannel.main)
         => AudioManager.Instance.PlayBGM(name, volume, channel);
    public static bool IsPlayingBGM
        => AudioManager.Instance.IsPlayingBGM;
    public static void StopBgm(BGMChannel channel = BGMChannel.main)
        => AudioManager.Instance.StopBgm(channel);
    public static void PlaySE(string name, float volume = 1, SEChannel channel = SEChannel.ch0)
        => AudioManager.Instance.PlaySE(name, volume, channel);
    public static void PlayVoice(string name, float volume = 1, VoiceChannel channel = VoiceChannel.ch0)
        => AudioManager.Instance.PlayVoice(name, volume, channel);
    public static void SetVolume(float volume, AudioGroup type, int channel = -1)
        => AudioManager.Instance.SetVolume(volume, type, channel);
    public static float GetVolume(AudioGroup type, int channel = -1)
        => AudioManager.Instance.GetVolume(type, channel);
}
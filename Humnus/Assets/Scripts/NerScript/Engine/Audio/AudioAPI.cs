using UnityEngine;
using NerScript.Resource;

public static class AudioAPI
{
    public static void PlayBGM(string name, float volume = 1, BGMChannel channel = BGMChannel.main)
    {
    }
    public static bool IsPlayingBGM
        => false;
    public static void StopBgm(BGMChannel channel = BGMChannel.main)
    {
    }
    public static void PlaySE(string name, float volume = 1, SEChannel channel = SEChannel.ch0)
    {
    }
    public static void PlayVoice(string name, float volume = 1, VoiceChannel channel = VoiceChannel.ch0)
    {
    }
    public static void SetVolume(float volume, AudioGroup type, int channel = -1)
    {
    }
    public static float GetVolume(AudioGroup type, int channel = -1)
        => 0;
}
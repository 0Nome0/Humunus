using UnityEngine;
using UnityEditor;
using NerScript.Resource;

public interface IAudio
{
    void PlayBGM(string name, float volume = 1.0f, BGMChannel channel = BGMChannel.main);
    bool IsPlayingBGM { get; }
    void StopBgm(BGMChannel channel = BGMChannel.main);
    void PlaySE(string name, float volume = 1.0f, SEChannel channel = SEChannel.ch0);
    void PlayVoice(string name, float volume = 1.0f, VoiceChannel channel = VoiceChannel.ch0);
    void SetVolume(float volume, AudioGroup type, int channel = -1);
    float GetVolume(AudioGroup type, int channel = -1);
}
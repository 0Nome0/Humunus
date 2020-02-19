using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シナリオで使用するSE,Voice管理用スクリプト
/// </summary>
public class SEToVoiceController : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<string, AudioClip> se;           //シナリオで使用するSE
    [HideInInspector]
    public Dictionary<string, AudioClip> voice;        //シナリオで使用するVoice

    private string[] seSoundArray;
    private string[] voiceSoundArray;

    public void Initialize()
    {
        se = new Dictionary<string, AudioClip>();
        voice = new Dictionary<string, AudioClip>();
    }

    /// <summary>
    /// SE,Voiceでーたの文字列セット
    /// </summary>
    /// <param name="seData"></param>
    /// <param name="voiceData"></param>
    public void SetSeToVoiceData(string[] seData,string[]voiceData)
    {
        if (seSoundArray == null)
            seSoundArray = new string[seData.Length];
        seSoundArray = seData;
        if (voiceSoundArray == null)
            voiceSoundArray = new string[voiceData.Length];
        voiceSoundArray = voiceData;
    }

    /// <summary>
    /// SEを追加する
    /// </summary>
    /// <param name="clip"></param>
    public void AddSE(AudioClip clip)
    {
        string name = clip.name;
        if (se.ContainsKey(name))
            return;
        se.Add(name, clip);
    }

    /// <summary>
    /// Voiceを追加する
    /// </summary>
    /// <param name="clip"></param>
    public void AddVoice(AudioClip clip)
    {
        string name = clip.name;
        if (voice.ContainsKey(name))
            return;
        voice.Add(name, clip);
    }
}

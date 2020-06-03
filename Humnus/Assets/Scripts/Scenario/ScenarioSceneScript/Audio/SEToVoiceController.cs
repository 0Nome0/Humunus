using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シナリオで使用するSE,Voice管理用スクリプト
/// </summary>
public class SEToVoiceController : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField, Range(0, 1)]
    private float volume = 1.0f;
    [HideInInspector]
    public Dictionary<string, AudioClip> se;           //シナリオで使用するSE
    [HideInInspector]
    public Dictionary<string, AudioClip> voice;        //シナリオで使用するVoice

    private string[] seSoundArray;
    private string[] voiceSoundArray;

    private int CurrentIndex => ScenarioDataInfo.Instance.scenarioTextIndex;

    public void Initialize()
    {
        audioSource.volume = volume;
        se = new Dictionary<string, AudioClip>();
        voice = new Dictionary<string, AudioClip>();
    }

    public void UpdateSE()
    {
        if (seSoundArray.Length <= CurrentIndex)
            return;
        if (seSoundArray[CurrentIndex] == "")
            return;
        audioSource.PlayOneShot(se[seSoundArray[CurrentIndex]]);
        Debug.Log("Seの再生");
    }

    public void UpdateVoice()
    {
        if (voiceSoundArray.Length <= CurrentIndex)
            return;
        if (voiceSoundArray[CurrentIndex] == "")
            return;
        if (!voice.ContainsKey(voiceSoundArray[CurrentIndex]))
        {
            Debug.LogError("そのキーは存在しません。keyName = " + voiceSoundArray[CurrentIndex]);
            return;
        }
        audioSource.PlayOneShot(voice[voiceSoundArray[CurrentIndex]]);
        Debug.Log("Voiceの再生");
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

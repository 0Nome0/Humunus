using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SmyCustom;

public class ScenarioSoundManager : MonoBehaviour
{
    public BGMListenController bgm_Controller = null;
    public SEToVoiceController se_Voice_Controller = null;

    public void Initialize()
    {
        bgm_Controller.Initialize();
        se_Voice_Controller.Initialize();
    }

    public void SetSoundData(string[] bgmData,string[] seData,string[]voiceData)
    {
        bgm_Controller.SetBGMData(bgmData);
        se_Voice_Controller.SetSeToVoiceData(seData, voiceData);
        bgm_Controller.FadeOutBGM();
    }

    public void SoundUpdate()
    {
        bgm_Controller.FadeInBGM();
        //se_Voice_Controller
    }

    /// <summary>
    /// 音源の読み込み
    /// </summary>
    public void Load()
    {
        LoadBGM();
        LoadSE();
        LoadVoice();
    }

    /// <summary>
    /// BGMの読み込み
    /// </summary>
    private void LoadBGM()
    {
        var bgms = Resources.LoadAll<AudioClip>("Audio/Scenario/BGM").ToList();
        while (bgms.Count > 0)
        {
            bgm_Controller.AddBGM(bgms.Dequeue());
        }
    }

    /// <summary>
    /// SEの読み込み
    /// </summary>
    private void LoadSE()
    {
        var se = Resources.LoadAll<AudioClip>("Audio/Scenario/SE").ToList();
        while (se.Count > 0)
        {
            se_Voice_Controller.AddSE(se.Dequeue());
        }
    }

    /// <summary>
    /// Voiceの読み込み
    /// </summary>
    private void LoadVoice()
    {
        var voice = Resources.LoadAll<AudioClip>("Audio/Scenario/Voice").ToList();
        while (voice.Count > 0)
        {
            se_Voice_Controller.AddVoice(voice.Dequeue());
        }
    }
}

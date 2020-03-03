using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SmyCustom;

public class ScenarioSoundManager : MonoBehaviour
{
    public BGMListenController bgm_Controller = null;
    public SEToVoiceController se_Voice_Controller = null;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        bgm_Controller.Initialize();
        se_Voice_Controller.Initialize();
    }

    /// <summary>
    /// テキストデータの読み込み
    /// </summary>
    /// <param name="bgmData"></param>
    /// <param name="seData"></param>
    /// <param name="voiceData"></param>
    public void SetSoundData(string[] bgmData, string[] seData, string[] voiceData)
    {
        bgm_Controller.SetBGMData(bgmData);
        se_Voice_Controller.SetSeToVoiceData(seData, voiceData);
    }

    public void SoundUpdate()
    {
        bgm_Controller.FadeInBGM();
        se_Voice_Controller.UpdateSE();
        se_Voice_Controller.UpdateVoice();
    }

    /// <summary>
    /// 音源の読み込み
    /// </summary>
    public IEnumerator Load()
    {
        yield return StartCoroutine(LoadBGM());
        yield return StartCoroutine(LoadSE());
        yield return StartCoroutine(LoadVoice());
    }

    /// <summary>
    /// BGMの読み込み
    /// </summary>
    private IEnumerator LoadBGM()
    {
        var bgms = Resources.LoadAll<AudioClip>("Audio/Scenario/BGM").ToList();
        while (bgms.Count > 0)
        {
            bgm_Controller.AddBGM(bgms.Dequeue());
        }
        Debug.Log("BGMの読み込みが終了しました");
        yield return null;
    }

    /// <summary>
    /// SEの読み込み
    /// </summary>
    private IEnumerator LoadSE()
    {
        var se = Resources.LoadAll<AudioClip>("Audio/Scenario/SE").ToList();
        while (se.Count > 0)
        {
            se_Voice_Controller.AddSE(se.Dequeue());
        }
        Debug.Log("SEの読み込みが終了しました");
        yield return null;
    }

    /// <summary>
    /// Voiceの読み込み
    /// </summary>
    private IEnumerator LoadVoice()
    {
        var voice = Resources.LoadAll<AudioClip>("Audio/Scenario/Voice").ToList();
        while (voice.Count > 0)
        {
            se_Voice_Controller.AddVoice(voice.Dequeue());
        }
        Debug.Log("Voiceの読み込みが終了しました");
        yield return null;
    }
}

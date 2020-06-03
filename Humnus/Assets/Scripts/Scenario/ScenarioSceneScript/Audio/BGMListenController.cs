using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BGMListenController : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField,Range(0,1)]
    private float volume = 0.5f;
    [HideInInspector]
    public Dictionary<string, AudioClip> bgm;           //シナリオで使用するBGM

    private int CurrentIndex => ScenarioDataInfo.Instance.scenarioTextIndex;

    private string[] bgmSoundArray;

    public void Initialize()
    {
        bgm = new Dictionary<string, AudioClip>();
        audioSource.volume = 0.0f;
    }

    /// <summary>
    /// リソースデータの文字列をセットする
    /// </summary>
    /// <param name="data"></param>
    public void SetBGMData(string[] data)
    {
        if (bgmSoundArray == null)
            bgmSoundArray = new string[data.Length];
        bgmSoundArray = data;
    }

    /// <summary>
    /// BGMを設定する
    /// </summary>
    /// <param name="clip"></param>
    public void AddBGM(AudioClip clip)
    {
        string name = clip.name;
        if (bgm.ContainsKey(name))
            return;
        bgm.Add(name, clip);
    }


    public void FadeInBGM()
    {
        if (bgmSoundArray.Length <= CurrentIndex)
            return;
        if (bgmSoundArray[CurrentIndex] == "")
            return;
        if (audioSource.isPlaying)
        {
            audioSource.DOFade(
                0.0f,
                0.1f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    FadeOutBGM();
                });
        }
    }

    /// <summary>
    /// BGMのフェードアウト
    /// </summary>
    public void FadeOutBGM()
    {
        if (bgmSoundArray.Length <= CurrentIndex)
            return;
        if (bgmSoundArray[CurrentIndex] == "")
            return;
        if (!bgm.ContainsKey(bgmSoundArray[CurrentIndex]))
        {
            Debug.LogError("そのキーは存在しません。keyName = " + bgmSoundArray[CurrentIndex]);
            return;
        }
        //クリップをセットする
        audioSource.clip = bgm[bgmSoundArray[CurrentIndex]];
        //audioSource.DOFade(
        //    volume,
        //    0.1f)
        //    .SetEase(Ease.Linear);
        audioSource.volume = volume;
        audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SmyCustom;

/// <summary>
/// シナリオシーンでフェードを管理するスクリプト
/// </summary>
public class FadeControl : MonoBehaviour
{
    private SpriteRenderer fadeSprite = null;
    private string[] fadeData;

    private int CurrentIndex => ScenarioDataInfo.Instance.scenarioTextIndex;

    //true：シナリオシーンのUpdateをしない
    public bool FadeFlag { get; private set; }


    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        fadeSprite = GetComponent<SpriteRenderer>();
        fadeSprite.color = fadeSprite.color.Alpha(1.0f);
        FadeFlag = true;
    }

    /// <summary>
    /// フェードデータをセットする
    /// </summary>
    /// <param name="data"></param>
    public void SetFadeData(string[] data)
    {
        if (fadeData == null)
            fadeData = new string[data.Length];
        fadeData = data;
    }

    /// <summary>
    /// フェイドイン
    /// </summary>
    public void FadeIn()
    {
        DOTween.ToAlpha(
            () => fadeSprite.color,
            color => fadeSprite.color = color,
            1.0f,
            0.3f)
            .SetEase(Ease.Linear);
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    public void FadeOut()
    {
        DOTween.ToAlpha(
           () => fadeSprite.color,
           color => fadeSprite.color = color,
           0.0f,
           0.3f)
           .SetEase(Ease.Linear)
           .OnComplete(() => FadeFlag = false);
    }

    public void FadeUpdate()
    {
        if (fadeData.Length <= CurrentIndex)
            return;

        if (fadeData[CurrentIndex] == "In")
            FadeIn();
        else if (fadeData[CurrentIndex] == "Out")
            FadeOut();
    }
}

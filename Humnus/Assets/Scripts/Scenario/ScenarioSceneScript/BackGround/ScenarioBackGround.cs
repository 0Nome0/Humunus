using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmyCustom;
using DG.Tweening;

public class ScenarioBackGround : MonoBehaviour
{
    [SerializeField]
    private BackGroundData spriteData = null;
    [SerializeField]
    private SpriteRenderer frontSprite = null;
    [SerializeField]
    private SpriteRenderer backSprite = null;

    private string[] backGroundArray;
    private int CurrentIndex => ScenarioDataInfo.Instance.scenarioTextIndex;

    public void Initialize()
    {
        frontSprite.sortingOrder = -1;
        backSprite.sortingOrder = -2;
    }

    public void SetBackGroundData(string[] data)
    {
        if (backGroundArray == null)
            backGroundArray = new string[data.Length];
        backGroundArray = data;
        UpdateBackGround();
    }

    /// <summary>
    /// 背景画像の更新（即座）
    /// </summary>
    /// <param name="spriteName"></param>
    public void UpdateBackGround()
    {
        if (backGroundArray.Length <= CurrentIndex)
            return;
        frontSprite.sprite = spriteData.GiveSpriteData(backGroundArray[CurrentIndex]);
    }

    /// <summary>
    /// 背景画像の更新（フェード）
    /// </summary>
    /// <param name="spriteName"></param>
    public void BackGroundChange()
    {
        if (backGroundArray.Length <= CurrentIndex)
            return;
        string name = backGroundArray[CurrentIndex];
        if (name == "")
            return;

        backSprite.sprite = spriteData.GiveSpriteData(name);
        DOTween.ToAlpha(
            () => frontSprite.color,
            color => frontSprite.color = color,
            0.0f,
            0.2f)
            .SetEase(Ease.Linear)
            .OnComplete(()=>
            {
                frontSprite.sprite = backSprite.sprite;
                frontSprite.color = frontSprite.color.Alpha(1.0f);
                backSprite.sprite = null;
            });
    }

}

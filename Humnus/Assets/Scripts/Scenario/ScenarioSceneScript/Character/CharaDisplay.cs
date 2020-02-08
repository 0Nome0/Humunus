using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SmyCustom;

public class CharaDisplay : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [HideInInspector]
    public string charaName = "";
    [HideInInspector]
    public string facialName = "";

    private int CurrentIndex => ScenarioDataInfo.Instance.scenarioTextIndex;

    private string[] displayData;   //表示するかどうか

    /// <summary>
    /// キャラ画像更新用プロパティ
    /// </summary>
    public Sprite CharaImage
    {
        get { return spriteRenderer.sprite; }
        set { spriteRenderer.sprite = value; }
    }

    /// <summary>
    /// キャラの色変更用プロパティ
    /// </summary>
    public Color CharaImageColor
    {
        get { return spriteRenderer.color; }
        set { spriteRenderer.color = value; }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        CharaImageColor = CharaImageColor.Alpha(0.0f);//スタート時Alphaをゼロにする
        CharaImage = null;
        charaName = "";
    }

    /// <summary>
    /// 表示するかどうかのデータをセット
    /// </summary>
    /// <param name="data"></param>
    public void SetDisplayData(string[] data)
    {
        if (displayData == null)
            displayData = new string[data.Length];
        displayData = data;
    }

    /// <summary>
    /// 単純にキャラ画像更新
    /// </summary>
    /// <param name="charaSprite"></param>
    public void SpriteUpdate(Sprite charaSprite)
    {
        CharaImage = charaSprite;                        //画像の更新
    }

    /// <summary>
    /// キャラの画像の色をフェード（Gray）に変更する
    /// →しゃべっていない状態にする
    /// </summary>
    public void FadeInCharacter()
    {
        if (CharaImage == null)
            return;
        DOTween.To(
            () => CharaImageColor,               //対象
            color => CharaImageColor = color,    //値の更新
            Color.gray,                          //最終的な色
            0.25f)                               //時間
            .SetEase(Ease.Linear);               //Easing指定
        spriteRenderer.sortingOrder = 0;
    }

    /// <summary>
    /// キャラの画像の色をフェード（White）に変更する
    /// →しゃべっている状態にする
    /// </summary>
    public void FadeOutCharacter()
    {
        if (CharaImage == null)
            return;
        DOTween.To(
            () => CharaImageColor,               //対象
            color => CharaImageColor = color,    //値の更新
            Color.white,                         //最終的な色
            0.25f)                               //時間
            .SetEase(Ease.Linear);               //Easing指定
        spriteRenderer.sortingOrder = 1;
    }

    /// <summary>
    //シナリオ時に呼ばれる
    /// 画像のAlpha値を0にする 
    /// </summary>
    /// <param name="charaImage">フェードアウトしてキャラを表示するなら値を入れる</param>
    public void ChangeFadeInCharacter(Sprite charaImage = null)
    {
        if (CharaImage == null)
            return;
        DOTween.ToAlpha(
            () => CharaImageColor,                          //対象
            color => CharaImageColor = color,               //値の更新
            0.0f,                                           //最終的なAlphaの色
            0.1f)                                           //時間
            .SetEase(Ease.Linear)                           //Easingの指定
            .OnComplete(() =>
            {
                if (charaImage != null)
                    ChangeFadeOutCharacter(charaImage);
                else
                    CharaImage = null;
            });
    }

    /// <summary>
    /// 表示画像を更新して
    /// Alphaを1にする
    /// </summary>
    /// <param name="charaImage"></param>
    public void ChangeFadeOutCharacter(Sprite charaImage)
    {
        
        CharaImage = charaImage;                        //画像の更新
        DOTween.ToAlpha(
           () => CharaImageColor,                       //対象
           color => CharaImageColor = color,            //値の更新
           1.0f,                                        //最終的なAlphaの色
           0.1f)                                        //時間
          .SetEase(Ease.Linear)                         //Easingの指定
          .OnComplete(() => Debug.Log("更新終了"));     //終了後に呼ばれる
        spriteRenderer.sortingOrder = 1;
    }

    /// <summary>
    /// データを見て表示するかどうか
    /// </summary>
    /// <returns></returns>
    public void DisplayCharacterInfo()
    {
        if (displayData[CurrentIndex] == "In")
        {
            ChangeFadeInCharacter();
        }
    }
}

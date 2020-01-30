using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SmyCustom;

public class UIWindow : MonoBehaviour
{
    [SerializeField]
    private float initScale = 0.8f;
    [SerializeField]
    private float duration = 0.5f;
    private RectTransform myRect = null;
    private Graphic[] childrenuGUIList;
     
    //必ずウィンドウを閉じてから表示する。（Unity上で）
    private static int displayNum = 0;   //表示されているウィンドウの数

    // Start is called before the first frame update
    void Start()
    {
        ScenarioDataInfo.Instance.pauseFlag = false;
        myRect = GetComponent<RectTransform>();
        childrenuGUIList = GetComponentsInChildren<Graphic>();
        myRect.transform.localScale = new Vector3(initScale, initScale, initScale);
        displayNum = 0;
        foreach(var g in childrenuGUIList)
        {
            g.color = g.color.Alpha(0);
            g.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// PauseMenuを表示する
    /// </summary>
    public void OpenMenu()
    {
        if (ScenarioDataInfo.Instance.pauseFlag && displayNum!=0)
            return;
        displayNum++;
        ScenarioDataInfo.Instance.pauseFlag = true;
        myRect.DOScale(1.0f, duration).SetEase(Ease.OutBack);
        foreach (var g in childrenuGUIList)
        {
            g.gameObject.SetActive(true);
            DOTween.ToAlpha(
                () => g.color               //対象
                , color => g.color = color   //値の更新
                , 1.0f                       //最終的なAlphaの値
                , duration)                  //時間/Easing指定
                .SetEase(Ease.Linear);      //Easingの指定
        }
    }

    /// <summary>
    /// PauseMenuを閉じる
    /// </summary>
    public void CloseMenu()
    {
        displayNum--;
        float closeDuration = duration / 2.0f;
        myRect.DOScale(initScale, closeDuration).SetEase(Ease.Linear);
        foreach (var g in childrenuGUIList)
        {
            DOTween.ToAlpha(
                () => g.color                                  //対象
                , color => g.color = color                      //値の更新
                , 0.0f                                          //最終的なAlphaの値
                , closeDuration)                                     //時間/Easing指定
                .SetEase(Ease.Linear)                          //Easing載せって
                .OnComplete(() => g.gameObject.SetActive(false));//終了時に呼ばれる
        }
    }

    public void PauseEndCheck()
    {
        if (displayNum == 0)
            ScenarioDataInfo.Instance.pauseFlag = false;
    }
}

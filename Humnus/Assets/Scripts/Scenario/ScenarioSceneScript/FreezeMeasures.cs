using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// フリーズ対策用画像の動き
/// </summary>
public class FreezeMeasures : MonoBehaviour
{
    [SerializeField,Header("ジャンプ力")]
    private float jumpPower = 20.0f;
    [SerializeField,Header("ジャンプする回数")]
    private int jumpNum = 1;
    [SerializeField,Header("時間")]
    private float duration = 1.0f;
    private RectTransform myRect = null;
    private Image pattImage = null;

    private Vector3 initLocalPosition;

    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
        initLocalPosition = myRect.localPosition;
        pattImage = GetComponent<Image>();
        pattImage.enabled = false;
    }

    /// <summary>
    /// 画像を表示するかどうか
    /// </summary>
    /// <param name="flag"></param>
    public void PattImageEnabled(bool flag)
    {
        pattImage.enabled = flag;
        myRect.localPosition = initLocalPosition;
    }

    /// <summary>
    /// フリーズ対策用アニメーション
    /// </summary>
    public void FreezeMeasuresAnim()
    {
        myRect.DOKill();
        PattImageEnabled(true);
        myRect.DOLocalJump(
            myRect.localPosition      //目的地
            , jumpPower               //ジャンプする力
            , jumpNum                 //回数
            , duration)               //時間
            .SetLoops(-1)             //ループ
            .SetEase(Ease.Linear);    //Easingの指定
    }
}

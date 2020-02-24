using NerScript;
using NerScript.Anime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayResultPanel : MonoBehaviour
{
    public TMP_Text score = null;
    public TMP_Text scoreB = null;
    public TMP_Text combo = null;
    public TMP_Text comboB = null;
    public Image rank = null;
    public Image rankB = null;

    public void Start()
    {
        score.color = score.color.SetedAlpha(0);
        scoreB.color = scoreB.color.SetedAlpha(0);
        combo.color = combo.color.SetedAlpha(0);
        comboB.color = comboB.color.SetedAlpha(0);
        rank.color = rank.color.SetedAlpha(0);
        rankB.color = rankB.color.SetedAlpha(0);

        score.transform.localScale = new Vector3(4, 4, 4);
        scoreB.transform.localScale = new Vector3(1, 1, 1);

        combo.transform.localScale = new Vector3(4, 4, 4);
        comboB.transform.localScale = new Vector3(1, 1, 1);
        rank.transform.localScale = new Vector3(4, 4, 4);
        rankB.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Effect()
    {
        scoreB.text = score.text;
        comboB.text = combo.text;
        rankB.sprite = rank.sprite;

        gameObject
        .ObjectAnimation()
        //スコア
        .OtherObject(score.gameObject, p =>
        {
            scoreB.enabled = false;
            p
            .Simultaneous(pp =>
            {
                pp.FloatLeapAnim(60, f => score.color = score.color.SetedAlpha(f), EasingTypes.SexticIn);
            })
            .ScaleAbs(1, 60, EasingTypes.SexticIn);
        })
        .OtherObject(scoreB.gameObject, p =>
        {
            p
            .WaitFrame(30)
            .Simultaneous(pp =>
            {
                scoreB.enabled = true;
                pp.FloatLeapAnim(60, f => scoreB.color = scoreB.color.SetedAlpha(1 - f), EasingTypes.SexticIn);
            })
            .ScaleAbs(2, 60, EasingTypes.SexticIn);
        })
        .WaitFrame(40)
        //コンボ
        .OtherObject(combo.gameObject, p =>
        {
            comboB.enabled = false;
            p
            .Simultaneous(pp =>
            {
                pp.FloatLeapAnim(60, f => combo.color = combo.color.SetedAlpha(f), EasingTypes.SexticIn);
            })
            .ScaleAbs(1, 60, EasingTypes.SexticIn);
        })
        .OtherObject(comboB.gameObject, p =>
        {
            p
            .WaitFrame(30)
            .Simultaneous(pp =>
            {
                comboB.enabled = true;
                pp.FloatLeapAnim(60, f => comboB.color = comboB.color.SetedAlpha(1 - f), EasingTypes.SexticIn);
            })
            .ScaleAbs(2, 60, EasingTypes.SexticIn);
        })
        .WaitFrame(40)
        //ランク
        .OtherObject(rank.gameObject, p =>
        {
            rankB.enabled = false;
            p
            .Simultaneous(pp =>
            {
                pp.FloatLeapAnim(60, f => rank.color = rank.color.SetedAlpha(f), EasingTypes.SexticIn);
            })
            .ScaleAbs(1, 60, EasingTypes.SexticIn);
        })
        .OtherObject(rankB.gameObject, p =>
        {
            p
            .WaitFrame(30)
            .Simultaneous(pp =>
            {
                rankB.enabled = true;
                pp.FloatLeapAnim(60, f => rankB.color = rankB.color.SetedAlpha(1 - f), EasingTypes.SexticIn);
            })
            .ScaleAbs(2, 60, EasingTypes.SexticIn);
        })
        .AnimationStart();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NerScript;
using NerScript.Anime;

public class PlayResultToUI : MonoBehaviour
{
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text comboText = null;
    [SerializeField] private ScriptableObjectDatabase musicDatabase = null;
    [SerializeField] private UnityEvent onScoreUIEnd = null;
    [SerializeField] private int scoreCountUpFrame = 120;

    public TMP_Text score = null;
    public TMP_Text combo = null;
    public TMP_Text perfect = null;
    public TMP_Text great = null;
    public TMP_Text miss = null;
    public Image rank = null;

    public Sprite S = null;
    public Sprite A = null;
    public Sprite B = null;
    public Sprite C = null;


    public void ToUI()
    {
        comboText.text = "0";
        scoreText.text = "0";

        score.text = PlayScoreManager.score.InsertComma();
        combo.text = PlayScoreManager.combo.ToString();
        perfect.text = PlayScoreManager.perfectCount.ToString();
        great.text = PlayScoreManager.greatCount.ToString();
        miss.text = PlayScoreManager.missCount.ToString();
        rank.sprite = S;


        scoreText
        .gameObject
        .ObjectAnimation()
        .Simultaneous(p =>
        {
            p
            .WaitFrame(30)
            .IntToIntAnim(scoreCountUpFrame, 0, PlayScoreManager.combo,
                i => comboText.text = i.ToString());
        })
        .IntToIntAnim(scoreCountUpFrame, 0, PlayScoreManager.score,
            (i) => scoreText.text = i.InsertComma(),
            EasingTypes.QuadIn2)
        .AnimationStart(() => onScoreUIEnd.Invoke());
    }

}
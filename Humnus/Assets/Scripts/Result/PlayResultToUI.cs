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
    [SerializeField] private Text maxScoreText = null;
    [SerializeField] private ScriptableObjectDatabase musicDatabase = null;
    [SerializeField] private UnityEvent onScoreUIEnd = null;
    [SerializeField] private int scoreCountUpFrame = 120;



    private void Start()
    {

    }


    private void Update()
    {

    }


    public void ToUI()
    {
        maxScoreText.text = ((MusicData)musicDatabase.objects[PlayResult.musicID])
                             .clearData[PlayResult.difficulty.Int()].maxScore.ToString();
        scoreText.text = "0";

        scoreText.gameObject
        .ObjectAnimation()
        .IntToIntAnim(scoreCountUpFrame, 0, PlayResult.score, (i) => scoreText.text = i.InsertComma(), EasingTypes.QuadIn2)
        .AnimationStart(() => onScoreUIEnd.Invoke());
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript;
using NerScript.Anime;
using NerScript.Attribute;
using UniRx;
using TMPro;



public class PlayScoreManager : MonoBehaviour
{
    [SerializeField, ReadOnlyOnInspector] private int score = 0;
    [SerializeField] private TextMeshProUGUI text = null;
    [SerializeField] private TextMeshProUGUI scoreText = null;




    private ObjectAnimationController textAnimController = null;




    private void Start()
    {

    }


    private void Update()
    {

    }


    public void Score(string msg,int score)
    {
        this.score += score;
        scoreText.text = "Score : " + score;
        text.text = msg;

        textAnimController?.Exit();

        textAnimController =
        text.gameObject.ObjectAnimation()
        .LclMoveRel(0, 20, 0, 60, EasingTypes.BackIn2)
        .LclMoveRel(0, -20, 0, 1)
        .RepeatAnim(1)
        .AnimationStart();
    }
}

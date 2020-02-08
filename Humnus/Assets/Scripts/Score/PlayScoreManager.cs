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



public enum NoteJudge
{
    Perfect,
    Great,
    Good,
    Bad,
    Miss,
}


public class PlayScoreManager : MonoBehaviour
{
    [SerializeField, ReadOnlyOnInspector] private int score;
    public int combo;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;



    private ObjectAnimationController textAnimController = null;




    private void Start()
    {

    }


    private void Update()
    {

    }

    private void Combo(bool isCombo)
    {
        combo++;
        if(isCombo)
        {
            comboText.text =
            combo + "\nCombo!";
        }
        else
        {
            combo = 0;
            comboText.text = combo + "\nCombo!";
        }
    }


    public void Score(NoteJudge judge, int _score)
    {
        int bonusPlus = PlayCharacter.HasPlayer(PlayerID.Type8) ? 2 : 0;
        float bonus = 1 + combo.Digit().ClampedMax(5 + bonusPlus) * 0.1f;
        float tempScore = _score * bonus;
        _score = (int)tempScore;

        score += _score;
        scoreText.text = "Score : " + score;
        text.text = judge.ToString();
        switch(judge)
        {

            case NoteJudge.Perfect:
                Combo(true);
                break;
            case NoteJudge.Great:
                Combo(true);
                break;
            case NoteJudge.Good:
                Combo(true);
                break;
            case NoteJudge.Bad:
                Combo(false);
                break;
            case NoteJudge.Miss:
                if(PlayCharacter.HasPlayer(PlayerID.Type1) && combo <= 100) Combo(true);
                else Combo(false);
                break;
        }

        textAnimController?.Exit();

        textAnimController =
        text.gameObject.ObjectAnimation()
            .LclMoveRel(0, 20, 0, 60, EasingTypes.BackIn2)
            .LclMoveRel(0, -20, 0, 1)
            .RepeatAnim(1)
            .AnimationStart();
    }
}

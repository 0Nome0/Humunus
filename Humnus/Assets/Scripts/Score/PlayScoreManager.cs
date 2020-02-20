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
    public Player player = null;
    [SerializeField, ReadOnlyOnInspector] private int score;
    public int combo;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public bool allPerfect = true;


    private ObjectAnimationController textAnimController = null;




    private void Start()
    {

    }


    private void Update()
    {

    }

    private void Combo(bool isCombo)
    {
        if(isCombo)
        {
            if(PlayCharacter.HasPlayer(PlayerID.Orphia))
            {
                PlayCharacter.Count(PlayerID.Orphia).Value++;
                if(60 < PlayCharacter.Count(PlayerID.Orphia).Value)
                {
                    PlayCharacter.Count(PlayerID.Orphia).Value = 0;
                }
            }
            if(PlayCharacter.HasPlayer(PlayerID.Renos))
            {
                player.hitPoint.Heal(20);
            }
            combo++;
            comboText.text = combo + "\nCombo!";
        }
        else
        {
            if(PlayCharacter.HasPlayer(PlayerID.Orphia))
            {
                PlayCharacter.Count(PlayerID.Orphia).Value = 0;
            }
            combo = 0;
            comboText.text = combo + "\nCombo!";
        }
    }


    public void Score(NoteJudge judge, float _score)
    {
        int bonusPlus = PlayCharacter.HasPlayer(PlayerID.Clemona) ? 2 : 0;
        float bonus = 1 + combo.Digit().ClampedMax(5 + bonusPlus) * 0.1f;
        _score = _score * bonus;

        score += (int)_score;
        scoreText.text = "Score : " + score;
        text.text = judge.ToString();
        switch(judge)
        {

            case NoteJudge.Perfect:
                Combo(true);
                break;
            case NoteJudge.Great:
                Combo(true);
                allPerfect = false;
                break;
            case NoteJudge.Miss:
                allPerfect = false;
                if(PlayCharacter.HasPlayer(PlayerID.Lucius) && combo <= 100) Combo(true);
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

    public void MusicEnd()
    {
        if(PlayCharacter.HasPlayer(PlayerID.Kruvy) && allPerfect)
        {
            score += 5000;
            scoreText.text = "Score : " + score;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript;
using UniRx;


public class MusicDataToUI : MonoBehaviour
{
    [SerializeReference] private MusicUIs UIs = new MusicUIs();
    [SerializeField] private GridPosition grid = null;
    [SerializeField] private SelectMusicLineUper musicDatas = null;


    private void Start()
    {
        grid.OnGrid.Subscribe(grid => OnGrid());

    }

    private void OnGrid()
    {
        ToUI();
    }

    private void Update()
    {

    }

    public void ToUI()
    {
        MusicDifficulty difficulty = MusicDifficultySelecter.musicDifficulty;
        MusicData data = musicDatas.GetMusicData(-grid.CurrentGrid);

        UIs.name.text = data.musicName;
        UIs.autor.text = data.authors;
        UIs.rank.text = data.clearData[difficulty.Int()].clearRank.ToString();
        UIs.score.text = data.clearData[difficulty.Int()].maxScore.ToString();
        UIs.combo.text = data.clearData[difficulty.Int()].maxCombo.ToString();
    }

}

public class MusicUIs
{
    public Text name;
    public Text autor;

    public Text rank;
    public Text score;
    public Text combo;
}
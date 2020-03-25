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
    public AudioSource source = null;
    public Image btnCover = null;
    public Button btn = null;

    private void Start()
    {
        grid.OnGrid.Subscribe(OnGrid);

    }

    private void OnGrid(int grid)
    {
        ToUI(grid);
    }

    public void ToUI(int grid)
    {
        MusicData data = musicDatas.GetMusicData(grid);
        Player.musicID = grid;

        btnCover.enabled = !data.openFlag[0];
        btn.enabled = data.openFlag[0];

        UIs.name.text = data.musicName;
        UIs.autor.text = "作曲：" + data.authors;

        string diff ="";
        for(int i = 0; i < data.difficult; i++)
        {
            diff += "♪";
        }
        UIs.difficalt.text = $"難易度:{diff}";
        UIs.icon.sprite = data.icon;
        source.clip = data.audiClip;
        source.Play();
        source.time = 10;
    }

}

public class MusicUIs
{
    public Text name;
    public Text autor;
    public Text difficalt;
    public Image icon;
}
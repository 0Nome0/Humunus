using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript;
using UniRx;


public class ScenarioDataToUI : MonoBehaviour
{
    [SerializeField] private ScenarioUIs UIs = new ScenarioUIs();
    [SerializeField] private GridPosition grid = null;
    [SerializeField] private SelectScenarioLineUper scenarioDatas = null;

    public Button nextBtn = null;
    public Image btnCover = null;
    public AudioSource audio = null;

    private void Start()
    {
        grid.OnGrid.Subscribe(grid => OnGrid());

    }

    private void OnGrid()
    {
        ToUI();
    }

    public void ToUI()
    {
        ScenarioData data = scenarioDatas.GetScenarioData(grid.CurrentGrid);
        nextBtn.enabled = data.openFlag;
        btnCover.enabled = !data.openFlag;
        UIs.icon.sprite = data.icon;
        audio.clip = data.clip;
        audio.Play();
    }
}

[Serializable]
public class ScenarioUIs
{
    public Image icon;
    //public Text score;
    //public Text combo;
}
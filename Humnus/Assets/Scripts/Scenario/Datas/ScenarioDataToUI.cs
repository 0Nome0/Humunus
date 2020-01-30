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
        ScenarioData data = scenarioDatas.GetScenarioData(grid.CurrentGrid);

        UIs.name.text = data.scenarioTitle;
        UIs.autor.text = data.episode.ToString();
    }

}

[Serializable]
public class ScenarioUIs
{
    public Text name;
    public Text autor;

    //public Text rank;
    //public Text score;
    //public Text combo;
}
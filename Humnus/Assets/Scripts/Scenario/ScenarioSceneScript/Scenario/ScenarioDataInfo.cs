using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ScenarioDataInfo
{
    private static ScenarioDataInfo instance;

    public static ScenarioDataInfo Instance
    {
        get
        {
            if (instance == null)
                instance = new ScenarioDataInfo();

            return instance;
        }
    }

    //コンストラクタ
    public ScenarioDataInfo()
    {
        Initialize();
    }

    private void Initialize()
    {
        scenarioProgress = 1;
        scenarioTextIndex = 0;
        pauseFlag = false;
    }

    public int scenarioProgress;   //全体のシナリオ進捗度

    public int scenarioTextIndex = 0;

    //読み込むCSVファイルの名前を返す
    public string StrScenarioCSV()
    {
        return "Scenario" + scenarioProgress.ToString();
    }

    //シナリオでポーズしているかどうか
    public bool pauseFlag;
    //シナリオでオートで進めるかどうか
    public bool autoFlag;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;


public class ScenarioLoad:MonoBehaviour
{
    //シナリオのデータを読み込む
    public List<string[]> LoadScenarioData()
    {
        List<string[]> scenarioData = new List<string[]>();
        TextAsset csvFile = Resources.Load("Scenario/" + ScenarioDataInfo.Instance.StrScenarioCSV()) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();//一行読み込み
            scenarioData.Add(line.Split(','));
        }
        reader.Close();
        return scenarioData;
    }
}

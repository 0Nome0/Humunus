using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(menuName ="Scenario",fileName ="ScenarioManager")]
public class ScenarioManagement : ScriptableObject
{
    [SerializeField]
    private List<CSVContainer> csvList = new List<CSVContainer>();

    public CSVContainer LoadScenario(Chapter chapter)
    {
        for (int i = 0; i < csvList.Count; i++)
        {
            if (chapter != csvList[i].chapter)
                continue;

            csvList[i].CSVLoader();
            return csvList[i];
        }
        return null;
    }
}

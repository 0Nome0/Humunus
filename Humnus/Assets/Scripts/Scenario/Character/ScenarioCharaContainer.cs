using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScenarioChara",fileName ="CharacterInfo")]
public class ScenarioCharaContainer : ScriptableObject
{
    [SerializeField]
    private List<ScenarioCharacter> characterList;

    public ScenarioCharacter GiveCharacter(CharacterType type)
    {
        for(int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].Type == type)
                return characterList[i];
        }
        return null;
    }
}

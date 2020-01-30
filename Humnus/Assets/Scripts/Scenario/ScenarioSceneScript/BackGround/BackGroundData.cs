using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Scenario/BackGroundData",fileName ="BackGroundData")]
public class BackGroundData : ScriptableObject
{
    [SerializeField]
    private List<SpriteData> data;

    public Sprite GiveSpriteData(string spriteName)
    {
        for(int i = 0; i < data.Count; i++)
        {
            if (data[i].spriteData.name == spriteName)
                return data[i].spriteData;
            continue;
        }
        Debug.LogError("そのデータはありません:" + spriteName);
        return null;
    }
}

[Serializable]
public class SpriteData
{
    public string spriteName = "";
    public Sprite spriteData = null;
}

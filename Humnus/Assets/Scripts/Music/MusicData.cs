using System;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using UnityEngine.AddressableAssets;

public class MusicData : ScriptableObject, IManagedByID
{
    public int id = -1;
    public int ID { get => id; set => id = value; }
    public Sprite icon;
    public string audioName;
    public string musicName;
    public string authors;
    public string info;
    public string notesData;
    public float feverStart;
    public float feverEnd;

    public string openConditions = "";
    public bool IsMusicOpen { get { foreach (var f in openFlag) if (f) return false; return true; } }
    public bool[] openFlag = new bool[(int)MusicDifficulty.Count];
    public MusicClearData[] clearData = new MusicClearData[(int)MusicDifficulty.Count];
}


[Serializable]
public struct MusicClearData
{
    public MusicClearRank clearRank;
    public int maxScore;
    public int maxCombo;
    public int clearCount;
}
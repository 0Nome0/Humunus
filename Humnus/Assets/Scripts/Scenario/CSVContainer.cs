using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public enum Chapter
{
    None = -1,
    Chapter1 = 0,
    Chapter2,
    Chapter3,
    Chapter4,
    Chapter5,
    Chapter6,
    Chapter7,
    Chapter8,
    Chapter9,
    Chapter10,
    Chapter11,
    Chapter12,
    Chapter13,
}

public enum EmotionType
{
    /// <summary>
    /// 通常
    /// </summary>
    Normal = 0,
    /// <summary>
    /// 喜び
    /// </summary>
    Grad,
    /// <summary>
    /// 怒っている
    /// </summary>
    Angry,
    /// <summary>
    /// 悲しい
    /// </summary>
    Sad,
    /// <summary>
    /// 楽しい
    /// </summary>
    Happy,
    /// <summary>
    /// アニメーション
    /// </summary>
    Animation,
}

public enum CharacterType
{
    None = -1,
    Chara1 = 0,
    Chara2,
    Chara3,
    Chara4,
    Chara5,
    Chara6,
    Chara7,
    Chara8,
    Chara9,
    Chara10,
}

[Serializable]
public class CSVContainer
{
    public Chapter chapter = Chapter.None;
    public TextAsset csvFile = null;

    /// <summary>
    /// 表情差分用
    /// </summary>
    [HideInInspector]
    public List<EmotionType> emotionList;
    [HideInInspector]
    public List<string> scenarioList;
    [HideInInspector]
    public List<CharacterType> charaList;

    public void CSVLoader()
    {
        emotionList = new List<EmotionType>();
        scenarioList = new List<string>();
        charaList = new List<CharacterType>();
        StringReader sr = new StringReader(csvFile.text);
        List<string> reader = new List<string>();
        
        while (sr.Peek() != -1)
        {
            reader = sr.ReadLine().Split(',').ToList();
            if (!AddCharaName(reader[0]))
                continue;
            if (!AddEmotion(reader[1]))
                continue;
            scenarioList.Add(reader[2]);
            reader.Clear();
        }
    }

    private bool AddCharaName(string charaName)
    {
        CharacterType type = CharacterType.None;
        if (!TryParse(charaName, out type))
            return false;
        charaList.Add(type);
        return true;
    }

    private bool AddEmotion(string emotionName)
    {
        EmotionType type = EmotionType.Normal;
        if (!TryParse(emotionName, out type))
            return false;
        emotionList.Add(type);
        return true;
    }

    private bool TryParse<T>(string s,out T type) where T:struct
    {
        return Enum.TryParse(s,out type)&&Enum.IsDefined(typeof(T),type);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SmyCustom;

public class CharaSpriteLoader : MonoBehaviour
{
    private List<CharaSpriteData> spriteData;

    private string beforeLoadCharaSpriteName = ""; //前に読み込んだ画像のキャラの名前
    private int dataIndex = 0;                     //データのインデックス

    public void Initialize()
    {
        beforeLoadCharaSpriteName = "";
        dataIndex = 0;
    }

    /// <summary>
    /// 画像データの読み込み
    /// </summary>
    public void LoadSpriteData()
    {
        //指定パス内にあるSpriteを読み込む
        var loadSpriteData = Resources.LoadAll<Sprite>("Image/Character/").ToList();
        spriteData = new List<CharaSpriteData>();
        spriteData.Add(new CharaSpriteData());
        //Queueにまだデータがあるなら続行する
        while (loadSpriteData.Count > 0)
        {
            AddData(loadSpriteData.Dequeue());
        }
        Debug.Log("終了");
    }

    /// <summary>
    /// データを追加する
    /// </summary>
    /// <param name="sprite"></param>
    private void AddData(Sprite sprite)
    {
        //0: キャラの名前 1:表情名
        string[] str_charaData = sprite.name.Split('_');
        //前回のキャラ名が空でなくかつ前回に読み込んだキャラの画像と違うのなら
        if (beforeLoadCharaSpriteName != "" &&
            beforeLoadCharaSpriteName != str_charaData[0])
        {
            dataIndex++;
            spriteData.Add(new CharaSpriteData());
        }

        //キャラデータのキャラ名が設定されていないなら
        if (spriteData[dataIndex].charaName == "")
            spriteData[dataIndex].charaName = str_charaData[0];
        //キャラクタの画像データリストに登録
        spriteData[dataIndex].SetData(str_charaData[1], sprite);

        beforeLoadCharaSpriteName = str_charaData[0];
    }

    /// <summary>
    /// キャラの名前、表情を指定して画像を返す
    /// </summary>
    /// <param name="charaName"></param>
    /// <param name="facialName"></param>
    /// <returns></returns>
    public Sprite GiveCharacterSprite(string charaName,string facialName)
    {
        for(int i = 0; i < spriteData.Count; i++)
        {
            if (spriteData[i].charaName != charaName)
                continue;
            return spriteData[i].GiveSprite(facialName);
        }

        return null;
    }
}

public class CharaSpriteData
{
    public string charaName = "";
    private Dictionary<string, Sprite> data;

    public CharaSpriteData()
    {
        charaName = "";
        data = new Dictionary<string, Sprite>();
    }

    /// <summary>
    /// データをセットする
    /// </summary>
    /// <param name="facialName"></param>
    /// <param name="sprite"></param>
    public void SetData(string facialName, Sprite sprite)
    {
        data.Add(facialName, sprite);
    }

    /// <summary>
    /// 表情からデータを渡す
    /// </summary>
    /// <param name="facialName"></param>
    /// <returns></returns>
    public Sprite GiveSprite(string facialName)
    {
        if (!data.ContainsKey(facialName))
        {
            Debug.LogError("そのキーは存在しません。NotKeyName = " + facialName);
            return null;
        }

        return data[facialName];
    }
}

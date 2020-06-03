using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterNameManager : MonoBehaviour
{
    [SerializeField]
    private Text charaNameText = null;

    private string[] charaNameArray;
    private string[] charaNameArray_J;//日本語（カタカナ表記用）

    private int currentCharaNameIndex => ScenarioDataInfo.Instance.scenarioTextIndex;

    /// <summary>
    /// 現在表示されているキャラクターの名前を取得
    /// </summary>
    public string CurrentCharaName => charaNameArray[currentCharaNameIndex];

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        charaNameText.text = "";
    }

    /// <summary>
    /// データの登録
    /// </summary>
    /// <param name="charaName"></param>
    public void SetScenarioData(string[] charaName)
    {
        if (charaNameArray == null)
            charaNameArray = new string[charaName.Length];
        charaNameArray = charaName;
        charaNameArray_J = new string[charaNameArray.Length];//読み込んだデータ分の配列生成
        Translation();
    }


    /// <summary>
    /// 表示する名前の更新
    /// </summary>
    public void TextUpdate()
    {
        if (charaNameArray.Length <= currentCharaNameIndex)
            return;
        string name = charaNameArray[currentCharaNameIndex];
        if (name == "None")
            charaNameText.text = "";
        else if (name != "")
        {
            if (name.Contains("?"))//名前を隠したいなら
            {
                name = "???";
                //CSVから読み込んだ名前の?を削除する
                charaNameArray[currentCharaNameIndex] = charaNameArray[currentCharaNameIndex].Replace("?", "");
            }
            else
                name = charaNameArray_J[currentCharaNameIndex];
            charaNameText.text = name;
        }
    }

    /// <summary>
    /// 翻訳(どうにかしたい・・・)
    /// </summary>
    private void Translation()
    {
        for (int i = 0; i < charaNameArray.Length; i++)
        {
            switch (charaNameArray[i])
            {
                case "Lucius":
                    charaNameArray_J[i] = "ルキウス";
                    break;
                case "Tem":
                    charaNameArray_J[i] = "テム";
                    break;
                case "Theobalt":
                    charaNameArray_J[i] = "テオバルト";
                    break;
                case "Orphia":
                    charaNameArray_J[i] = "オルフィア";
                    break;
                case "Filo":
                    charaNameArray_J[i] = "フィロ";
                    break;
                case "Nicola":
                    charaNameArray_J[i] = "二コラ";
                    break;
                case "Clemona":
                    charaNameArray_J[i] = "クレモナ";
                    break;
                case "Cello":
                    charaNameArray_J[i] = "セロ";
                    break;
                case "Valen":
                    charaNameArray_J[i] = "ヴァレン";
                    break;
                case "Casartilio":
                    charaNameArray_J[i] = "カサルティリオ";
                    break;
                case "Lycopodia":
                    charaNameArray_J[i] = "リコポディア";
                    break;
                case "Barometer":
                    charaNameArray_J[i] = "バロメッタ";
                    break;
                case "Renos":
                    charaNameArray_J[i] = "リュノス";
                    break;
                case "Kruvy":
                    charaNameArray_J[i] = "クルヴィー";
                    break;
                case "Irishio":
                    charaNameArray_J[i] = "イリシオ";
                    break;
                case "Golem":
                    charaNameArray_J[i] = "ゴーレム";
                    break;
                default:
                    charaNameArray_J[i] = charaNameArray[i];
                    break;
            }
        }
    }

}

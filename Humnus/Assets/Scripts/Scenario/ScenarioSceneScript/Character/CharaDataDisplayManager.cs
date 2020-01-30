using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの名前、スプライト管理
/// </summary>
public class CharaDataDisplayManager : MonoBehaviour
{
    [SerializeField]
    private CharaSpriteLoader spriteLoader = null;
    [SerializeField]
    private CharacterNameManager c_Manager = null;
    [SerializeField]
    private CharaDisplay firstCharacter = null;
    [SerializeField]
    private CharaDisplay secondCharacter = null;

    private string[] charaFacialData;  //表情のデータ
    private int currentFacialIndex => ScenarioDataInfo.Instance.scenarioTextIndex;
    private string beforeTalkCharaName = ""; //前の会話が誰がしゃべっていたか

    #region キャラデータラップ

    /// <summary>
    /// 現在しゃべっているキャラクタ
    /// </summary>
    private string CurrentCharaName => c_Manager.CurrentCharaName;

    /// <summary>
    /// 一人目のキャラクタの名前
    /// </summary>
    private string FirstCharaName => firstCharacter.charaName;

    /// <summary>
    /// 二人目のキャラクタの名前
    /// </summary>
    private string SecondCharaName => secondCharacter.charaName;

    /// <summary>
    /// 進行度に合わせた表情データ
    /// </summary>
    private string FacialName => charaFacialData[currentFacialIndex];

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        spriteLoader.Initialize();
        c_Manager.Initialize();
        firstCharacter.Initialize();
        secondCharacter.Initialize();
        beforeTalkCharaName = "";
    }

    #region データの読み込み

    public void SetDisplayData(string[]firstDisplayData,string[]secondDisplayData)
    {
        firstCharacter.SetDisplayData(firstDisplayData);
        secondCharacter.SetDisplayData(secondDisplayData);
    }

    /// <summary>
    /// 読み込み
    /// </summary>
    /// <param name="facialData"></param>
    /// <param name="charaNameData"></param>
    public void Load(string[] facialData, string[] charaNameData)
    {
        spriteLoader.LoadSpriteData();
        SetFacialData(facialData);
        SetCharaNameData(charaNameData);

        TextUpdate();
    }

    /// <summary>
    /// シナリオデータを取得する
    /// </summary>
    /// <param name="data"></param>
    private void SetFacialData(string[] data)
    {
        if (this.charaFacialData == null)
            this.charaFacialData = new string[data.Length];
        this.charaFacialData = data;
    }

    /// <summary>
    /// キャラクタの名前のデータをセットする
    /// </summary>
    /// <param name="data"></param>
    private void SetCharaNameData(string[] data)
    {
        c_Manager.SetScenarioData(data);
    }

    #endregion

    public void TextUpdate()
    {
        if (charaFacialData.Length <= currentFacialIndex)
            return;
        c_Manager.TextUpdate();
        Debug.Log("しゃべっているキャラ:" + CurrentCharaName);
        Debug.Log("表情:" + FacialName);
        //ナレーション(もしくは同一キャラがしゃべっている)
        if (CurrentCharaName == "")
        {

        }
        else if (CurrentCharaName == "None")
        {
            if (firstCharacter.CharaImage != null)
                firstCharacter.FadeInCharacter();
            if (secondCharacter.CharaImage != null)
                secondCharacter.FadeInCharacter();
        }
        else if (FirstCharaName == CurrentCharaName)
        {
            if (FacialName != "" &&
                firstCharacter.facialName != FacialName)
                firstCharacter.SpriteUpdate(GiveCharaSprite());
            firstCharacter.FadeOutCharacter();
            secondCharacter.FadeInCharacter();
        }
        else if (SecondCharaName == CurrentCharaName)
        {
            if (FacialName != "" &&
                secondCharacter.facialName != FacialName)
                secondCharacter.SpriteUpdate(GiveCharaSprite());
            firstCharacter.FadeInCharacter();
            secondCharacter.FadeOutCharacter();
        }
        else
        {
            //どちらも表示されていないなら
            if (FirstCharaName == "" && SecondCharaName == "")
            {
                firstCharacter.ChangeFadeOutCharacter(GiveCharaSprite());
                UpdateCharaInfo(ref firstCharacter);
            }
            //一人目が表示されていて、二人目が表示されていないなら
            else if (FirstCharaName != "" && SecondCharaName == "")
            {
                secondCharacter.ChangeFadeOutCharacter(GiveCharaSprite());
                UpdateCharaInfo(ref secondCharacter);
                firstCharacter.FadeInCharacter();
            }
            //一人目が表示おらず、二人目が表示されているなら
            else if (FirstCharaName == "" && SecondCharaName != "")
            {
                firstCharacter.ChangeFadeOutCharacter(GiveCharaSprite());
                UpdateCharaInfo(ref firstCharacter);
                secondCharacter.FadeInCharacter();
            }
            //両者ともに表示されていたら
            else
            {
                if (beforeTalkCharaName == FirstCharaName)
                {
                    secondCharacter.ChangeFadeInCharacter(GiveCharaSprite());
                    UpdateCharaInfo(ref secondCharacter);
                    firstCharacter.FadeInCharacter();
                }
                else if (beforeTalkCharaName == SecondCharaName)
                {
                    firstCharacter.ChangeFadeInCharacter(GiveCharaSprite());
                    UpdateCharaInfo(ref firstCharacter);
                    secondCharacter.FadeInCharacter();
                }
            }
        }

        beforeTalkCharaName = CurrentCharaName;
    }

    /// <summary>
    /// 名前と表情を指定して画像を返す
    /// </summary>
    /// <param name="charaName"></param>
    /// <param name="facialName"></param>
    /// <returns></returns>
    private Sprite GiveCharaSprite()
    {
        if (FacialName == "")
            return null;
        return spriteLoader.GiveCharacterSprite(CurrentCharaName, FacialName);
    }

    private void UpdateCharaInfo(ref CharaDisplay chara)
    {
        chara.charaName = CurrentCharaName;
        chara.facialName = FacialName;
    }
}

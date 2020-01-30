using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterSpriteInfomation : MonoBehaviour
{
    [SerializeField]
    private Image standImage = null;
    [SerializeField]
    private Image faceImage = null;

    private RectTransform faceRectTransform = null;
    private ScenarioCharacter charaInfo;
    public CharacterType CharaType { get; private set; }

    private void Start()
    {
        standImage.enabled = false;
        faceImage.enabled = false;
        faceRectTransform = faceImage.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 画像が設定されているかどうか
    /// </summary>
    /// <returns></returns>
    public bool ImageInfo()
    {
        return standImage.enabled == true && faceImage == true;
    }

    /// <summary>
    /// キャラクターを変更する
    /// </summary>
    public void CharacterChange(ScenarioCharacter c, EmotionType type,CharacterSpriteInfomation info)
    {
        standImage.enabled = true;
        faceImage.enabled = true;

        standImage.sprite = c.StandImage;
        faceImage.sprite = c.EmotionList[(int)type].faceImage;
        faceRectTransform.localPosition = c.FacePosition;
        faceRectTransform.sizeDelta = c.FaceScale;
        charaInfo = c;
        CharaType = c.Type;

        info.TalkImageColor(Color.gray, 0.25f);
        TalkImageColor(Color.white, 0.25f);
    }

    /// <summary>
    /// 表情を変更する
    /// </summary>
    /// <param name="faceSprite"></param>
    public void FaceSpriteChange(EmotionType type)
    {
        faceImage.sprite = charaInfo.EmotionList[(int)type].faceImage;
    }

    /// <summary>
    /// しゃべっていないときは暗くする
    /// </summary>
    public void TalkImageColor(Color c,float duration)
    {
        standImage.DOColor(c,duration);
        faceImage.DOColor(c,duration);
    }
}

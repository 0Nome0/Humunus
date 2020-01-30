using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandCharaManager : MonoBehaviour
{
    [SerializeField]
    private ScenarioContainer scenarioContainer = null;
    [SerializeField]
    private ScenarioCharaContainer charaContainer = null;
    [SerializeField]
    private CharacterSpriteInfomation left = null;
    [SerializeField]
    private CharacterSpriteInfomation right = null;
    [SerializeField]
    private RectTransform callOut = null;

    private CharacterType beforeChara;
    // Start is called before the first frame update
    void Start()
    {
        NoTalkCharacter();
        scenarioContainer.OnNext += () =>
        {
            if (!CharacterStandChack())
                NoTalkCharacter();
            else
                StandCharacterChange();
        };
    }

    private bool CharacterStandChack()
    {
        return left.ImageInfo() && right.ImageInfo();
    }

    /// <summary>
    /// まだ立ち絵が設定されていないなら
    /// 設定する
    /// </summary>
    private void NoTalkCharacter()
    {
        ScenarioCharacter c = charaContainer.GiveCharacter(scenarioContainer.LoadCharaType);
        EmotionType type = scenarioContainer.LoadEmotionType;
        this.beforeChara = c.Type;
        if (!left.ImageInfo())
            left.CharacterChange(c,type,right);
        else if (left.CharaType == scenarioContainer.LoadCharaType)
            left.FaceSpriteChange(type);
        else if (!right.ImageInfo())
        {
            right.CharacterChange(c, type,left);
            CalloutStatusChange(new Vector3(-1, 1, 1), new Vector3(28.25f, 0, 0));
        }
    }

    /// <summary>
    /// すでに二人表示しているなら
    /// 表情を変更するかキャラを変える
    /// </summary>
    private void StandCharacterChange()
    {
        CharacterType nowCharaType = scenarioContainer.LoadCharaType;
        EmotionType nowEmotonType = scenarioContainer.LoadEmotionType;
        if (left.CharaType == nowCharaType)
            left.FaceSpriteChange(nowEmotonType);
        else if (right.CharaType == nowCharaType)
            right.FaceSpriteChange(nowEmotonType);
        else if (left.CharaType == beforeChara)
        {
            right.CharacterChange(charaContainer.GiveCharacter(nowCharaType), nowEmotonType,left);
            CalloutStatusChange(new Vector3(-1, 1, 1), new Vector3(30, 0, 0));
        }
        else if (right.CharaType == beforeChara)
        {
            left.CharacterChange(charaContainer.GiveCharacter(nowCharaType), nowEmotonType,right);
            CalloutStatusChange(Vector3.one, Vector3.zero);
        }
        beforeChara = nowCharaType;
    }

    private void CalloutStatusChange(Vector3 scale, Vector3 position)
    {
        callOut.localScale = scale;
        callOut.localPosition = position;
    }
}

using System.Collections.Generic;
using NerScript;
using NerScript.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class FavCharaSelector : MonoBehaviour
{
    public static PlayerID fav = PlayerID.Lucius;
    public Image character = null;
    public ScriptableObjectDatabase charaDB = null;
    public List<LongInput> charas;

    public PopupWindowController popUp = null;
    public CharaDetailWindow dataWindow = null;

    private void Start()
    {
        character.sprite = charaDB.GetObjectByID<CharacterData>(fav.Int()).icon;

        foreach(var chara in charas)
        {
            CharacterData data = charaDB.GetObjectByID<CharacterData>(charas.IndexOf(chara));

            if(!data.isOpened)
            {
                Image im = chara.transform.GetChild(0).GetComponent<Image>();
                im.color = Colors.Black;
                continue;
            }

            chara
            .OnPoint
            .Where(b=> b == LongInput.PointerEvent.Click && !chara.longedInput)
            .Subscribe(_ => { character.sprite = data.icon; });

            chara
            .OnLongPress
            .Subscribe(_ =>
            {
                dataWindow.data = data;
                popUp.Show();
                dataWindow.Setting();
            });
        }
    }
}
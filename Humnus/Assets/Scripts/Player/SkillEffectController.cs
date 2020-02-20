using NerScript;
using NerScript.Anime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class SkillEffectController : MonoBehaviour
{
    public Image white = null;
    public Image line = null;
    public Image Uback = null;
    public Image Dback = null;
    public Image Uchara = null;
    public Image Dchara = null;
    public Image Uframe = null;
    public Image Dframe = null;
    public TMP_Text Utext = null;
    public TMP_Text Dtext = null;
    public Image ready = null;
    public Player player = null;
    public ScriptableObjectDatabase charaDatabase = null;

    private void Start()
    {

        void SetAlpha(Image image, float a)
        {
            image.color = image.color.SetedAlpha(a);
        }
        void SetTextAlpha(TMP_Text text, float a)
        {
            text.color = text.color.SetedAlpha(a);
        }
        float Ratio(float min, float max, float num)
        {
            return (num - min) / (max - min);
        }

        Uchara.sprite = charaDatabase.GetObjectByID<CharacterData>(PlayCharacter.Left.Int()).icon;
        Dchara.sprite = charaDatabase.GetObjectByID<CharacterData>(PlayCharacter.Right.Int()).icon;

        gameObject
        .ObjectAnimation()

        //初期設定
        .PlayActionAnim(() =>
        {
            white.color = white.color.SetedAlpha(0);
            white.enabled = true;
            ((RectTransform)line.transform).sizeDelta = ((RectTransform)line.transform).sizeDelta.SetedX(0);

            SetAlpha(Uback, 0);
            Uback.transform.localPosition = new Vector3(0, 0, 0);
            SetAlpha(Uchara, 0);
            Uframe.transform.localPosition = Uframe.transform.localPosition.SetedX(75);

            SetAlpha(Dback, 0);
            Dback.transform.localPosition = new Vector3(0, 0, 0);
            SetAlpha(Dchara, 0);
            Dframe.transform.localPosition = Dframe.transform.localPosition.SetedX(-75);

            SetAlpha(ready,0);

        })

        //画面フェード
        .FloatToFloatAnim(60, 0, 0.5f, f => { SetAlpha(white, f); })
        //ライン
        .FloatToFloatAnim(30, 0, 120,
            f =>
            {
                ((RectTransform)line.transform).sizeDelta = ((RectTransform)line.transform).sizeDelta.SetedX(f);
            }, EasingTypes.QuadIn2)

        //上
        .Simultaneous(p =>
        {
            p
            //背景
            .FloatToFloatAnim(30, 80, -2.15f, f =>
            {
                SetAlpha(Uback, Ratio(80, -2.15f, f));
                Uback.transform.localPosition = new Vector3(0, f, 0);
            }, EasingTypes.QuadIn2)

            //キャラ
            .FloatToFloatAnim(30, 0, 1, f => { SetAlpha(Uchara, f); })

            //スキル
            .FloatToFloatAnim(30, 75, 45,
                f =>
                {
                    Uframe.transform.localPosition = Uframe.transform.localPosition.SetedX(f);
                    float a = Ratio(75, 45, f);
                    SetAlpha(Uframe, a);
                    SetTextAlpha(Utext, a);
                },
                EasingTypes.QuintIn2);
            ;
        })

        //下
        .Simultaneous(p =>
        {
            p
            //背景
            .FloatToFloatAnim(30, -80, 3.5f, f =>
            {
                SetAlpha(Dback, Ratio(-80, 3.5f, f));
                Dback.transform.localPosition = new Vector3(0, f, 0);
            }, EasingTypes.QuadIn2)

            //キャラ
            .FloatToFloatAnim(30, 0, 1, f => { SetAlpha(Dchara, f); })

            //スキル
            .FloatToFloatAnim(30, -75, -45,
                f =>
                {
                    Dframe.transform.localPosition = Dframe.transform.localPosition.SetedX(f);
                    float a = Ratio(-75, -45, f);
                    SetAlpha(Uframe, a);
                    SetTextAlpha(Utext, a);
                },
                EasingTypes.QuintIn2);
            ;
        })
        .WaitFrame(180)
        .Simultaneous(p =>
        {
            p
            .Simultaneous(pp =>
            {
                pp.FloatToFloatAnim(30, -2.15f, -80, f =>
                {
                    SetAlpha(Uback, Ratio(-80, -2.15f, f));
                    Uback.transform.localPosition = new Vector3(0, f, 0);
                }, EasingTypes.QuadIn2);
            })
            .Simultaneous(pp => { pp.FloatToFloatAnim(30, 1, 0, f => { SetAlpha(Uchara, f); }); })
            .Simultaneous(pp =>
            {
                pp
                .FloatToFloatAnim(30, 45, 75, f =>
                    {
                        Uframe.transform.localPosition = Uframe.transform.localPosition.SetedX(f);
                        float a = Ratio(75, 45, f);
                        SetAlpha(Uframe, a);
                        SetTextAlpha(Utext, a);
                    },
                    EasingTypes.QuintIn2);
            });
        })
        .Simultaneous(p =>
        {
            p
            //背景
            .Simultaneous(pp =>
            {
                pp.FloatToFloatAnim(30, 3.5f, 80, f =>
                {
                    SetAlpha(Dback, Ratio(80, 3.5f, f));
                    Dback.transform.localPosition = new Vector3(0, f, 0);
                }, EasingTypes.QuadIn2);
            })
            //キャラ
            .Simultaneous(pp => { pp.FloatToFloatAnim(30, 1, 0, f => { SetAlpha(Dchara, f); }); })
            .Simultaneous(pp =>
            {
                //スキル
                pp.FloatToFloatAnim(30, -45, -75,
                    f =>
                    {
                        Dframe.transform.localPosition = Dframe.transform.localPosition.SetedX(f);
                        float a = Ratio(-75, -45, f);
                        SetAlpha(Uframe, a);
                        SetTextAlpha(Utext, a);
                    },
                    EasingTypes.QuintIn2);
            })
            ;
        })
        .FloatToFloatAnim(30, 45, 0, f =>
            { line.transform.SetLclRotZ(f); }, EasingTypes.QuadIn2)
        .FloatToFloatAnim(30, 120, 60,
            f =>
            {
                ((RectTransform)line.transform).sizeDelta = ((RectTransform)line.transform).sizeDelta.SetedX(f);
            }, EasingTypes.QuadIn2)
        .FloatToFloatAnim(30, 10, 0, f =>
        {
            SetAlpha(ready,Ratio(10,0,f));
            ready.transform.SetLclPosX(f);
        }, EasingTypes.QuadIn2)
        .WaitFrame(40)
        .FloatToFloatAnim(30, 0, -10, f =>
        {
            SetAlpha(ready,Ratio(-10,0,f));
            ready.transform.SetLclPosX(f);
            ((RectTransform)line.transform).sizeDelta =
            ((RectTransform)line.transform).sizeDelta.SetedX(Ratio(-10,0,f) * 70 - 10 );
            SetAlpha(white, Ratio(-10,0,f) * 0.5f);

        }, EasingTypes.QuadIn2)

        //
        //
        //
        //
        .AnimationStart(() =>
        {
            player.enabled = true;
            gameObject.Inactivate();
        });
    }
}

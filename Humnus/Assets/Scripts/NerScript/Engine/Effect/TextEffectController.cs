using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Toolkit;
using NerScript;
using NerScript.Anime;
using TMPro;
using Random = UnityEngine.Random;

public class TextEffectController : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private bool autoDestroy = true;

    private Pool<TMP_Text> pool;

    private void Start()
    {
        TMP_Text Crete(int i)
        {
            return Instantiate(effectPrefab, transform).GetComponent<TMP_Text>();
        }
        void Initialize(TMP_Text textMesh)
        {
            textMesh.gameObject.SetActive(true);
        }
        void Finalize(TMP_Text textMesh)
        {
            textMesh.gameObject.SetActive(false);
            textMesh.color = textMesh.color.SetedAlpha(1);
        }

        pool = new Pool<TMP_Text>(Crete, Initialize, Finalize);
    }



    public class EffectInfo
    {
        public Vector3 position;
        public Color? textColor;
        public EffectInfo(Vector3 position, Color? textColor)
        {
            this.position = position;
            this.textColor = textColor;
        }
    }

    private TMP_Text Effect(EffectInfo info, string text)
    {
        TMP_Text textMesh = pool.Get();
        textMesh.transform.position = info.position;
        textMesh.text = text;
        textMesh.color = info.textColor ?? textMesh.color;
        return textMesh;
    }

    public void TextEffect(EffectInfo info, string msg)
    {
        TMP_Text text = Effect(info, msg);
        text.text = msg;

        void SetAlpha(float alpha)
        {
            text.color = text.color.SetedAlpha(alpha);
        }

        AnimationPlanner anim =
        text
        .gameObject
        .ObjectAnimation()
        .WaitFrame(40 + Random.Range(0, 5))
        .Simultaneous(p => p.FloatToFloatAnim(30 + Random.Range(0, 5), 1, 0, SetAlpha))
        .MoveRel(0, 1, 0, 80 + Random.Range(0, 10), EasingTypes.QuartIn2);

        if(autoDestroy)
        {
            anim.AnimationStart(() => pool.Used(text));
        }
        else
        {
            anim.AnimationStart();
        }
    }


    public void NumEffect(EffectInfo info, int num)
    {
        int startNum = num / 4;
        if(num < 3)
        {
            startNum = num;
        }
        TMP_Text text = Effect(info, startNum.ToString());

        void SetText(int displayNum)
        {
            text.text = displayNum.ToString();
        }
        void SetAlpha(float alpha)
        {
            text.color = text.color.SetedAlpha(alpha);
        }

        AnimationPlanner anim =
        text
        .gameObject
        .ObjectAnimation()
        .IntToIntAnim(20 + Random.Range(0, 5), startNum, num, SetText, EasingTypes.QuadIn4)
        .WaitFrame(20 + Random.Range(0, 5))
        .Simultaneous(p => p.FloatToFloatAnim(30 + Random.Range(0, 5), 1, 0, SetAlpha))
        .MoveRel(0, 1, 0, 80 + Random.Range(0, 10), EasingTypes.QuartIn2);

        if(autoDestroy)
        {
            anim.AnimationStart(() => pool.Used(text));
        }
        else
        {
            anim.AnimationStart();
        }
    }
}

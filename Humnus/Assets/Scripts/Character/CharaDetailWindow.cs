using System;
using NerScript;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class CharaDetailWindow : MonoBehaviour
{
    public TMP_Text infoText = null;
    public AudioSource source = null;
    public AudioSource bgm = null;

    public Button voice1 = null;
    public Button voice2 = null;
    public Button voice3 = null;
    public Button voiceS = null;

    public Image cover1 = null;
    public Image cover2 = null;
    public Image cover3 = null;
    public Image coverS = null;

    public Image autor = null;
    public Image charactor = null;

    public CharacterData data = null;



    public void Close()
    {
        bgm.volume = 1f;
        source.Stop();
    }

    public void Setting()
    {
        bgm.volume = 0.3f;

        source.clip = data.voice1;
        source.Play();

        infoText.text = data.info;
        autor.sprite = data.iconAu;
        charactor.sprite = data.icon;

        cover1.enabled = cover2.enabled = cover3.enabled = coverS.enabled = false;

        if(data.voice1 == null) { cover1.enabled = true; }
        if(data.voice2 == null) { cover2.enabled = true; }
        if(data.voice3 == null) { cover3.enabled = true; }
        if(data.SVoice == null) { coverS.enabled = true; }


        voice1
        .OnPointerClickAsObservable()
        .Subscribe(_ =>
        {
            source.clip = data.voice1;
            source.Play();
        });
        voice2
        .OnPointerClickAsObservable()
        .Subscribe(_ =>
        {
            source.clip = data.voice2;
            source.Play();
        });
        voice3
        .OnPointerClickAsObservable()
        .Subscribe(_ =>
        {
            source.clip = data.voice3;
            source.Play();
        });
        voiceS
        .OnPointerClickAsObservable()
        .Subscribe(_ =>
        {
            source.clip = data.SVoice;
            source.Play();
        });
    }
}
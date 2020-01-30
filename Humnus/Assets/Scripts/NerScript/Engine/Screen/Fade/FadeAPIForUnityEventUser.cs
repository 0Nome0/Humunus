using NerScript.Games;
using System;
using UnityEngine;
using UnityEngine.Events;


public class FadeAPIForUnityEventUser : MonoBehaviour, IFade
{
    private FadeAPI api = new FadeAPI();

    [SerializeField] private int outFrame = 30;
    [SerializeField] private int inFrame = 30;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private FadeEndEvents events = null;


    public void FadeOut()
    {
        api.FadeOut(outFrame, () => events.onOutEnd.Invoke());
    }
    public void FadeIn()
    {
        api.FadeIn(inFrame, () => events.onInEnd.Invoke());
    }

    public void FadeStart()
    {
        api.FadeStart(outFrame, inFrame, () => events.onBeBetween.Invoke(), () => events.onEnd.Invoke());
    }

    public void FadeColorChange()
    {
        api.FadeColorChange(color);
    }


    public void FadeOut(int frame = 30, Action onOutEnd = null)
    {
        outFrame = frame;
        api.FadeOut(outFrame, onOutEnd);
    }

    public void FadeIn(int frame = 30, Action onInEnd = null)
    {
        inFrame = frame;
        api.FadeIn(inFrame, onInEnd);
    }

    public void FadeStart(int _outFrame = 30, int _inFrame = 30, Action onBeBetween = null, Action onEnd = null)
    {
        outFrame = _outFrame;
        inFrame = _inFrame;
        api.FadeStart(_outFrame, _inFrame, onBeBetween, onEnd);
    }

    public void FadeColorChange(Color color)
    {
        api.FadeColorChange(color);
    }

    [Serializable]
    private class FadeEndEvents
    {
        [SerializeField] public UnityEvent onOutEnd = null;
        [SerializeField] public UnityEvent onInEnd = null;
        [SerializeField] public UnityEvent onBeBetween = null;
        [SerializeField] public UnityEvent onEnd = null;
    }
}

using System;
using NerScript.Games;
using UnityEngine;

public class FadeAPI : IFade
{
    public void FadeOut(int frame = 30, Action onOutEnd = null)
    {
        ScreenManager.FadeSystem.Instance.FadeOut(frame, onOutEnd);
    }
    public void FadeIn(int frame = 30, Action onInEnd = null)
    {
        ScreenManager.FadeSystem.Instance.FadeIn(frame, onInEnd);
    }
    public void FadeStart(int outFrame = 30, int inFrame = 30, Action onBeBetween = null, Action onEnd = null)
    {
        ScreenManager.FadeSystem.Instance.FadeStart(outFrame, inFrame, onBeBetween, onEnd);
    }
    public void FadeColorChange(Color color)
    {
        ScreenManager.FadeSystem.Instance.FadeColorChange(color);
    }
}
using System;
using NerScript.Games;
using UnityEngine;

public interface IFade
{
    void FadeOut(int frame = 30, Action onOutEnd = null);
    void FadeIn(int frame = 30, Action onInEnd = null);
    void FadeStart(int outFrame = 30, int inFrame = 30, Action onBeBetween = null, Action onEnd = null);
    void FadeColorChange(Color color);
}
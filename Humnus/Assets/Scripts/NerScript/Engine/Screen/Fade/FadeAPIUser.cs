using System;
using NerScript.Games;
using UnityEngine;

public class FadeAPIUser : MonoBehaviour
{
    private FadeAPI api = new FadeAPI();

    public void FadeOut(int frame = 30) => api.FadeOut(frame);
    public void FadeIn(int frame = 30) => api.FadeIn(frame);
    public void FadeStart(int frame = 30) => api.FadeStart(frame, frame);
}
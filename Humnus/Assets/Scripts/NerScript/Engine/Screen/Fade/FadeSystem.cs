using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript.Anime;

namespace NerScript.Games
{
    public enum FadeType { Out, In, None }
    public class FadeSystem : SingletonMonoBehaviour<FadeSystem>
    {
        public static FadeType fadeType { get; private set; } = FadeType.None;

        [SerializeField] private Image fadeImage = null;
        [SerializeField] private bool startCover = false;



        public void Initialize()
        {
            instance = this;
            if (startCover) fadeImage.color = fadeImage.color.SetedAlpha(1);
        }

        public void FadeOut(int frame = 30, Action onOutEnd = null)
        {
            if (fadeType != FadeType.None) return;
            fadeType = FadeType.Out;
            fadeImage.gameObject
            .ObjectAnimation()
            .ImageFadeIn(frame)
            .AnimationStart(() => { fadeType = FadeType.None; onOutEnd?.Invoke(); });
        }

        public void FadeIn(int frame = 30, Action onInEnd = null)
        {
            if (fadeType != FadeType.None) return;
            fadeType = FadeType.In;
            fadeImage.gameObject
            .ObjectAnimation()
            .ImageFadeOut(frame)
            .AnimationStart(() => { fadeType = FadeType.None; onInEnd?.Invoke(); });
        }

        public void FadeStart(int outFrame = 30, int inFrame = 30, Action onBeBetween = null, Action onEnd = null)
        {
            if (fadeType != FadeType.None) return;
            void FadeIn()
            {
                onBeBetween?.Invoke();
                this.FadeIn(inFrame, onEnd);
            }
            FadeOut(outFrame, FadeIn);
        }

        public void FadeColorChange(Color color)
        {
            fadeImage.color = color.SetedAlpha(fadeImage.color.a);
        }

    }
}
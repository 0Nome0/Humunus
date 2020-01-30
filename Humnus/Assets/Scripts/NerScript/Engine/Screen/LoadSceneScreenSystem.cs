using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript.Anime;
using UnityEngine.SceneManagement;
using UniRx;

namespace NerScript.Games
{
    public class LoadSceneScreenSystem : SingletonMonoBehaviour<LoadSceneScreenSystem>
    {
        [SerializeField] private Slider slider = null;


        public void Initialize()
        {
            instance = this;
            gameObject.SetActive(false);
            slider.value = 0;
        }

        public void Show(Subject<float> progress)
        {
            gameObject.SetActive(true);
            progress.Subscribe(
                f => slider.value = f,
                () => Hide()
                );
        }
        public void Hide()
        {
            slider.value = 0;
            gameObject.SetActive(false);
        }
    }
}
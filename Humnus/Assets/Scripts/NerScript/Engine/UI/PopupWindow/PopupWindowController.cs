using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript.Attribute;
using NerScript.Anime;
using UniRx;

namespace NerScript.UI
{
    public class PopupWindowController : MonoBehaviour
    {
        public PopUpSetting showSetting = null;
        public PopUpSetting hideSetting = null;

        public bool showPopup { get; private set; } = false;
        private bool isShow = false;


        private void Start()
        {
            isShow = false;
            transform.SetScl(0);
        }

        public void Show()
        {
            isShow = true;
            PopupStateChange();
            DisableButtons();
            gameObject
            .ObjectAnimation()
            .ScaleAbs(1, showSetting.flame, showSetting.easing)
            .AnimationStart(() => EnableButtons());
        }
        public void Hide()
        {
            isShow = false;
            EnableButtons();
            gameObject
            .ObjectAnimation()
            .ScaleAbs(0, hideSetting.flame, hideSetting.easing)
            .AnimationStart(() =>
            {
                DisableButtons();
                PopupStateChange();
            });
        }

        private void PopupStateChange()
        {
            gameObject.SetActive(isShow);
            showPopup = isShow;
        }

        private void DisableButtons()
        {
            foreach (var button in showSetting.buttons)
            {
                button.enabled = !isShow;
            }

        }
        private void EnableButtons()
        {
            foreach (var button in hideSetting.buttons)
            {
                button.enabled = !isShow;
            }
        }



        [Serializable]
        public class PopUpSetting
        {
            public List<Button> buttons = null;
            public int flame = 1;
            [SearchableEnum] public EasingTypes easing = EasingTypes.LineIn;
        }

    }
}
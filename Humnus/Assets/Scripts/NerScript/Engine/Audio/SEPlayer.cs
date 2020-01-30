using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Attribute;
using NerScript.Anime;

namespace NerScript.Resource
{
    public class SEPlayer : MonoBehaviour
    {
        [SerializeField, SEName] private string seName = "";
        [SerializeField, Range(0, 1)] float volume = 1.0f;
        [SerializeField] private SEChannel channel = SEChannel.ch0;
        [SerializeField, InspectorButton("再生", "Play")] private bool play = false;

        public void Play()
        {
            AudioAPI.PlaySE(seName, volume, channel);
        }
    }
}
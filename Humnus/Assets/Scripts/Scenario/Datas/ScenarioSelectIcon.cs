﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript;
using NerScript.Anime;


namespace NerScript
{
    public class ScenarioSelectIcon : MonoBehaviour
    {
        public Image image = null;
        public Text text = null;

        [SerializeField] private Vector3 anker = new Vector3();
        [SerializeField] private float outRange = 1;
        [SerializeField] private EasingTypes easing = EasingTypes.LineIn;

        [SerializeField] private TransformData max = new TransformData();
        [SerializeField] private TransformData min = new TransformData();

        [SerializeField] private float l = 0;


        public void Start()
        {
            max.position.y = min.position.y + transform.localPosition.y;
            min.position.y = transform.localPosition.y;
        }


        public void Update()
        {
            float dist = transform.position.y - anker.y;
            dist = Mathf.Abs(dist);
            l = dist / outRange;
            l.ClampMax(1);
            Lerp(l);
        }

        public void Lerp(float lerp)
        {
            TransformData current = min + (max - min) * easing.Easing(lerp);
            current.SetToLocalTransform(transform);
        }
    }
}
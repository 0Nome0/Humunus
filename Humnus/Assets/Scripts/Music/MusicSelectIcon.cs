using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using NerScript.Anime;
using TMPro;
using UnityEngine.UI;


namespace NerScript
{
    public class MusicSelectIcon : MonoBehaviour
    {
        public TMP_Text text = null;
        public Image cover = null;
        [SerializeField] private Vector3 anker = new Vector3();
        [SerializeField] private float outRange = 1;
        [SerializeField] private EasingTypes easing = EasingTypes.LineIn;

        [SerializeField] private TransformData max = new TransformData();
        [SerializeField] private TransformData min = new TransformData();

        [SerializeField] private float l = 0;


        public void Start()
        {
            float lPosY = transform.localPosition.y;
            max.position.y = min.position.y + lPosY;
            min.position.y = lPosY;
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
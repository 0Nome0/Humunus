using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using NerScript.Anime;


namespace NerScript
{
    public class MusicSelectIcon : MonoBehaviour
    {
        [SerializeField] private Vector3 anker = new Vector3();
        [SerializeField] private float outRange = 1;
        [SerializeField] private EasingTypes easing = EasingTypes.LineIn;

        [SerializeField] private TransformData max = new TransformData();
        [SerializeField] private TransformData min = new TransformData();

        [SerializeField] private float l = 0;


        public void Start()
        {
            max.position.x = transform.localPosition.x;
            min.position.x = transform.localPosition.x;
        }


        public void Update()
        {
            float dist = transform.position.x - anker.x;
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using UniRx;

namespace NerScript.Accessory
{
    public class SimpleLookAter : MonoBehaviour, IObjectAccessory
    {
        ObjectAccessoryType IObjectAccessory.AccessoryType => ObjectAccessoryType.LookAter;
        [SerializeField] private Transform targetT = null;
        [SerializeField] private Vector3 targetV = new Vector3();

        public void SetTransform(Transform tr) { targetT = tr; }
        public void SetVector3(Vector3 pos) { targetV = pos; }

        [ContextMenu("LookAt")]
        public void LookAt()
        {
            if (targetT == null)
            {
                transform.LookAt(targetV);
            }
            else
            {
                transform.LookAt(targetT);
            }
        }

        public void Update()
        {
            LookAt();
        }
    }
}

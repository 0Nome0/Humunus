using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Accessory
{
    public class SimpleRotater : MonoBehaviour, IObjectAccessory
    {
        ObjectAccessoryType IObjectAccessory.AccessoryType => ObjectAccessoryType.Rotater;
        public enum Axis { X, Y, Z }
        [SerializeField] private Axis axis = Axis.X;
        [SerializeField] private float speed = 1.0f;

        private void Update()
        {
            Vector3 vel = Vector3.zero;
            if (axis == Axis.X) vel = new Vector3(speed, 0, 0);
            else if (axis == Axis.Y) vel = new Vector3(0, speed, 0);
            else if (axis == Axis.Z) vel = new Vector3(0, 0, speed);
            transform.localEulerAngles += vel;
        }
    }
}

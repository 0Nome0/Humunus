using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace NerScript
{
    [Serializable]
    public struct TransformData
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public TransformData(Vector3 _position, Vector3 _rotation, Vector3 _scale)
        {
            position = _position;
            rotation = _rotation;
            scale = _scale;
        }

        public void SetToTransform(Transform tr)
        {
            tr.position = position;
            tr.eulerAngles = rotation;
            tr.localScale = scale;
        }

        public void SetToLocalTransform(Transform tr)
        {
            tr.localPosition = position;
            tr.localEulerAngles = rotation;
            tr.localScale = scale;
        }

        public void SetToLocalTransform(Transform tr, bool x, bool y, bool z)
        {         
            tr.localPosition = position;
            tr.localEulerAngles = rotation;
            tr.localScale = scale;
        }

        public static TransformData operator +(TransformData t1, TransformData t2)
        {
            return new TransformData(
                t1.position + t2.position,
                t1.rotation + t2.rotation,
                t1.scale + t2.scale);
        }

        public static TransformData operator -(TransformData t1, TransformData t2)
        {
            return new TransformData(
                t1.position - t2.position,
                t1.rotation - t2.rotation,
                t1.scale - t2.scale);
        }
        public static TransformData operator *(TransformData tr, float s)
        {
            return new TransformData(
                tr.position * s,
                tr.rotation * s,
                tr.scale * s);
        }
        public static TransformData operator /(TransformData tr, float s)
        {
            return new TransformData(
                tr.position * s,
                tr.rotation * s,
                tr.scale * s);
        }

    }
}

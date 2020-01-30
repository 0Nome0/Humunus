using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace NerScript
{

    public enum ValueDataType
    {
        None = -1,
        Multi = 0,
        Bool = 1,
        Int = 2,
        String = 3,
        Float = 4,
        Vector2 = 5,
        Vector3 = 6,
    }

    [Serializable]
    public class ValueData
    {
        public ValueDataType type = ValueDataType.None;
        public bool Bool = false;
        public int Int = 0;
        public string String = "";
        public float Float = 0.0f;
        public Vector2 Vector2 = new Vector2();
        public Vector3 Vector3 = new Vector3();


        public ValueData() { }

        public ValueData(ValueData value)
        {
            Bool = value.Bool;
            Int = value.Int;
            Float = value.Float;
            String = value.String;
            Vector2 = value.Vector2;
            Vector3 = value.Vector3;
        }
        public object GetValue()
        {
            switch (type)
            {
                case ValueDataType.Bool: return Bool;
                case ValueDataType.Int: return Int;
                case ValueDataType.String: return String;
                case ValueDataType.Float: return Float;
                case ValueDataType.Vector2: return Vector2;
                case ValueDataType.Vector3: return Vector3;
            }
            return default;
        }
        public object GetValue(ValueDataType _type)
        {
            switch (_type)
            {
                case ValueDataType.Bool: return Bool;
                case ValueDataType.Int: return Int;
                case ValueDataType.String: return String;
                case ValueDataType.Float: return Float;
                case ValueDataType.Vector2: return Vector2;
                case ValueDataType.Vector3: return Vector3;
            }
            return default;
        }

        public void SetValue(object value)
        {
            switch (value)
            {
                case bool val: Bool = val; break;
                case int val: Int = val; break;
                case string val: String = val; break;
                case float val: Float = val; break;
                case Vector2 val: Vector2 = val; break;
                case Vector3 val: Vector3 = val; break;
            }
        }

        public void SetValue(ValueDataType _type, object value)
        {
            switch (_type)
            {
                case ValueDataType.Bool: Bool = (bool)value; break;
                case ValueDataType.Int: Int = (int)value; break;
                case ValueDataType.String: String = (string)value; break;
                case ValueDataType.Float: Float = (float)value; break;
                case ValueDataType.Vector2: Vector2 = (Vector2)value; break;
                case ValueDataType.Vector3: Vector3 = (Vector3)value; break;
            }
        }

        public void SetValue(ValueData data)
        {
            Bool = data.Bool;
            Int = data.Int;
            Float = data.Float;
            String = data.String;
            Vector2 = data.Vector2;
            Vector3 = data.Vector3;
        }

        public void Inversion()
        {
            Bool = !Bool;
            Int = -Int;
            Float = -Float;
            //String = String;
            Vector2 = -Vector2;
            Vector3 = -Vector3;
        }

        public void Add(ValueData data)
        {
            Bool |= data.Bool;
            Int += data.Int;
            Float += data.Float;
            String += data.String;
            Vector2 += data.Vector2;
            Vector3 += data.Vector3;
        }

        public static ValueData Add(ValueData v1, ValueData v2) { v1.Add(v2); return v1; }

        public void Multiple(ValueData value)
        {
            Bool &= value.Bool;
            Int *= value.Int;
            Float *= value.Float;
            String = value.String;
            Vector2 *= value.Vector2;
            Vector3.x *= value.Vector3.x;
            Vector3.y *= value.Vector3.y;
            Vector3.z *= value.Vector3.z;
        }
        public static ValueData Multiple(ValueData v1, ValueData v2) { v1.Multiple(v2); return v1; }
    }
}

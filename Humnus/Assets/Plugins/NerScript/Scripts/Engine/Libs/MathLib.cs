using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NerScript
{
    public static class MathLib
    {

        #region Contains

        public static bool ContainsIn(this int self, int min, int max) => min <= self && self <= max;
        public static bool ContainsIn(this float self, float min, float max) => min <= self && self <= max;

        #endregion


        public static int Repeat(this ref int self, int length)
        {
            self %= length;
            if(0 <= self) return self;
            return (length + self);
        }

        public static int Digit(this int num, int digit = 1) => num / (int)Mathf.Pow(10, digit);


        #region Clamp

        public static void Clamp(this ref int self, int min, int max)
        {
            if(self < min) self = min;
            else if(max < self) self = max;
        }
        public static void ClampMin(this ref int self, int min)
        {
            if(self < min) self = min;
        }
        public static void ClampMax(this ref int self, int max)
        {
            if(max < self) self = max;
        }

        public static void Clamp(this ref float self, float min, float max)
        {
            if(self < min) self = min;
            else if(max < self) self = max;
        }
        public static void ClampMin(this ref float self, float min)
        {
            if(self < min) self = min;
        }
        public static void ClampMax(this ref float self, float max)
        {
            if(max < self) self = max;
        }

        #endregion

        #region Clamped

        public static int Clamped(this int self, int min, int max)
        {
            if(self < min) self = min;
            else if(max < self) self = max;
            return self;
        }
        public static int ClampedMin(this int self, int min)
        {
            if(self < min) self = min;
            return self;
        }
        public static int ClampedMax(this int self, int max)
        {
            if(max < self) self = max;
            return self;
        }

        public static float Clamped(this float self, float min, float max)
        {
            if(self < min) self = min;
            else if(max < self) self = max;
            return self;
        }
        public static float ClampedMin(this float self, float min)
        {
            if(self < min) self = min;
            return self;
        }
        public static float ClampedMax(this float self, float max)
        {
            if(max < self) self = max;
            return self;
        }

        #endregion

    }
}

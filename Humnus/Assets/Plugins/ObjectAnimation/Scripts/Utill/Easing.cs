using UnityEngine;
using System;
using System.Collections.Generic;

namespace NerScript.Anime
{
    using FloatFunc = Func<float, float>;

    public enum EasingType
    {
        Line, Quad, Cubic, Quart, Quint, Sextic,
        Sin,
        Exp,
        Circ,
        Elastic,
        Back,
        Bounce,
    }

    public enum EasingOptType
    {
        In, In2, In3, In4, Out, Out2, Out3, Out4,
    }


    public enum EasingTypes
    {
        None = -1,
        LineIn = EasingHelper.ETD * 0, LineOut = EasingHelper.ETD * 0 + 4,
        QuadIn = EasingHelper.ETD * 1, QuadIn2, QuadIn3, QuadIn4, QuadOut, QuadOut2, QuadOut3, QuadOut4,
        CubicIn = EasingHelper.ETD * 2, CubicIn2, CubicIn3, CubicIn4, CubicOut, CubicOut2, CubicOut3, CubicOut4,
        QuartIn = EasingHelper.ETD * 3, QuartIn2, QuartIn3, QuartIn4, QuartOut, QuartOut2, QuartOut3, QuartOut4,
        QuintIn = EasingHelper.ETD * 4, QuintIn2, QuintIn3, QuintIn4, QuintOut, QuintOut2, QuintOut3, QuintOut4,
        SexticIn = EasingHelper.ETD * 5, SexticIn2, SexticIn3, SexticIn4, SexticOut, SexticOut2, SexticOut3,
        SexticOut4,
        SineIn = EasingHelper.ETD * 6, SineIn2, SineIn3, SineIn4, SineOut, SineOut2, SineOut3, SineOut4,
        ExpIn = EasingHelper.ETD * 7, ExpIn2, ExpIn3, ExpIn4, ExpOut, ExpOut2, ExpOut3, ExpOut4,
        CircIn = EasingHelper.ETD * 8, CircIn2, CircIn3, CircIn4, CircOut, CircOut2, CircOut3, CircOut4,
        ElasticIn = EasingHelper.ETD * 9, ElasticIn2, ElasticIn3, ElasticIn4, ElasticOut, ElasticOut2, ElasticOut3,
        ElasticOut4,
        BackIn = EasingHelper.ETD * 10, BackIn2, BackIn3, BackIn4, BackOut, BackOut2, BackOut3, BackOut4,
        BounceIn = EasingHelper.ETD * 11, BounceIn2, BounceIn3, BounceIn4, BounceOut, BounceOut2, BounceOut3,
        BounceOut4,
    }

    public static class EasingHelper
    {
        public const int ETD = 10;

        #region InOut

        public static EasingTypes In(this EasingType type) => (EasingTypes)((int)type * ETD + EasingOptType.In);
        public static EasingTypes In2(this EasingType type) => (EasingTypes)((int)type * ETD + EasingOptType.In2);
        public static EasingTypes In3(this EasingType type) => (EasingTypes)((int)type * ETD + EasingOptType.In3);
        public static EasingTypes In4(this EasingType type) => (EasingTypes)((int)type * ETD + EasingOptType.In4);
        public static EasingTypes Out(this EasingType type) => (EasingTypes)((int)type * ETD + EasingOptType.Out);
        public static EasingTypes Out2(this EasingType type) => (EasingTypes)((int)type * ETD + EasingOptType.Out2);
        public static EasingTypes Out3(this EasingType type) => (EasingTypes)((int)type * ETD + EasingOptType.Out3);
        public static EasingTypes Out4(this EasingType type) => (EasingTypes)((int)type * ETD + EasingOptType.Out4);

        #endregion

        public static float Easing(this EasingTypes type, float t) => Anime.Easing.GetEasing(t, type);
        public static EasingTypes OutToIn(this EasingTypes type) =>
        (4 <= (int)type % 10) ? (EasingTypes)(int)(type - 4) : type;


    }

    public static class Easing
    {
        private const float T = 1F;       //1
        private const float Th = 1F / 2F; //0.5
        private const float PiOv2 = Mathf.PI / 2;
        public static float BackCoefficient = 1.0F; //Backの戻る値

        private static readonly List<Func<float, float>> easings = new List<Func<float, float>>()
        {
            Line, Quad, Cubic, Quart, Quint, Sextic,
            Sin, Exp, Circle, Elastic, Back, Bounce,
        };

        public static float GetEasing(float t, EasingTypes type)
        {
            int easingTypeNum = (int)type / EasingHelper.ETD;
            int easingOptType = (int)type % EasingHelper.ETD;
            Func<float, float> easing = easings[easingTypeNum];

            switch(easingOptType)
            {
                default: return In(easing, t);
                case 1:  return In2(easing, t);
                case 2:  return In3(easing, t);
                case 3:  return In4(easing, t);
                case 4:  return Out(easing, t);
                case 5:  return Out2(easing, t);
                case 6:  return Out3(easing, t);
                case 7:  return Out4(easing, t);
            }
        }

        public static float GetStart(EasingTypes type) => GetEasing(0, type);
        public static float GetEnd(EasingTypes type) => GetEasing(1, type);


        private static float In(FloatFunc easing, float f) => easing(f);
        private static float In2(FloatFunc easing, float f) => T - easing(T - f);
        private static float In3(FloatFunc easing, float f)
            => f < Th ? In(easing, f * 2) / 2 : In2(easing, (f - Th) * 2) / 2 + Th;
        private static float In4(FloatFunc easing, float f)
            => f < Th ? In2(easing, f * 2) / 2 : In(easing, (f - Th) * 2) / 2 + Th;
        private static float Out(FloatFunc easing, float f) => T - In(easing, f);
        private static float Out2(FloatFunc easing, float f) => T - In2(easing, f);
        private static float Out3(FloatFunc easing, float f) => T - In3(easing, f);
        private static float Out4(FloatFunc easing, float f) => T - In4(easing, f);


        public static Func<float, float> GetEasingFunc(EasingTypes type)
        {
            int easingTypeNum = (int)type / EasingHelper.ETD;
            int easingOptType = (int)type % EasingHelper.ETD;
            Func<float, float> easing = easings[easingTypeNum];
            return easing;
        }

        private static float Line(float t) => t;
        private static float Quad(float t) => t * t;
        private static float Cubic(float t) => t * t * t;
        private static float Quart(float t) => t * t * t * t;
        private static float Quint(float t) => t * t * t * t * t;
        private static float Sextic(float t) => t * t * t * t * t * t;
        private static float Sin(float t) => Mathf.Sin(t * PiOv2);
        private static float Exp(float t) => t == 0.0F ? 0 : Mathf.Pow(2, 10 * (t - 1));
        private static float Circle(float t) => T - Mathf.Sqrt(1 - t * t);
        private static float Elastic(float t) => t % 1 == 0 ? t : -(Exp(t) * Sin(t * 15));
        private static float Back(float t) => Cubic(t) * (T + BackCoefficient) - BackCoefficient * Quad(t);
        private static float Bounce(float t)
        {
            if(t < 4.0f / 11.0F) return 7.5625f * Quad(t);
            else if(t < 8.0f / 11.0F) return 7.5625f * Quad(t - 6.0F / 11.0F) + 0.75f;
            else if(t < 10f / 11.0F) return 7.5625f * Quad(t - 9.0F / 11.0F) + 0.9375f;
            else return 7.5625f * Quad(t - 10.5f / 11.0f) + 0.984375f;
        }
        private static float Bounce_PROT(float t)
        {
            if(t % 1 == 0) return t;
            float t2 = t * 1.26F + 1.26F;
            return Mathf.Abs(2 - GetEasing(t2 * t2 * t2, EasingType.Sin.In2())) / 2 *
                   GetEasing(t2 * t2 * t2, EasingType.Exp.Out()) + Exp(t);
        }
    }
}

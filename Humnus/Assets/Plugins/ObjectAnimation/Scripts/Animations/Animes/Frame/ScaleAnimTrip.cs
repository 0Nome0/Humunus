using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.Anime
{

    public static partial class ObjectAnim
    {
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="scale"></param>
        public static AnimationPlanner TripScaleAbs(
            this AnimationPlanner animation, Vector3 scale, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Scale()
            .ScaleAbs(scale, oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .ScaleAbs(() => memory.memory[ObjectAnimMemoryType._Scale], oneWayFrame, easing);
        }
        public static AnimationPlanner TripScaleAbs(
            this AnimationPlanner animation, float sclX, float sclY, float sclZ, int oneWayFrame, int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Scale()
            .ScaleAbs(new Vector3(sclX, sclY, sclZ), oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .ScaleAbs(() => memory.memory[ObjectAnimMemoryType._Scale], oneWayFrame, easing);
        }
        public static AnimationPlanner TripScaleAbs(
            this AnimationPlanner animation, float scl, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Scale()
            .ScaleAbs(scl, oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .ScaleAbs(() => memory.memory[ObjectAnimMemoryType._Scale], oneWayFrame, easing);
        }
        public static AnimationPlanner TripScaleRel(
            this AnimationPlanner animation, Vector3 vector, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Scale()
            .ScaleRel(vector, oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .ScaleAbs(() => memory.memory[ObjectAnimMemoryType._Scale], oneWayFrame, easing);
        }
        public static AnimationPlanner TripScaleRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Scale()
            .ScaleRel(new Vector3(vecX, vecY, vecZ), oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .ScaleAbs(() => memory.memory[ObjectAnimMemoryType._Scale], oneWayFrame, easing);
        }
        public static AnimationPlanner TripScaleRel(
            this AnimationPlanner animation, float vec, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Scale()
            .ScaleRel(vec, oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .ScaleAbs(() => memory.memory[ObjectAnimMemoryType._Scale], oneWayFrame, easing);
        }
    }
}

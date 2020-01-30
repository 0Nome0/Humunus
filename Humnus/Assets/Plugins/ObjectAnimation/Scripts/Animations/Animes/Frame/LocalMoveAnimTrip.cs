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
        /// <param name="position"></param>
        public static AnimationPlanner TripLclMoveAbs(
            this AnimationPlanner animation, Vector3 position, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_LocalPosition()
            .LclMoveAbs(position, oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .LclMoveAbs(() => memory.memory[ObjectAnimMemoryType._LocalPosition], oneWayFrame, easing);
        }
        public static AnimationPlanner TripLclMoveAbs(
            this AnimationPlanner animation, float posX, float posY, float posZ, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_LocalPosition()
            .LclMoveAbs(new Vector3(posX, posY, posZ), oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .LclMoveAbs(() => memory.memory[ObjectAnimMemoryType._LocalPosition], oneWayFrame, easing);
        }
        public static AnimationPlanner TripLclMoveRel(
            this AnimationPlanner animation, Vector3 vector, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_LocalPosition()
            .LclMoveRel(vector, oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .LclMoveAbs(() => memory.memory[ObjectAnimMemoryType._LocalPosition], oneWayFrame, easing);
        }
        public static AnimationPlanner TripLclMoveRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_LocalPosition()
            .LclMoveRel(new Vector3(vecX, vecY, vecZ), oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .LclMoveAbs(() => memory.memory[ObjectAnimMemoryType._LocalPosition], oneWayFrame, easing);
        }
    }
}

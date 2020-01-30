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
        public static AnimationPlanner TripMoveAbs(
            this AnimationPlanner animation, Vector3 position, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Position()
            .MoveAbs(position, oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .MoveAbs(() => memory.memory[ObjectAnimMemoryType._Position], oneWayFrame, easing);
        }
        public static AnimationPlanner TripMoveAbs(
            this AnimationPlanner animation, float posX, float posY, float posZ, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Position()
            .MoveAbs(new Vector3(posX, posY, posZ), oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .MoveAbs(() => memory.memory[ObjectAnimMemoryType._Position], oneWayFrame, easing);
        }
        public static AnimationPlanner TripMoveRel(
            this AnimationPlanner animation, Vector3 vector, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Position()
            .MoveRel(vector, oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .MoveAbs(() => memory.memory[ObjectAnimMemoryType._Position], oneWayFrame, easing);
        }
        public static AnimationPlanner TripMoveRel(
            this AnimationPlanner animation, float vecX, float vecY, float vecZ, int oneWayFrame,
            int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Position()
            .MoveRel(new Vector3(vecX, vecY, vecZ), oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .MoveAbs(() => memory.memory[ObjectAnimMemoryType._Position], oneWayFrame, easing);
        }
    }
}

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
        /// <param name="rotation"></param>
        public static AnimationPlanner TripLclRotateAbs(this AnimationPlanner animation, Vector3 rotation, int oneWayFrame, int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Rotation(memory)
            .RotateAbs(rotation, oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .RotateAbs(() => memory.memory[ObjectAnimMemoryType._Rotation], oneWayFrame, easing);
        }
        public static AnimationPlanner TripLclRotateAbs(this AnimationPlanner animation, float rotX, float rotY, float rotZ, int oneWayFrame, int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Rotation(memory)
            .RotateAbs(new Vector3(rotX, rotY, rotZ), oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .RotateAbs(() => memory.memory[ObjectAnimMemoryType._Rotation], oneWayFrame, easing);
        }
        public static AnimationPlanner TripLclRotateRel(this AnimationPlanner animation, Vector3 vector, int oneWayFrame, int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Rotation(memory)
            .RotateRel(vector, oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .RotateAbs(() => memory.memory[ObjectAnimMemoryType._Rotation], oneWayFrame, easing);
        }
        public static AnimationPlanner TripLclRotateRel(this AnimationPlanner animation, float vecX, float vecY, float vecZ, int oneWayFrame, int waitBeyond = 0, EasingTypes easing = EasingTypes.LineIn)
        {
            ObjectAnimationMemoryUser memory = new ObjectAnimationMemoryUser(animation.memory);
            return
            animation
            .Memory_Rotation(memory)
            .RotateRel(new Vector3(vecX, vecY, vecZ), oneWayFrame, easing)
            .WaitFrame(waitBeyond)
            .RotateAbs(() => memory.memory[ObjectAnimMemoryType._Rotation], oneWayFrame, easing);
        }
    }
}

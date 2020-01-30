using UnityEngine;

namespace NerScript.Anime
{
    public static class ObjectAnimationTemplate
    {
        #region FadeSlide
        public static AnimationPlanner FadeSlideIn(this AnimationPlanner animation, AssignmentPointer<float> alpha,
            Vector3 vector, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return
            animation
            .Simultaneous(p => p.FloatLeapAnim(frame, f => alpha.Value = f, easing))
            .MoveRel(vector, frame, easing)
            ;
        }

        public static AnimationPlanner FadeSlideOut(this AnimationPlanner animation, AssignmentPointer<float> alpha,
            Vector3 vector, int frame, EasingTypes easing = EasingTypes.LineIn)
        {
            return
            animation
            .Simultaneous(p => p.FloatLeapAnim(frame, f => alpha.Value = (1 - f), easing))
            .MoveRel(-vector, frame, easing)
            ;
        }
        #endregion














    }
}

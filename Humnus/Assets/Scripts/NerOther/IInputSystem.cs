using System;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using Object = UnityEngine.Object;
using System.Linq;

namespace NerScript.Input
{
    using Input = UnityEngine.Input;

    public interface IInputSystem
    {
        Vector3 PreviousMousePosition { get; }
        Vector3 CurrentMousePosition { get; }
        Vector3 MouseMoveDistance { get; }
        Vector3 MouseMoveDirection { get; }
        bool LeftClick { get; }
        bool OnLeftClick { get; }


        List<Touch> PreviousTouches { get; }
        List<Touch> CurrentTouches { get; }
        Touch? PreviousTouch { get; }
        Touch? CurrentTouch { get; }
        Vector3 PreviousTouchPosition { get; }
        Vector3 CurrentTouchPosition { get; }
        Vector3? TouchMoveDistance { get; }
        Vector3? TouchMoveDirection { get; }
        bool Touch { get; }
        bool OnTouch { get; }

        bool Screen { get; }
        bool OnScreen { get; }



        bool ContainsPosition(Rect screenPosition);
    }
}
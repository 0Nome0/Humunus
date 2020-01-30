using System;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using Object = UnityEngine.Object;
using System.Linq;
using NerScript.Input;


public class InputSystemAPI : IInputSystem
{
    public Vector3 PreviousMousePosition => InputSystem.Instance.PreviousMousePosition;
    public Vector3 CurrentMousePosition => InputSystem.Instance.CurrentMousePosition;
    public Vector3 MouseMoveDistance => InputSystem.Instance.MouseMoveDistance;
    public Vector3 MouseMoveDirection => InputSystem.Instance.MouseMoveDirection;
    public bool LeftClick => InputSystem.Instance.LeftClick;
    public bool OnLeftClick => InputSystem.Instance.OnLeftClick;
    public List<Touch> PreviousTouches => InputSystem.Instance.PreviousTouches;
    public List<Touch> CurrentTouches => InputSystem.Instance.CurrentTouches;
    public Touch? PreviousTouch => InputSystem.Instance.PreviousTouch;
    public Touch? CurrentTouch => InputSystem.Instance.CurrentTouch;
    public Vector3 PreviousTouchPosition => InputSystem.Instance.PreviousTouchPosition;
    public Vector3 CurrentTouchPosition => InputSystem.Instance.CurrentTouchPosition;
    public Vector3? TouchMoveDistance => InputSystem.Instance.TouchMoveDistance;
    public Vector3? TouchMoveDirection => InputSystem.Instance.TouchMoveDirection;
    public bool Touch => InputSystem.Instance.Touch;
    public bool OnTouch => InputSystem.Instance.OnTouch;
    public bool Screen => InputSystem.Instance.Screen;
    public bool OnScreen => InputSystem.Instance.OnScreen;

    public bool ContainsPosition(Rect screenPosition) => InputSystem.Instance.ContainsPosition(screenPosition);
}
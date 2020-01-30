using System;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using Object = UnityEngine.Object;
using System.Linq;

namespace NerScript.Input
{
    using Input = UnityEngine.Input;

    public class InputSystem : SingletonClass<InputSystem>, IInputSystem
    {

        private Camera mainCamera = null;



        public Vector3 PreviousMousePosition { get; private set; }
        public Vector3 CurrentMousePosition { get; private set; }
        public Vector3 MouseMoveDistance => CurrentMousePosition - PreviousMousePosition;
        public Vector3 MouseMoveDirection => MouseMoveDistance.normalized;
        public bool LeftClick { get; private set; }
        public bool OnLeftClick { get; private set; }


        public List<Touch> PreviousTouches { get; private set; }
        public List<Touch> CurrentTouches { get; private set; }
        public Touch? PreviousTouch => 0 < PreviousTouches.Count ? PreviousTouches[0] : (Touch?)null;
        public Touch? CurrentTouch => OnTouch ? CurrentTouches[0] : (Touch?)null;
        public Vector3 PreviousTouchPosition =>
            PreviousTouch == null ? new Vector3() : mainCamera.ScreenToViewportPoint(PreviousTouch.Value.position);
        public Vector3 CurrentTouchPosition =>
            CurrentTouch == null ? new Vector3() : mainCamera.ScreenToViewportPoint(CurrentTouch.Value.position);
        public Vector3? TouchMoveDistance => CurrentTouchPosition - PreviousTouchPosition;
        public Vector3? TouchMoveDirection => TouchMoveDistance?.normalized;
        public bool Touch { get; private set; }
        public bool OnTouch { get; private set; }

        public bool Screen => LeftClick || Touch;
        public bool OnScreen => OnLeftClick || OnTouch;




        public InputSystem()
        {







        }

        private void Init()
        {

        }

        public void Update()
        {
            mainCamera = Camera.main;
            UpdateInput();
        }

        public void UpdateInput()
        {
            OnLeftClick = Input.GetMouseButtonDown(0);
            LeftClick = Input.GetMouseButton(0);
            OnTouch = 0 < Input.touchCount && CurrentTouches.Count == 0;
            Touch = 0 < Input.touchCount;


            if (LeftClick)
            {
                if (OnLeftClick)
                {
                    CurrentMousePosition = PreviousMousePosition
                        = mainCamera.ScreenToViewportPoint(Input.mousePosition);
                }
                else
                {
                    PreviousMousePosition = CurrentMousePosition;
                    CurrentMousePosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
                }
            }
            PreviousTouches = CurrentTouches;
            CurrentTouches = Input.touches.ToList();
        }



        public bool ContainsPosition(Rect screenPosition)
        {
            foreach (var t in CurrentTouches)
            {
                if (screenPosition.Contains(t.position))
                {
                    return true;
                }
            }
            if (screenPosition.Contains(CurrentMousePosition))
            {
                return true;
            }

            return false;
        }

    }
}
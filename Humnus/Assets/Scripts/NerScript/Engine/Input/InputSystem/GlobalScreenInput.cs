using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using NerScript;
using Pointer = UnityEngine.InputSystem.Pointer;


public class GlobalScreenInput : SingletonClass<GlobalScreenInput>, GlobalEvent.IMouseActions
{
    public IOptimizedObservable<Vector2> OnScreen => onScreen;
    private readonly Subject<Vector2> onScreen = new Subject<Vector2>();

    private GlobalEvent.MouseActions input = new GlobalEvent.MouseActions(new GlobalEvent());
    public GlobalScreenInput()
    {
        input.SetCallbacks(this);
        Enable();
    }
    public void Enable() => input.Enable();
    public void Disable() => input.Disable();

    public Vector2 PointDelta
    {
        get {
            Vector2 delta = Vector2.zero;
            if (Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero)
            {
                Debug.Log("mouseMove!");
                delta = Mouse.current.delta.ReadValue();
            }
            else if (Touchscreen.current != null && Touchscreen.current.delta.ReadValue() != Vector2.zero)
            {
                Debug.Log("touchMove!");
                delta = Touchscreen.current.delta.ReadValue();
            }
            return delta;
        }
    }
    public void OnClick(InputAction.CallbackContext context)
    {
        onScreen.OnNext(Mouse.current.position.ReadValue());
    }
}

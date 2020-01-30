using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using NerScript;
using Pointer = UnityEngine.InputSystem.Pointer;


public class GlobalVelocityInput : SingletonClass<GlobalVelocityInput>, VelocityInput.INormalActions
{
    private VelocityInput.NormalActions input = new VelocityInput.NormalActions(new VelocityInput());
    public GlobalVelocityInput()
    {
        input.SetCallbacks(this);
        Enable();
    }
    public void Enable() => input.Enable();
    public void Disable() => input.Disable();

    public IOptimizedObservable<bool> Up => up;
    private readonly Subject<bool> up = new Subject<bool>();
    public void OnUp(InputAction.CallbackContext context) { up.OnNext(true); }

    public IOptimizedObservable<bool> Left => left;
    private readonly Subject<bool> left = new Subject<bool>();
    public void OnLeft(InputAction.CallbackContext context) { left.OnNext(true); }

    public IOptimizedObservable<bool> Down => down;
    private readonly Subject<bool> down = new Subject<bool>();
    public void OnDown(InputAction.CallbackContext context) { down.OnNext(true); }

    public IOptimizedObservable<bool> Right => right;
    private readonly Subject<bool> right = new Subject<bool>();
    public void OnRight(InputAction.CallbackContext context) { right.OnNext(true); }
}

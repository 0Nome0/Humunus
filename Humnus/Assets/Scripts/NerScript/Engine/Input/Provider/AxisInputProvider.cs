using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using NerScript;
using NerScript.Input;
using NerScript.Attribute;
using NerScript.RiValuer;

public class AxisInputProvider : MonoBehaviour, IAxisInput<Vector>, IRiValuerProvider
{
    [SerializeField, InputName] private string axisName = "";
    [SerializeField] private bool useUnityEvent = false;
    [SerializeField, ShowIf("useUnityEvent")] private AxisInputUnityEvent inputEvent = new AxisInputUnityEvent();
    private AxisInput input = new AxisInput();

    private void Awake()
    {
        input.axisNames = new string[] { axisName };
    }
    private void Start()
    {
        if (useUnityEvent)
        {
            this.ObserveEveryValueChanged(p => p.Down)
            .Subscribe(_ =>
            {
                inputEvent.down.Invoke();
                inputEvent.s_down.OnNext(Unit.Default);
            });
            this.ObserveEveryValueChanged(p => p.Up)
            .Subscribe(_ =>
            {
                inputEvent.up.Invoke();
                inputEvent.s_up.OnNext(Unit.Default);
            });
            this.ObserveEveryValueChanged(p => p.Move)
            .Subscribe(_ =>
            {
                inputEvent.move.Invoke();
                inputEvent.s_move.OnNext(Unit.Default);
            });
        }
        else
        {
            inputEvent = null;
        }
    }

    private void OnEnable()
    {
        ((IRiValuerProvider)this).providerInfo.UpdateFlow = true;
    }

    private void OnDisable()
    {
        ((IRiValuerProvider)this).providerInfo.UpdateFlow = false;
    }
    RiValuerProviderInfo IRiValuerProvider.providerInfo { get; set; } = new RiValuerProviderInfo()
    {
        UpdateFlow = true,
        ValueType = ValueDataType.Float,
    };

    RiValuerValue IRiValuerProvider.Flow()
    {
        return new RiValuerValue(this) { Float = Axis.x };
    }



    public bool Down => input.Down;
    public bool Up => input.Up;
    public Vector Axis => input.Axis;
    public bool Move => input.Move;
    public Vector Difference => input.Difference;
    public bool[] Direction => input.Direction;
}

[Serializable]
public class AxisInputUnityEvent
{
    public UnityEvent down = new UnityEvent();
    public UnityEvent up = new UnityEvent();
    public UnityEvent move = new UnityEvent();
    public Subject<Unit> s_down = new Subject<Unit>();
    public Subject<Unit> s_up = new Subject<Unit>();
    public Subject<Unit> s_move = new Subject<Unit>();
    public IOptimizedObservable<Unit> Down => s_down;
    public IOptimizedObservable<Unit> Up => s_up;
    public IOptimizedObservable<Unit> Move => s_move;
}

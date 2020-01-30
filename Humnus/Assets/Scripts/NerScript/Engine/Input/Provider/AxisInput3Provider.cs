using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using NerScript;
using NerScript.Input;
using NerScript.Attribute;
using NerScript.RiValuer;

public class AxisInput3Provider : MonoBehaviour, IAxisInput<Vector3>, IRiValuerProvider
{
    [SerializeField, InputName] private string xAxisName = "";
    [SerializeField, InputName] private string yAxisName = "";
    [SerializeField, InputName] private string zAxisName = "";
    [SerializeField] private bool useUnityEvent = false;
    [SerializeField, ShowIf("useUnityEvent")] private AxisInputUnityEvent inputEvent = new AxisInputUnityEvent();
    private AxisInput3 input = new AxisInput3();

    private void Awake()
    {
        input.axisNames = new string[] { xAxisName, yAxisName, zAxisName };
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
    RiValuerValue IRiValuerProvider.Flow()
    {
        return new RiValuerValue(this) { Vector3 = Axis };
    }
    RiValuerProviderInfo IRiValuerProvider.providerInfo { get; set; } = new RiValuerProviderInfo()
    {
        UpdateFlow = true,
        ValueType = ValueDataType.Vector3,
    };
    public bool Down => input.Down;
    public bool Up => input.Up;
    public Vector3 Axis => input.Axis;
    public bool Move => input.Move;
    public Vector3 Difference => input.Difference;
    public bool[] Direction => input.Direction;
}
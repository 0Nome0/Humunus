using System;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using NerScript.Attribute;
using NerScript.RiValuer;
using UniRx;

public class ExperiencePointController : MonoBehaviour, IRiValuerProvider
{
    [SerializeField, ReadOnlyOnInspector] private StockableGauge exp = new StockableGauge(0) {overflow = true};
    public int EXP { get => exp.Current; set => exp.Current = value; }
    public int NextEXP { get => exp.Max; set => exp.Max = value; }

    public IOptimizedObservable<(int hp, int change)> OnEXPChanged => onEXPChanged;
    private readonly Subject<(int hp, int change)> onEXPChanged = new Subject<(int hp, int change)>();

    public IOptimizedObservable<Unit> OnMax => onMax;
    private readonly Subject<Unit> onMax = new Subject<Unit>();


    public bool Max => exp.Full;
    public float Ratio => exp.Ratio;

    public void Start()
    {
        exp.overflow = true;
        onEXPChanged.Subscribe(
            _ => { ((IRiValuerProvider)this).providerInfo.ProviderNodeGroup.Flow(Flow()); });
    }

    public bool Experience(int _exp)
    {
        exp.Add(_exp);
        onEXPChanged.OnNext((exp.Current, _exp));
        return Max;
    }

    public void UpdateEXP()
    {
        exp.Add(0);
        onEXPChanged.OnNext((exp.Current, 0));
    }

    public void LevelUp(int nextEXP)
    {
        exp.RemoveStock();
        exp.Max = nextEXP;
        onEXPChanged.OnNext((exp.Current, NextEXP));
    }

    public RiValuerValue Flow()
    {
        return new RiValuerValue(this) {Float = Ratio};
    }

    RiValuerProviderInfo IRiValuerProvider.providerInfo { get; set; }
        = new RiValuerProviderInfo() {ValueType = ValueDataType.Float,};
}

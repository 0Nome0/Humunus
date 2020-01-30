using System;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using NerScript.Attribute;
using NerScript.RiValuer;
using UniRx;

public class HitPointController : MonoBehaviour, IRiValuerProvider
{
    [SerializeField] private Gauge hp = new Gauge(0);
    public int HP { get => hp.Current; set => hp.Current = value; }
    public int MaxHP { get => hp.Max; set => hp.Max = value; }

    public IOptimizedObservable<(int hp, int change)> OnHPChanged => onHPChanged;
    private readonly Subject<(int hp, int change)> onHPChanged = new Subject<(int hp, int change)>();
    public IOptimizedObservable<Unit> OnDead => onDead;
    private readonly Subject<Unit> onDead = new Subject<Unit>();

    public bool Dead => hp.Empty;
    public float Ratio => hp.Ratio;




    public void Start()
    {
        OnHPChanged.Subscribe(_ => DeadCheck());
        IRiValuerProvider prv = this;
        if(prv.providerInfo.ProviderNodeGroup != null)
        {
            OnHPChanged.Subscribe(_ => { prv.providerInfo.ProviderNodeGroup.Flow(Flow()); });
        }
    }

    public void UpdateHp()
    {
        hp.Add(0);
        onHPChanged.OnNext((hp.Current, 0));
    }

    public bool Damage(int damage)
    {
        hp.Add(-damage);
        onHPChanged.OnNext((hp.Current, -damage));
        return Dead;
    }
    public bool Heal(int heal)
    {
        hp.Add(heal);
        onHPChanged.OnNext((hp.Current, heal));
        return Dead;
    }
    public bool Kill()
    {
        int _hp = hp.Current;
        hp.ToEmpty();
        onHPChanged.OnNext((hp.Current, _hp));
        return Dead;
    }
    public bool FullHeal()
    {
        int _hp = hp.Current;
        hp.ToFull();
        onHPChanged.OnNext((hp.Current, MaxHP - _hp));
        return Dead;
    }

    public void GrowMaxHP(int addMaxHP, bool heal = false, int addHeal = 0)
    {
        hp.Max += addMaxHP;
        if(heal) Heal(addMaxHP + addHeal);
    }

    private void DeadCheck()
    {
        if(Dead)
        {
            onDead.OnNext(Unit.Default);
        }
    }

    public RiValuerValue Flow()
    {
        return new RiValuerValue(this) {Float = Ratio};
    }
    RiValuerProviderInfo IRiValuerProvider.providerInfo { get; set; } = new RiValuerProviderInfo()
    {
        ValueType = ValueDataType.Float,
    };
}

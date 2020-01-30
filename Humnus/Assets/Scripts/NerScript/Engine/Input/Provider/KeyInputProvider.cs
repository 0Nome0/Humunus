using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NerScript;
using NerScript.Attribute;
using NerScript.Input;
using NerScript.RiValuer;
using UniRx;

public class KeyInputProvider : MonoBehaviour, IKeyInput, IRiValuerProvider
{
    [SerializeField, SearchableEnum] private KeyCode key = KeyCode.None;
    [SerializeField] private bool useUnityEvent = false;
    [SerializeField, ShowIf("useUnityEvent")] private KeyInputUnityEvent inputEvent = new KeyInputUnityEvent();
    private KeyInput input = new KeyInput();
    private void Awake()
    {
        input.key = key;
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
            this.ObserveEveryValueChanged(p => p.OnDown)
            .Subscribe(_ =>
            {
                inputEvent.onDown.Invoke();
                inputEvent.s_onDown.OnNext(Unit.Default);
            });
            this.ObserveEveryValueChanged(p => p.OnUp)
            .Subscribe(_ =>
            {
                inputEvent.onUp.Invoke();
                inputEvent.s_onUp.OnNext(Unit.Default);
            });
            this.ObserveEveryValueChanged(p => p.OnInversion)
            .Subscribe(_ =>
            {
                inputEvent.onInversion.Invoke();
                inputEvent.s_onInversion.OnNext(Unit.Default);
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
        return new RiValuerValue(this) { Bool = Down };
    }
    RiValuerProviderInfo IRiValuerProvider.providerInfo { get; set; } = new RiValuerProviderInfo()
    {
        UpdateFlow = true,
        ValueType = ValueDataType.Bool,
    };
    public bool Down => input.Down;
    public bool Up => input.Up;
    public bool OnDown => input.OnDown;
    public bool OnUp => input.OnUp;
    public bool OnInversion => input.OnInversion;
}

[Serializable]
public class KeyInputUnityEvent
{
    public UnityEvent down = new UnityEvent();
    public UnityEvent up = new UnityEvent();
    public UnityEvent onDown = new UnityEvent();
    public UnityEvent onUp = new UnityEvent();
    public UnityEvent onInversion = new UnityEvent();
    public Subject<Unit> s_down = new Subject<Unit>();
    public Subject<Unit> s_up = new Subject<Unit>();
    public Subject<Unit> s_onDown = new Subject<Unit>();
    public Subject<Unit> s_onUp = new Subject<Unit>();
    public Subject<Unit> s_onInversion = new Subject<Unit>();
    public IOptimizedObservable<Unit> Down => s_down;
    public IOptimizedObservable<Unit> Up => s_up;
    public IOptimizedObservable<Unit> OnDown => s_onDown;
    public IOptimizedObservable<Unit> OnUp => s_onUp;
    public IOptimizedObservable<Unit> OnInversion => s_onInversion;
}

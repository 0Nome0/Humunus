using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript;
using NerScript.Attribute;
using NerScript.RiValuer;
using UniRx;

public class TransformController : MonoBehaviour, IRiValuerDemander
{
    public enum ControlType { Add, Set, Clamp }
    [HideInInspector] ValueDataType IRiValuerDemander.ValueType => ValueDataType.Vector3;
    [SerializeField] private TransformProperty control = TransformProperty.Position;
    [SerializeField] private ControlType type = ControlType.Add;
    [SerializeField, ShowIf("IsClamp")] private Vector3 min = new Vector3();
    [SerializeField, ShowIf("IsClamp")] private Vector3 max = new Vector3();


    public IOptimizedObservable<int> Name => name_;
    private readonly Subject<int> name_ = new Subject<int>();

    private bool IsClamp => type == ControlType.Clamp;
    void IRiValuerDemander.Draw(RiValuerValue value)
    {
        if (type == ControlType.Add) Add(value.Vector3);
        else if (type == ControlType.Set) Set(value.Vector3);
    }

    public void Add(Vector3 vector)
    {
        switch (control)
        {
            case TransformProperty.Position: Position += vector; break;
            case TransformProperty.LocalPosition: LocalPosition += vector; break;
            case TransformProperty.Rotation: Rotation += vector; break;
            case TransformProperty.LocalRotation: LocalRotation += vector; break;
            case TransformProperty.Scale: Scale += vector; break;
            case TransformProperty.LocalScale: LocalScale += vector; break;
        }
    }

    public void Set(Vector3 vector)
    {
        switch (control)
        {
            case TransformProperty.Position: Position = vector; break;
            case TransformProperty.LocalPosition: LocalPosition = vector; break;
            case TransformProperty.Rotation: Rotation = vector; break;
            case TransformProperty.LocalRotation: LocalRotation = vector; break;
            case TransformProperty.Scale: Scale = vector; break;
            case TransformProperty.LocalScale: LocalScale = vector; break;
        }
    }
    public void Clamp(Vector3 min, Vector3 max)
    {
        switch (control)
        {
            case TransformProperty.Position: Position.Clamp(min, max); break;
            case TransformProperty.LocalPosition: LocalPosition.Clamp(min, max); break;
            case TransformProperty.Rotation: Rotation.Clamp(min, max); break;
            case TransformProperty.LocalRotation: LocalRotation.Clamp(min, max); break;
            case TransformProperty.Scale: Scale.Clamp(min, max); break;
            case TransformProperty.LocalScale: LocalScale.Clamp(min, max); break;
        }
    }

    private void Update()
    {
        if (type == ControlType.Clamp) Clamp(min, max);
    }

    public Vector3 Position { get => transform.position; set => transform.position = value; }
    public Vector3 LocalPosition { get => transform.localPosition; set => transform.localPosition = value; }
    public Vector3 Rotation { get => transform.eulerAngles; set => transform.eulerAngles = value; }
    public Vector3 LocalRotation { get => transform.localEulerAngles; set => transform.localEulerAngles = value; }
    public Vector3 Scale
    {
        get => transform.lossyScale;
        set {
            if (transform.parent == null) transform.localScale = value;
            transform.localScale = value.Division(transform.parent.lossyScale);
        }
    }
    public Vector3 LocalScale { get => transform.localScale; set => transform.localScale = value; }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using NerScript;

public class NotesTapArea : MonoBehaviour
{
    public Button button = null;
    [SerializeField] private Directional larn = Directional.Left;


    private Subject<Directional> onTap = new Subject<Directional>();
    public IOptimizedObservable<Directional> OnTap => onTap;


    private void Awake()
    {
        button = GetComponent<Button>();

        button
        .OnPointerDownAsObservable()
        .Subscribe(_ =>
        {
            onTap.OnNext(larn);
        });
    }
}

using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class ScreenClickObserver : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick = null;


    public IOptimizedObservable<int> Name => _name;
    private readonly Subject<int> _name = new Subject<int>();

    private void Start()
    {
        GlobalScreenInput
           .Instance.OnScreen
           .TakeUntilDestroy(gameObject)
           .Subscribe(_ => onClick.Invoke());






    }
}

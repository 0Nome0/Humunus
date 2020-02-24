using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniRx;
using UniRx.Async;
using UnityEngine.EventSystems;

namespace NerScript.UI
{
    public class LongInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
    {
        public enum PointerEvent { Down, Up,Click, }

        [SerializeField] private int longFrame = 30;
        public UnityEvent onLongPressEvent = null;

        public bool isDown { get; private set; } = false;
        public bool longedInput { get; private set; } = false;

        public IOptimizedObservable<Unit> OnLongPress => onLongPress;
        private readonly Subject<Unit> onLongPress = new Subject<Unit>();

        public IOptimizedObservable<PointerEvent> OnPoint => onPoint;
        private readonly Subject<PointerEvent> onPoint = new Subject<PointerEvent>();




        public int PressCount { get; private set; }


        private void Start()
        {
            OnLongPress.Subscribe(_ =>
            {
                PressCount = -1;
                onLongPressEvent.Invoke();
                longedInput = true;
            });

            UpdateObserve();
        }

        private IDisposable UpdateObserve()
        {
            return
            Observable
            .EveryUpdate()
            .TakeUntilDestroy(gameObject)
            .Where(_ => CanInput())
            .Do(_ => PressCount++)
            .Where(_ => longFrame <= PressCount)
            .ThrottleFirstFrame(1)
            .Subscribe(_ => { onLongPress.OnNext(Unit.Default); });
        }
        private bool CanInput()
        {
            return
            isDown &&
            PressCount != -1
            ;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDown = true;
            longedInput = false;
            onPoint.OnNext(PointerEvent.Down);
            PressCount = 0;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
            onPoint.OnNext(PointerEvent.Up);
            PressCount = -1;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            isDown = false;
            PressCount = -1;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            onPoint.OnNext(PointerEvent.Click);
        }
    }
}
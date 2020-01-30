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
    public class ButtonLongInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private Button button = null;
        [SerializeField] private int longFrame = 30;
        public UnityEvent onLongPressEvent = null;

        public bool isDown { get; private set; } = false;
        public IOptimizedObservable<Unit> OnLongPress => onLongPress;
        private Subject<Unit> onLongPress = new Subject<Unit>();

        private int pressCount = 0;


        private void Start()
        {
            if (button == null) return;
            OnLongPress.Subscribe(_ =>
            {
                pressCount = -1;
                onLongPressEvent.Invoke();
                "Longpress".DebugLog();
            });

            UpdateObserve();
        }

        private IDisposable UpdateObserve()
        {
            return Observable
            .EveryUpdate()
            .TakeUntilDestroy(gameObject)
            .Where(_ => CanInput())
            .Do(_ => pressCount++)
            .Where(_ => longFrame <= pressCount)
            .Do(_ => onLongPress.OnNext(Unit.Default))
            .ThrottleFrame(1)
            .Subscribe();
        }
        private bool CanInput()
        {
            return 
                isDown &&
                pressCount != -1 &&
                button.enabled;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pressCount = 0;
            isDown = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            pressCount = -1;
            isDown = false;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            pressCount = -1;
        }
    }
}
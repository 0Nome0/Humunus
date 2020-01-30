using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;
using NerScript;
using NerScript.Anime;
using NerScript.Attribute;
using NerScript.Input;

namespace NerScript.UI
{
    public class SelectableMenuElement : MonoBehaviour
    {
        [Serializable]
        private class SelectableMenuElementEvents
        {
            public UnityEvent focusEvent = null;
            public UnityEvent defocusEvent = null;
            public UnityEvent selectEvent = null;
            public UnityEvent deselectEvent = null;
        }

        [SerializeField] private SelectableMenuElementEvents events = null;

        #region OnFocus
        private Subject<SelectableMenuElement> onFocus = new Subject<SelectableMenuElement>();
        public IOptimizedObservable<SelectableMenuElement> OnFocus => onFocus;
        private Subject<SelectableMenuElement> onDefocus = new Subject<SelectableMenuElement>();
        public IOptimizedObservable<SelectableMenuElement> OnDefocus => onDefocus;
        #endregion
        #region OnSelect
        private Subject<SelectableMenuElement> onSelect = new Subject<SelectableMenuElement>();
        public IOptimizedObservable<SelectableMenuElement> OnSelect => onSelect;
        private Subject<SelectableMenuElement> onDeselect = new Subject<SelectableMenuElement>();
        public IOptimizedObservable<SelectableMenuElement> OnDeselect => onDeselect;
        #endregion

        private ObjectAnimationController animController = null;

        private void Awake()
        {
            OnFocus.Subscribe(_ =>
            {
                events.focusEvent.Invoke();
                animController = gameObject.ObjectAnimation().TripScaleRel(1.2f, 40, 0, EasingTypes.QuadIn3).EndlessAnim().AnimationStart();
            });
            OnDefocus.Subscribe(_ =>
            {
                events.defocusEvent.Invoke();
                animController.Exit();
            });
            OnSelect.Subscribe(_ =>
            {
                events.selectEvent.Invoke();
            });
            OnDeselect.Subscribe(_ =>
            {
                events.deselectEvent.Invoke();
            });
        }

        public void Focused() => onFocus.OnNext(this);
        public void Defocused() => onDefocus.OnNext(this);
        public void Selected() => onSelect.OnNext(this);
        public void Deselected() => onDeselect.OnNext(this);
    }
}

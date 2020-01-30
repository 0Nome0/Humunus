using System;
using UnityEngine;
using UnityEngine.Events;

namespace NerScript.UI
{
    [Serializable]
    public class SelectableMenuEvents
    {
        public UnityEvent onFocusMove = null;
        public UnityEvent onSelect = null;
        public UnityEvent onDeselect = null;
    }
}

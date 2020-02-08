using System;
using System.Collections;
using System.Collections.Generic;

namespace NerScript
{
    using UnityEngine;

    public interface ISwitcher : IPointer
    {
        int Index { get; set; }
    }

    public interface ISwitcher<T> : ISwitcher, IPointer<T>
    {
    }

    public interface IListSwitcher<T> :  ISwitcher<IList<T>>, IListPointer<T>
    {
    }

    public abstract class SwitcherBase<T> : ISwitcher<T>
    {
        public int Index { get => index; set => index = value.Clamped(0, values.Count); }
        private int index = 0;

        public T Value { get => values[Index]; set => values[Index] = value; }
        private readonly IList<T> values = null;

        protected SwitcherBase(IList<T> values)
        {
            this.values = values;
        }
    }

    public class Switcher<T> : SwitcherBase<T>, ISwitcher<T>
    {
        public Switcher(IList<T> _values) : base(_values) {  }
        public Switcher(params T[] values) : base(values) { }
    }

    public class ListSwitcher<T> : SwitcherBase<IList<T>>, IListSwitcher<T>
    {
        public ListSwitcher(IList<IList<T>> _values) : base(_values) {}
        public ListSwitcher(params IList<T>[] values) : base(values) { }
    }

    public class ObjectSwitcher : SwitcherBase<Object>
    {
        public ObjectSwitcher(IList<Object> values) : base(values) {  }
        public ObjectSwitcher(params Object[] values) : base(values) {  }
    }
}

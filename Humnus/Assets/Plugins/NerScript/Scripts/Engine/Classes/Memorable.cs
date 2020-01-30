using System;
using System.Collections.Generic;

namespace NerScript
{
    [Serializable]
    public class Memorable<T> : IPointer<T>
    {
        public readonly struct Item
        {
            public readonly T item;
            public Item(in T item) { this.item = item; }
            public static explicit operator T(Item value) { return value.item; }
        }

        public Item CurrentItem => new Item(Current);
        public Item CreateItem(T item) => new Item(item);

        public T Value
        {
            get => Current;
            set => Update(value);
        }
        public T Current { get; private set; } = default;
        public T Previous { get; private set; }

        public Memorable(T _value = default)
        {
            Current = _value;
        }

        private void Update(T newValue)
        {
            Previous = Current;
            Current = newValue;
        }

        public bool EqualPrevious => Previous.Equals(Value);


        public void Deconstruct(out T c, out T p)
        {
            c = Current;
            p = Previous;
        }
    }
}

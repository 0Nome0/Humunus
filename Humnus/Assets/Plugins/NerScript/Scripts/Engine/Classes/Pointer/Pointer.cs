using System;
using System.Collections;
using System.Collections.Generic;

namespace NerScript
{
    using UnityEngine;

    public interface IPointer
    {
    }

    public interface IReadonlyPointer<out T> : IPointer
    {
        T Value { get; }
    }

    public interface IWriteonlyPointer<in T> : IPointer
    {
        T Value { set; }
    }

    public interface IPointer<T> : IReadonlyPointer<T>, IWriteonlyPointer<T>
    {
        new T Value { get; set; }
    }

    public interface IListPointer<T> :  IPointer<IList<T>>
    {
    }

    public class Pointer<T> : IPointer<T>
    {
        public T Value { get; set; }
        public Pointer(T value) { Value = value; }
    }

    public class ListPointer<T> : IListPointer<T>
    {
        public IList<T> Value { get; set; }

        public ListPointer(IList<T> value) { Value = value; }
    }

    public class ObjectPointer : IPointer<Object>
    {
        public Object Value { get; set; }
        public ObjectPointer(Object obj) { Value = obj; }
    }

    public class AssignmentPointer<T> : IPointer<T> where T : struct
    {
        public delegate void Assignment(T value);

        public event Assignment assignment;

        private T value = default;
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                assignment?.Invoke(this.value);
            }
        }

        public AssignmentPointer(T _value) { Value = _value; }
        public AssignmentPointer(T _value, Assignment _assignment)
        {
            Value = _value;
            assignment = _assignment;
        }
    }
}

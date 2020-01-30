using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NerScript
{
    public class TreeElement<T>
    {
        private readonly List<TreeElement<T>> children = new List<TreeElement<T>>();

        public TreeElement<T> Parent { get; protected set; } = null;
        public T Value { get; } = default;
        public IReadOnlyList<TreeElement<T>> Children => children;
        public int ChildCount => Children.Count;

        public TreeElement(T item)
        {
            Value = item;
        }

        public TreeElement<T> AddChild(TreeElement<T> child)
        {
            if(child == null) throw new ArgumentNullException($"Adding {nameof(child)} is null.");
            children.Add(child);
            child.Parent = this;
            return this;
        }

        public TreeElement<T> FindChild(TreeElement<T> child, bool recursive = true)
        {
            return FindChild(c => c == child, recursive);
        }

        public TreeElement<T> FindChild(Predicate<TreeElement<T>> match , bool recursive = true)
        {
            if(match == null) throw new ArgumentNullException($"{nameof(match)} is null.");
            TreeElement<T> find = children.Find(match);
            if(find != null || !recursive) return find;
            //return children.FirstOrDefault(c => c.FindChild(child) != null);
            foreach(var c in children)
            {
                find = c.FindChild(match);
                if(find != null) return find;
            }
            return null;
        }

        public TreeElement<T> RemoveChild(TreeElement<T> child)
        {
            children.Remove(child);
            return this;
        }

        public TreeElement<T> ClearChildren()
        {
            children.Clear();
            return this;
        }

        public TreeElement<T> RemoveOwn()
        {
            Parent.RemoveChild(this);
            return Parent;
        }

        public bool HasChild(TreeElement<T> child, bool recursive = true)
        {
            if(children.Contains(child)) return true;
            if(!recursive) return false;
            return children.Any(chi => chi.HasChild(child));
        }

        public bool HasParent(TreeElement<T> parent)
        {
            if(Parent == null) return false;
            if(Parent == parent) return true;
            return Parent.HasParent(parent);
        }
    }
}

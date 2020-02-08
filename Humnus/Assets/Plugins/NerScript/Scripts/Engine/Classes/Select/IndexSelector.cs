using System.Collections.Generic;
using System.Linq;

namespace NerScript
{
    public interface IIndexSelector
    {
        int SelectIndex { get; }
    }


    public class IndexSelector : IIndexSelector
    {
        public readonly bool allowNonSelect;
        public readonly bool loopSelect;

        public int SelectIndex
        {
            get => selectIndexes[0];
            private set
            {
                ClearSelect();
                selectIndexes[0] = Clamped(value);
            }
        }
        public List<int> selectIndexes = new List<int>() { };
        public int SelectCount => selectIndexes.Count;

        public bool IsSelect => SelectIndex != -1;
        public bool Contains(int index) => selectIndexes.Contains(index);

        protected virtual int Count { get; }

        public IndexSelector(int count, bool allowNonSelect = true, bool loopSelect = false)
        {
            this.allowNonSelect = allowNonSelect;
            this.loopSelect = loopSelect;
            Count = count;
            ClearSelect();
        }

        public void ClearSelect()
        {
            selectIndexes.Clear();
            selectIndexes.Add(allowNonSelect ? -1 : 0);
        }
        public void AllSelect()
        {
            selectIndexes.Clear();
            for(int i = 0; i < Count; i++)
            {
                selectIndexes.Add(i);
            }
        }

        public void Select(int index)
        {
            ClearSelect();
            SelectIndex = index;
        }
        public void SelectRange(int min, int max) { Selects(ListLib.CreateIntList(min, max)); }
        public void Selects(params int[] indexes) { Selects(indexes.ToList()); }
        public void Selects(List<int> indexes)
        {
            selectIndexes = indexes;
            Optimize();
        }
        /// <summary>
        /// If you select "6" while selecting "3", it becomes "3,4,5,6"
        /// </summary>
        public void SelectsToIndex(int atIndex)
        {
            if(selectIndexes.Contains(atIndex)) return;
            if(atIndex < SelectIndex)
            {
                SelectRange(atIndex, selectIndexes.Last());
            }
            else
            {
                SelectRange(selectIndexes.First(), atIndex);
            }
        }
        public void AddSelect(int index)
        {
            selectIndexes.Add(index);
            Optimize();
        }
        public void AddSelects(params int[] indexes) { AddSelects(indexes.ToList()); }
        public void AddSelects(IEnumerable<int> indexes)
        {
            selectIndexes.AddRange(indexes);
            Optimize();
        }
        public void MoveSelect(int velocity)
        {
            if(SelectCount != 0) SelectIndex += velocity;
        }
        public void RemoveSelect(int index) { selectIndexes.Remove(index); }

        public void Optimize()
        {
            selectIndexes =
                selectIndexes
                   .Where(IsInRange)
                   .Distinct()
                   .OrderBy(i => i)
                   .ToList();
            if(SelectCount == 0)
            {
                ClearSelect();
            }
        }
        private int Clamped(int index) => loopSelect ? index.Repeat(Count) : index.Clamped(0, Count - 1);
        private bool IsInRange(int index) => index.ContainsIn(0, Count - 1);
    }
}

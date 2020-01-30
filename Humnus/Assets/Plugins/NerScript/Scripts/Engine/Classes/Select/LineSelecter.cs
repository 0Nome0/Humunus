using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NerScript
{
    public class LineSelector
    {
        private readonly bool loopSelect;

        private Range selectRange = new Range() { start = 0, end = 0 };
        public Range SelectRange => selectRange;
        public int[] SelectPoints
        {
            get
            {
                int[] res = new int[selectRange.Count];
                for(int i = 0, p = selectRange.start; p <= selectRange.end; i++, p++)
                {
                    res[i] = p;
                }
                return res;
            }
        }
        public int SelectPoint => selectRange.start;

        public int SelectCount => selectRange.Count;
        public int SelectLength => selectRange.Length;
        public bool Contains(int index) => selectRange.Contains(index);

        //protected virtual Range Line { get; }
        private Range Line { get; }

        public LineSelector(Range line, bool loopSelect = false)
        {
            Line = line;
            this.loopSelect = loopSelect;
        }
        public LineSelector(int min, int max, bool loopSelect = false)
        {
            Line = new Range(min, max);
            this.loopSelect = loopSelect;
        }

        public void ClearSelect() { selectRange.Init(); }
        public void AllSelect() { selectRange = Line; }
        public void Select(int start, int end)
        {
            selectRange.Set(start, end);
            Optimize();
        }
        public void AddBefore(int velocity)
        {
            selectRange.start -= velocity;
            Optimize();
        }
        public void AddAfter(int velocity)
        {
            selectRange.end += velocity;
            Optimize();
        }
        public void MoveSelect(int velocity)
        {
            selectRange.start += velocity;
            selectRange.end += velocity;
            Optimize();
        }
        private void Optimize()
        {
            Order();
            Fit();
            Clamp();
        }
        private void Order()
        {
            if(selectRange.end < selectRange.start)
            {
                int temp = selectRange.start;
                selectRange.end = selectRange.start;
                selectRange.start = temp;
            }
        }
        private void Fit()
        {
            if(Line.Length < SelectLength)
            {
                selectRange.end = selectRange.start + Line.Length;
            }
        }
        private void Clamp()
        {
            if(selectRange.start < Line.start)
            {
                int l = SelectLength;
                selectRange.end = Line.end;
                selectRange.start = Line.end - l;
            }
            if(Line.end < selectRange.end)
            {
                int l = SelectLength;
                selectRange.start = Line.start;
                selectRange.end = Line.start + l;
            }
        }
    }
}

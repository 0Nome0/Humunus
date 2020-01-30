namespace NerScript
{
    public struct Range
    {
        public int start;
        public int end;
        public Range(int _start, int _end)
        {
            start = _start;
            end = _end;
        }
        public int Count => Length + 1;
        public int Length => end - start;
        public bool Contains(int num) => start <= num && num <= end;
        public void Init() { start = end = 0; }
        public void Set(int newStart, int newEnd)
        {
            start = newStart;
            end = newEnd;
        }
    }
}

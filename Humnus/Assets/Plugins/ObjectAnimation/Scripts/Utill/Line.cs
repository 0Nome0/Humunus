using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerScript.Anime
{
    internal struct Line
    {
        public float start;
        public float end;

        public Line(float start, float end)
        {
            this.start = start;
            this.end = end;
        }

        public float Length => end - start;
    }
}

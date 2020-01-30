using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NerScript.Anime
{
    internal struct Vector3Line
    {
        public Vector3 start;
        public Vector3 end;

        float Length => (end - start).magnitude;

        public Vector3Line(Vector3 _start, Vector3 _end)
        {
            start = _start;
            end = _end;
        }
    }
}

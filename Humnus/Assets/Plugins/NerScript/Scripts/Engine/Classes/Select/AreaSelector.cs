using System.Linq;
using UnityEngine;

namespace NerScript
{
    public class AreaSelector
    {
        private readonly LineSelector xLine;
        private readonly LineSelector yLine;
        public Vector2Int SelectSize => new Vector2Int(xLine.SelectCount, yLine.SelectCount);
        public int SelectArea => xLine.SelectCount * yLine.SelectCount;

        public Vector2Int[] SelectPoints
        {
            get
            {
                Vector2Int[] res = new Vector2Int[SelectArea];
                int[] xs = xLine.SelectPoints;
                int[] ys = yLine.SelectPoints;
                for(int i = 0, yi = 0; yi < ys.Length; yi++)
                {
                    for(int xi = 0; xi < xs.Length; i++, xi++)
                    {
                        res[i] = new Vector2Int(xs[xi], ys[yi]);
                    }
                }
                return res;
            }
        }
        public Vector2Int SelectPoint => new Vector2Int(xLine.SelectPoint, yLine.SelectPoint);

        public AreaSelector(Vector2Int size, bool loopSelect = false)
        {
            xLine = new LineSelector(0, size.x - 1, loopSelect);
            yLine = new LineSelector(0, size.y - 1, loopSelect);
        }

        public AreaSelector(int x, int y, bool loopSelect = false)
        {
            xLine = new LineSelector(0, x - 1, loopSelect);
            yLine = new LineSelector(0, y - 1, loopSelect);
        }

        public void SetRect(int x, int y)
        {
            xLine.Select(0, x - 1);
            yLine.Select(0, y - 1);
        }
        public void SetPosition(Vector2Int position)
        {
            MoveSelect(position.x - xLine.SelectPoint, position.y - yLine.SelectPoint);
        }

        public void MoveSelect(Vector2Int velocity)
        {
            MoveSelect(velocity.x, velocity.y);
        }

        public void MoveSelect(int x, int y)
        {
            xLine.MoveSelect(x);
            yLine.MoveSelect(y);
        }
    }
}

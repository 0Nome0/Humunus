using System;
using UnityEngine;

namespace NerScript
{
    [Serializable]
    public class RectField
    {
        public float top = 0;
        public float right = 0;
        public float bottom = 0;
        public float left = 0;

        public RectField(float top, float right, float bottom, float left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }

        public float Width => right + left;
        public float Height => top + bottom;
        public float Area => Width * Height;

        public Vector2 RT => new Vector2(right, top);
        public Vector2 RB => new Vector2(right, -bottom);
        public Vector2 LB => new Vector2(-left, -bottom);
        public Vector2 LT => new Vector2(-left, top);

        public Vector2[] Points => new Vector2[] {RT, RB, LB, LT};

        public bool outOfField(Vector2 originalizedPosition)
        {
            if(top < originalizedPosition.y) return true;
            if(right < originalizedPosition.x) return true;
            if(originalizedPosition.y < -bottom) return true;
            if(originalizedPosition.x < -left) return true;
            return false;
        }
    }
}

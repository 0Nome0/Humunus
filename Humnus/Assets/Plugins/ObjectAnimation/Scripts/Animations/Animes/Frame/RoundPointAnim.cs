using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;


namespace NerScript.Anime
{
    public enum PlaneType { XY, XZ, YZ }
    //////現在使用不可
    internal sealed class RoundPoint : ObjectAnimBase
    {

        private readonly int frame = 0;
        private readonly Func<Transform, float> endrad = null;
        private Line line;
        private Vector3 origin;
        private readonly PlaneType type;
        float distance;

        //internal RoundPoint(Vector3 _origin, float _endRad, int _frame, PlaneType type, Func<Transform, Vector3> _endpos)
        internal RoundPoint(Vector3 _origin, int _frame, PlaneType _type, Func<Transform, float> _endrad)
            : base()
        {
            Name = "RoundPoint";
            origin = _origin;
            frame = _frame;
            endrad = _endrad;
            type = _type;

            origin = GetOrigin();

            //originが他で移動してリピートすると、おかしくなる。

            animation = (transform, detail) =>
            {
                animationPlayDetail = detail;
                distance = GetDistance(origin, transform.position);
                Vector3 nowRadV = transform.position - origin;
                float nowRad = ToPlaneVec2(nowRadV, type).VectorToRad();
                line = new Line(nowRad, endrad(transform));

                anim = GetUpdate(transform, () =>
                {
                    float rad = 0;
                    float lerp = (float)1 / frame;
                    rad = Mathf.Lerp(line.start, line.end, lerp);
                    Vector2 radV = rad.RadToVector();
                    Vector3 pos = CalPoint(radV);
                    Vector3 complet = CompletionPosition(transform.position);
                    transform.position = origin + (pos * distance) + complet;
                });
            };
        }
        protected override void SetAnimationLast()
        {
            animationLast = (tr) =>
            {
                Vector2 radV = line.end.RadToVector();
                Vector3 pos = CalPoint(radV);
                Vector3 complet = CompletionPosition(tr.position);
                tr.position = origin + (pos * distance) + complet;
            };
        }

        private Vector3 GetOrigin()
        {
            if (type == PlaneType.XY)
            {
                origin = new Vector3(origin.x, origin.y, 0);
            }
            else if (type == PlaneType.XZ)
            {
                origin = new Vector3(origin.x, 0, origin.z);
            }
            else if (type == PlaneType.YZ)
            {
                origin = new Vector3(0, origin.y, origin.z);
            }
            return origin;
        }

        private Vector3 CalPoint(Vector2 vec)
        {
            Vector3 position = new Vector3();
            if (type == PlaneType.XY)
            {
                position = new Vector3(vec.x, vec.y, 0);
            }
            else if (type == PlaneType.XZ)
            {
                position = new Vector3(vec.x, 0, vec.y);
            }
            else if (type == PlaneType.YZ)
            {
                position = new Vector3(0, vec.y, vec.x);
            }
            return position;
        }

        private float GetDistance(Vector3 origin, Vector3 position)
        {
            float distance = 0;
            Vector3 vec = position - origin;
            if (type == PlaneType.XY)
            {
                distance = new Vector2(vec.x, vec.y).magnitude;
            }
            if (type == PlaneType.XZ)
            {
                distance = new Vector2(vec.x, vec.z).magnitude;
            }
            if (type == PlaneType.YZ)
            {
                distance = new Vector2(vec.z, vec.y).magnitude;
            }
            return distance;
        }

        private Vector3 CompletionPosition(Vector3 position)
        {
            if (type == PlaneType.XY)
            {
                position = new Vector3(0, 0, position.z);
            }
            else if (type == PlaneType.XZ)
            {
                position = new Vector3(0, position.y, 0);
            }
            else if (type == PlaneType.YZ)
            {
                position = new Vector3(position.x, 0, 0);
            }
            return position;
        }

        private static Vector2 ToPlaneVec2(Vector3 vec, PlaneType type)
        {
            Vector2 radV = new Vector2();
            if (type == PlaneType.XY)
            {
                radV = new Vector2(vec.x, vec.y);
            }
            else if (type == PlaneType.XZ)
            {
                radV = new Vector2(vec.x, vec.z);
            }
            else if (type == PlaneType.YZ)
            {
                radV = new Vector2(vec.z, vec.y);
            }
            return radV;
        }




        protected override ObjectAnimRoot GetAnimClone()
        {
            return new RoundPoint(origin, frame, type, endrad);
        }

        internal static RoundPoint CreateAbs(Vector3 origin, float toRad, int frame, PlaneType type)
        {
            return new RoundPoint(origin, frame, type, (tr) => toRad);
        }

        internal static RoundPoint CreateRel(Vector3 origin, float rotateRad, int frame, PlaneType type)
        {
            return new RoundPoint(origin, frame, type, (tr) => (ToPlaneVec2(tr.position - origin, type).VectorToRad() + rotateRad));
        }

        protected override void ExitAnim(Transform tr)
        {

        }
    }


    public static partial class ObjectAnim
    {
        /// <summary>
        /// Position of transform that gameobject has will move to position.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="position"></param>
        public static AnimationPlanner RoundPointAbs(this AnimationPlanner animation, Vector3 origin, float toRad, int frame, PlaneType type)
        {
            return animation.AddAnimation(RoundPoint.CreateAbs(origin, toRad, frame, type));
        }
        public static AnimationPlanner RoundPointAbs(this AnimationPlanner animation, float posX, float posY, float posZ, float toRad, int frame, PlaneType type)
        {
            return animation.AddAnimation(RoundPoint.CreateAbs(new Vector3(posX, posY, posZ), toRad, frame, type));
        }

        public static AnimationPlanner RoundPointRel(this AnimationPlanner animation, Vector3 origin, float rotateRad, int frame, PlaneType type)
        {
            return animation.AddAnimation(RoundPoint.CreateRel(origin, rotateRad, frame, type));
        }
        public static AnimationPlanner RoundPointRel(this AnimationPlanner animation, float vecX, float vecY, float vecZ, float rotateRad, int frame, PlaneType type)
        {
            return animation.AddAnimation(RoundPoint.CreateRel(new Vector3(vecX, vecY, vecZ), rotateRad, frame, type));
        }
    }
}

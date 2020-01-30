using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace NerScript
{
    /// <summary>
    /// 拡張メソッド
    /// </summary>
    public static class TransformExtend
    {
        public struct Translate
        {
            SpaceType space;
            TransformType type;
            Axis axis;
            public int Int => space.Int() * 100 + type.Int() * 10 + axis.Int();

            public Translate(SpaceType _space, TransformType _type, Axis _axis)
            {
                space = _space;
                type = _type;
                axis = _axis;
            }
        }

        public static RectTransform SetPos(this RectTransform tr, float? x, float? y)
        {
            Vector2 p = tr.anchoredPosition;
            tr.anchoredPosition = new Vector2(x ?? p.x, y ?? p.y);
            return tr;
        }

        #region Axis

        public static Vector3 Bottom(this Transform tr)
        {
            return -tr.up;
        }
        public static Vector3 Left(this Transform tr)
        {
            return -tr.right;
        }
        public static Vector3 Back(this Transform tr)
        {
            return -tr.forward;
        }

        #endregion

        #region World

        #region SetPos

        public static Transform SetPos(this Transform tr, float? x, float? y, float? z)
        {
            Vector3 p = tr.position;
            tr.position = new Vector3(x ?? p.x, y ?? p.y, z ?? p.z);
            return tr;
        }
        public static Transform SetPosX(this Transform tr, float x)
        {
            Vector3 p = tr.position;
            tr.position = new Vector3(x, p.y, p.z);
            return tr;
        }
        public static Transform SetPosY(this Transform tr, float y)
        {
            Vector3 p = tr.position;
            tr.position = new Vector3(p.x, y, p.z);
            return tr;
        }
        public static Transform SetPosZ(this Transform tr, float z)
        {
            Vector3 p = tr.position;
            tr.position = new Vector3(p.x, p.y, z);
            return tr;
        }
        public static Transform PosToZero(this Transform tr)
        {
            tr.position = Vector3.zero;
            return tr;
        }

        #endregion

        #region AddPos

        public static Transform AddPosX(this Transform tr, float x)
        {
            tr.position += new Vector3(x, 0, 0);
            return tr;
        }
        public static Transform AddPosY(this Transform tr, float y)
        {
            tr.position += new Vector3(0, y, 0);
            return tr;
        }
        public static Transform AddPosZ(this Transform tr, float z)
        {
            tr.position += new Vector3(0, 0, z);
            return tr;
        }

        #endregion

        #region GetPos

        public static Vector3 GetUpPos(this Transform tr, float ratio = 1.0f)
        {
            return tr.position + (tr.up * ratio);
        }
        public static Vector3 GetBottomPos(this Transform tr, float ratio = 1.0f)
        {
            return tr.position - (tr.up * ratio);
        }
        public static Vector3 GetRightPos(this Transform tr, float ratio = 1.0f)
        {
            return tr.position + (tr.right * ratio);
        }
        public static Vector3 GetLeftPos(this Transform tr, float ratio = 1.0f)
        {
            return tr.position - (tr.right * ratio);
        }
        public static Vector3 GetForwardPos(this Transform tr, float ratio = 1.0f)
        {
            return tr.position + (tr.forward * ratio);
        }
        public static Vector3 GetBackPos(this Transform tr, float ratio = 1.0f)
        {
            return tr.position - (tr.forward * ratio);
        }

        #endregion

        #region MovePos

        public static Transform MoveTo(this Transform tr, Vector3 position, float ratio = 1.0f)
        {
            tr.position += (position - tr.position) * ratio;
            return tr;
        }
        public static Transform MoveForward(this Transform tr, float ratio = 1.0f)
        {
            tr.position += tr.forward * ratio;
            return tr;
        }
        public static Transform MoveBack(this Transform tr, float ratio = 1.0f)
        {
            tr.position -= tr.forward * ratio;
            return tr;
        }
        public static Transform MoveUp(this Transform tr, float ratio = 1.0f)
        {
            tr.position += tr.up * ratio;
            return tr;
        }
        public static Transform MoveDown(this Transform tr, float ratio = 1.0f)
        {
            tr.position -= tr.up * ratio;
            return tr;
        }
        public static Transform MoveRight(this Transform tr, float ratio = 1.0f)
        {
            tr.position += tr.right * ratio;
            return tr;
        }
        public static Transform MoveLeft(this Transform tr, float ratio = 1.0f)
        {
            tr.position -= tr.right * ratio;
            return tr;
        }

        #endregion

        #region SetRot

        public static Transform SetRot(this Transform tr, float? x, float? y, float? z)
        {
            Vector3 p = tr.eulerAngles;
            tr.eulerAngles = new Vector3(x ?? p.x, y ?? p.y, z ?? p.z);
            return tr;
        }
        public static Transform SetRotX(this Transform tr, float x)
        {
            Vector3 p = tr.eulerAngles;
            tr.eulerAngles = new Vector3(x, p.y, p.z);
            return tr;
        }
        public static Transform SetRotY(this Transform tr, float y)
        {
            Vector3 p = tr.eulerAngles;
            tr.eulerAngles = new Vector3(p.x, y, p.z);
            return tr;
        }
        public static Transform SetRotZ(this Transform tr, float z)
        {
            Vector3 p = tr.eulerAngles;
            tr.eulerAngles = new Vector3(p.x, p.y, z);
            return tr;
        }

        #endregion

        #region AddRot

        public static Transform AddRotX(this Transform tr, float x)
        {
            tr.eulerAngles += new Vector3(x, 0, 0);
            return tr;
        }
        public static Transform AddRotY(this Transform tr, float y)
        {
            tr.eulerAngles += new Vector3(0, y, 0);
            return tr;
        }
        public static Transform AddRotZ(this Transform tr, float z)
        {
            tr.eulerAngles += new Vector3(0, 0, z);
            return tr;
        }

        #endregion

        #region GetScl

        public static float GetMaxScl(this Transform tr)
        {
            Vector3 scl = tr.lossyScale;
            float max = Mathf.Max(scl.x, scl.y, scl.z);
            return max;
        }

        #endregion

        #endregion

        #region Local

        #region SetLocalPos

        public static Transform SetLclPos(this Transform tr, float? x, float? y, float? z)
        {
            Vector3 p = tr.localPosition;
            tr.localPosition = new Vector3(x ?? p.x, y ?? p.y, z ?? p.z);
            return tr;
        }
        public static Transform SetLclPosX(this Transform tr, float x)
        {
            Vector3 p = tr.localPosition;
            tr.localPosition = new Vector3(x, p.y, p.z);
            return tr;
        }
        public static Transform SetLclPosY(this Transform tr, float y)
        {
            Vector3 p = tr.localPosition;
            tr.localPosition = new Vector3(p.x, y, p.z);
            return tr;
        }
        public static Transform SetLclPosZ(this Transform tr, float z)
        {
            Vector3 p = tr.localPosition;
            tr.localPosition = new Vector3(p.x, p.y, z);
            return tr;
        }
        public static Transform LclPosToZero(this Transform tr)
        {
            tr.localPosition = Vector3.zero;
            return tr;
        }

        #endregion

        #region SetLocalRot

        public static Transform SetLclRot(this Transform tr, float? x, float? y, float? z)
        {
            Vector3 p = tr.localEulerAngles;
            tr.localEulerAngles = new Vector3(x ?? p.x, y ?? p.y, z ?? p.z);
            return tr;
        }
        public static Transform SetLclRotX(this Transform tr, float x)
        {
            Vector3 p = tr.localEulerAngles;
            tr.localEulerAngles = new Vector3(x, p.y, p.z);
            return tr;
        }
        public static Transform SetLclRotY(this Transform tr, float y)
        {
            Vector3 p = tr.localEulerAngles;
            tr.localEulerAngles = new Vector3(p.x, y, p.z);
            return tr;
        }
        public static Transform SetLclRotZ(this Transform tr, float z)
        {
            Vector3 p = tr.localEulerAngles;
            tr.localEulerAngles = new Vector3(p.x, p.y, z);
            return tr;
        }

        #endregion

        #region SetScale

        public static Transform SetScl(this Transform tr, float? x, float? y, float? z)
        {
            Vector3 p = tr.localScale;
            tr.localScale = new Vector3(x ?? p.x, y ?? p.y, z ?? p.z);
            return tr;
        }
        public static Transform SetSclX(this Transform tr, float x)
        {
            Vector3 p = tr.localScale;
            tr.localScale = new Vector3(x, p.y, p.z);
            return tr;
        }
        public static Transform SetSclY(this Transform tr, float y)
        {
            Vector3 p = tr.localScale;
            tr.localScale = new Vector3(p.x, y, p.z);
            return tr;
        }
        public static Transform SetSclZ(this Transform tr, float z)
        {
            Vector3 p = tr.localScale;
            tr.localScale = new Vector3(p.x, p.y, z);
            return tr;
        }
        public static Transform SetScl(this Transform tr, float s)
        {
            tr.localScale = new Vector3(s, s, s);
            return tr;
        }
        public static Transform SetSclTimes(this Transform tr, float s)
        {
            tr.localScale *= s;
            return tr;
        }

        #endregion

        #region AddScale

        public static Transform AddScale(this Transform tr, float scale)
        {
            tr.localScale += new Vector3(scale, scale, scale);
            return tr;
        }

        public static Transform AddScaleX(this Transform tr, float x)
        {
            tr.localScale += new Vector3(x, 0, 0);
            return tr;
        }
        public static Transform AddScaleY(this Transform tr, float y)
        {
            tr.localScale += new Vector3(0, y, 0);
            return tr;
        }
        public static Transform AddScaleZ(this Transform tr, float z)
        {
            tr.localScale += new Vector3(0, 0, z);
            return tr;
        }

        #endregion

        #endregion

        #region 親子関係

        public static List<Transform> GetChildren(this Transform trf)
        {
            List<Transform> children = new List<Transform>();
            for(int i = 0; i < trf.childCount; i++)
            {
                children.Add(trf.GetChild(i));
            }

            return children;
        }
        public static List<GameObject> GetChildrenG(this Transform trf)
        {
            List<GameObject> children = new List<GameObject>();
            for(int i = 0; i < trf.childCount; i++)
            {
                children.Add(trf.GetChild(i).gameObject);
            }

            return children;
        }
        /// <summary>
        /// 子供に追加
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static Transform AddChild(this Transform tr, Transform child)
        {
            child.SetParent(tr);
            return tr;
        }
        public static Transform AddChild(this Transform tr, GameObject child)
        {
            child.SetParent(tr);
            return tr;
        }
        /// <summary>
        /// 親に設定
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Transform SetParent(this Transform tr, GameObject parent)
        {
            tr.SetParent(parent.transform);
            return tr;
        }
        /// <summary>
        /// 親の設定の消去
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        public static Transform SetParent(this Transform tr)
        {
            tr.SetParent(null);
            return tr;
        }

        #endregion

        #region GetClosest

        /// <summary>
        /// 一番近いオブジェクトを取得
        /// </summary>
        /// <returns></returns>
        public static Transform GetClosestObjectInList2D(this Vector3 pos, List<Transform> objs)
        {
            if(objs.NullorEnpty()) return null;
            return objs.FindMin(tr => Vector2.Distance(tr.position, pos));
        }
        public static Transform GetClosestObjectInList2D(this Transform self, List<Transform> objs)
        {
            if(objs.NullorEnpty()) return null;
            Vector2 pos = self.position;
            return objs.FindMin(tr => Vector2.Distance(tr.position, pos));
        }
        public static T GetClosestObjectInList2D<T>(this Transform self, List<T> comps) where T : Component
        {
            if(comps.NullorEnpty()) return null;
            Vector2 pos = self.position;
            return comps.FindMin(comp => Vector2.Distance(comp.transform.position, pos));
        }
        public static Transform GetClosestObjectInList2D(this Transform self, List<GameObject> objs)
        {
            List<Transform> trs = objs.Select(obj => obj.transform).ToList();
            return GetClosestObjectInList2D(self, trs);
        }
        public static Transform GetClosestObjectInList(this Transform self, List<Transform> objs)
        {
            if(objs.NullorEnpty()) return null;
            Vector3 pos = self.position;
            return objs.FindMin(tr => Vector3.Distance(tr.position, pos));
        }
        public static Transform GetClosestObjectInList(this Transform tr, List<GameObject> objs)
        {
            List<Transform> trs = objs.Select(obj => obj.transform).ToList();
            return GetClosestObjectInList(tr, trs);
        }

        #endregion


        #region Enum

        public static Vector3 GetPosition(this Transform self, SpaceType space)
        {
            if(space == SpaceType.Local) return self.localPosition;
            return self.position;
        }
        public static float GetPosition(this Transform self, SpaceType space, Axis axis)
        {
            return self.GetPosition(space).GetAxis(axis);
        }
        public static Transform SetPosition(this Transform self, SpaceType space, Vector3 value)
        {
            if(space == SpaceType.World) self.position = value;
            else if(space == SpaceType.Local) self.localPosition = value;
            return self;
        }
        public static Transform SetPosition(this Transform self, SpaceType space, Axis axis, float value)
        {
            self.SetPosition(space, self.GetPosition(space).SetedAxis(axis, value));
            return self;
        }
        public static Transform AddPosition(this Transform self, SpaceType space, Vector3 value)
        {
            if(space == SpaceType.World) self.position += value;
            else if(space == SpaceType.Local) self.localPosition += value;
            return self;
        }
        public static Transform AddPosition(this Transform self, SpaceType space, Axis axis, float value)
        {
            self.SetPosition(space, self.GetPosition(space).AddedAxis(axis, value));
            return self;
        }

        #endregion

    }
}

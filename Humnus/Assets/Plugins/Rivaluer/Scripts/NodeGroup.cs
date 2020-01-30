using System;
using System.Collections.Generic;
using System.Linq;
using NerScript;
using UnityEngine;
using NodeType = NerScript.RiValuer.RiValuerNodeType;

namespace NerScript.RiValuer
{
    [Serializable]
    public class NodeGroup : IManagedByID
    {
        public static Dictionary<(ValueDataType, ValueDataType), Action<RiValuerValue>>
            convertMatch = new Dictionary<(ValueDataType, ValueDataType), Action<RiValuerValue>>()
            {
                {(ValueDataType.Bool, ValueDataType.Int), v => v.Int = v.Bool ? 1 : 0},
                {(ValueDataType.Bool, ValueDataType.Float), v => v.Float = v.Bool ? 1 : 0},
                {(ValueDataType.Bool, ValueDataType.String), v => v.String = v.Bool.ToString()},
                {(ValueDataType.Float, ValueDataType.Bool), v => v.Bool = v.Float != 0},
                {(ValueDataType.Float, ValueDataType.Int), v => v.Int = (int)v.Float},
                {(ValueDataType.Float, ValueDataType.String), v => v.String = v.Float.ToString()},
                {(ValueDataType.Float, ValueDataType.Vector2), v => v.Vector2.x = v.Float},
                {(ValueDataType.Float, ValueDataType.Vector3), v => v.Vector3.x = v.Float},
                {(ValueDataType.Int, ValueDataType.Bool), v => v.Bool = v.Int != 0},
                {(ValueDataType.Int, ValueDataType.Float), v => v.Float = v.Int},
                {(ValueDataType.Int, ValueDataType.String), v => v.String = v.Int.ToString()},
                {(ValueDataType.Int, ValueDataType.Vector2), v => v.Vector2.x = v.Int},
                {(ValueDataType.Int, ValueDataType.Vector3), v => v.Vector3.x = v.Int},
                {(ValueDataType.Vector2, ValueDataType.Int), v => v.Int = (int)v.Vector2.x},
                {(ValueDataType.Vector2, ValueDataType.Float), v => v.Float = v.Vector2.x},
                {(ValueDataType.Vector2, ValueDataType.String), v => v.String = v.Vector2.ToString()},
                {(ValueDataType.Vector2, ValueDataType.Vector3), v => v.Vector3 = v.Vector2},
                {(ValueDataType.Vector3, ValueDataType.Int), v => v.Int = (int)v.Vector3.x},
                {(ValueDataType.Vector3, ValueDataType.Float), v => v.Float = v.Vector3.x},
                {(ValueDataType.Vector3, ValueDataType.String), v => v.String = v.Vector3.ToString()},
                {(ValueDataType.Vector3, ValueDataType.Vector2), v => v.Vector2 = v.Vector3},
            };

        public NodeGroup(RiValuerNode self)
        {
            Self = self;
        }
        public NodeGroup(NodeGroup other)
        {
            Self = other.Self;
            _next = other._next;
            _previous = other._previous;
            _resource = other._resource;
        }

        public int ID { get => Self.ID; set => Self.ID = value; }

        public RiValuerNode Self = null;
        [SerializeField] private int nextID = -1;
        private NodeGroup _next = null;
        [SerializeField] private int previousID = -1;
        private NodeGroup _previous = null;
        [SerializeField] private int resourceID = -1;
        private NodeGroup _resource = null;

        public NodeGroup NextGroup
        {
            get
            {
                if(_next == null) throw new RiValuerException.NotArrivedException(Self.name + " Has no next.");
                return _next;
            }
            set
            {
                _next = value;
                nextID = (value == null ? -1 : value.ID);
            }
        }
        public NodeGroup PreviousGroup
        {
            get
            {
                if(_previous == null)
                    throw new RiValuerException.NotArrivedException(Self.name + "Has no previous.");
                return _previous;
            }
            set
            {
                _previous = value;
                previousID = value?.ID ?? -1;
            }
        }
        public NodeGroup ResourceGroup
        {
            get
            {
                if(_resource == null)
                    throw new RiValuerException.NotArrivedException(Self.name + "Has no resource.");
                return _resource;
            }
            set
            {
                _resource = value;
                resourceID = value?.ID ?? -1;
            }
        }


        public bool HasNext => nextID != -1;
        public bool HasPrevious => previousID != -1;
        public bool HasResource => resourceID != -1;



        public bool ValueTypeMatch(NodeGroup other) => Self.valueType == other.Self.valueType;
        public bool ResourceMatch(NodeGroup other)
        {
            return Self.nodeType == NodeType.Resource && other.Self.typeInfo == RiValuerNodeTypeInfo.Resourceable;
        }
        public bool ConvertMatch(NodeGroup next)
        {
            return Self.nodeType == NodeType.Convert &&
                convertMatch.ContainsKey((next.Self.valueType, Self.valueType));
        }
        public bool IsMultiValueType => Self.valueType == ValueDataType.Multi;

        public void NodeReference(List<NodeGroup> list)
        {
            if(HasNext)
            {
                NextGroup = list.Count <= nextID ? null : list[nextID];
            }
            if(HasPrevious)
            {
                PreviousGroup = list.Count <= previousID ? null : list[previousID];
            }
            if(HasResource)
            {
                ResourceGroup = list.Count <= resourceID ? null : list[resourceID];
            }
        }

        public void AdvSetNext(NodeGroup next)
        {
            if(HasNext) NextGroup.PreviousGroup = null;
            if(next.HasPrevious) next.PreviousGroup.NextGroup = null;
            if(HasNext && NextGroup.Self.nodeType == NodeType.Convert)
            {
                NextGroup.Self.valueType = ValueDataType.Multi;
            }

            NextGroup = next;
            next.PreviousGroup = this;
            next.Self.valueType = Self.valueType;
            Self.valueType = next.Self.valueType;
        }
        public void AdvSetResource(NodeGroup resource)
        {
            if(HasResource) ResourceGroup.NextGroup = null;
            if(resource.HasNext) resource.NextGroup = null;
            ResourceGroup = resource;
            resource.NextGroup = this;
            Self.value = resource.Self.value;
            resource.Self.valueType = (Self.nodeType == NodeType.Convert) ? ValueDataType.Int : Self.valueType;
        }
        public void AdvSetConvert(NodeGroup next)
        {
            if(HasNext) NextGroup.PreviousGroup = null;
            if(next.HasPrevious) next.PreviousGroup.NextGroup = null;
            NextGroup = next;
            next.PreviousGroup = this;
            Self.valueType = next.Self.valueType;
            if(next.Self.nodeType == NodeType.Convert) next.Self.valueType = Self.valueType;
            else
            {
                Self.valueType = next.Self.valueType;
            }
        }

        public void Flow(RiValuerValue flowedValue = default)
        {
            switch(Self.nodeType)
            {
                case NodeType.Provider:
                    flowedValue = Self.Provider.Flow();
                    break;
                case NodeType.Demander:
                    Self.Demander.Draw(flowedValue);
                    break;
                case NodeType.Add:
                    flowedValue.Add(Self.value);
                    break;
                case NodeType.Multiple:
                    flowedValue.Multiple(Self.value);
                    break;
                case NodeType.Convert:
                {
                    convertMatch[(PreviousGroup.Self.valueType, NextGroup.Self.valueType)](flowedValue);
                    if(HasResource && ResourceGroup.Self.valueType == ValueDataType.Int)
                    {
                        flowedValue.Vector2 =
                            Vector2.zero.MergeVec2(flowedValue.Vector2)[
                                ((VectorLib.VectorAxisMatch)flowedValue.Int).ToString()];
                        flowedValue.Vector3 =
                            Vector3.zero.MergeVec3(flowedValue.Vector3)[
                                ((VectorLib.VectorAxisMatch)flowedValue.Int).ToString()];
                    }
                }
                    break;
                default: return;
            }
            SaveFlowedValue(flowedValue);
            if(HasNext) NextGroup.Flow(flowedValue);
        }

        public void SaveFlowedValue(RiValuerValue flowedValue)
        {
            Self.lastFlowedNode = new RiValuerValue(flowedValue);
        }


        public bool Contains(NodeGroup node)
        {
            return
                this == node ||
                _next == node ||
                _previous == node ||
                _resource == node;
        }

        public void Reset(List<NodeGroup> list)
        {
            if(HasNext)
            {
                if(Self.nodeType == RiValuerNodeType.Resource) NextGroup.ResourceGroup = null;
                else NextGroup.PreviousGroup = null;
            }
            if(HasPrevious)
            {
                PreviousGroup.NextGroup = null;
            }
            if(HasResource)
            {
                ResourceGroup.NextGroup = null;
            }
            NextGroup = null;
            PreviousGroup = null;
            ResourceGroup = null;
            Self.Reset();
        }


    }
}

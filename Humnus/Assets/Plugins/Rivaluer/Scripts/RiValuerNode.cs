using System;
using UnityEngine;
using NerScript;


namespace NerScript.RiValuer
{
    [Serializable]
    public class RiValuerNode : IManagedByID
    {
        public string name = "Node";

        public int id = -1;

        public int ID { get => id; set => id = value; }
        public Rect rect = new Rect();
        public RiValuerValue value;
        public RiValuerValue lastFlowedNode;

        public RiValuerNodeType nodeType = RiValuerNodeType.None;
        public RiValuerNodeTypeInfo typeInfo = RiValuerNodeTypeInfo.None;
        public ValueDataType valueType = ValueDataType.None;



        public MonoBehaviour monobehaviour = null;
        public IRiValuerProvider Provider => (IRiValuerProvider)monobehaviour;
        public IRiValuerDemander Demander => (IRiValuerDemander)monobehaviour;

        public void SetProvider(MonoBehaviour _provider)
        {
            monobehaviour = _provider;
            if (monobehaviour == null) return;
            valueType = Provider.providerInfo.ValueType;
            NameSetting();
        }
        public void SetDemander(MonoBehaviour _demander)
        {
            monobehaviour = _demander;
            if (monobehaviour == null) return;
            valueType = Demander.ValueType;
            NameSetting();
        }
        private void NameSetting()
        {
            name = monobehaviour.GetType().ToString();
        }



        public void Reset()
        {
            value = new RiValuerValue();
            valueType = ValueDataType.Multi;
            monobehaviour = null;
        }
        public RiValuerNode() { }
        public RiValuerNode(int _id, Vector2 pos)
        {
            ID = _id;
            rect = new Rect(pos.x, pos.y, 200, 110);
        }
        public RiValuerNode(Rect _rect)
        {
            rect = _rect;
        }
        public RiValuerNode(RiValuerNode node)
        {
            name = node.name;
            ID = node.ID;
            rect = node.rect;
            value = node.value;
            nodeType = node.nodeType;
            typeInfo = node.typeInfo;
            valueType = node.valueType;
            monobehaviour = node.monobehaviour;
        }
        public static bool operator ==(RiValuerNode x, RiValuerNode y)
        {
            if (x is null && y is null) return true;
            if (x is null && y.ID == -1) return true;
            if (y is null && x.ID == -1) return true;
            return x.Equals(y);
        }
        public static bool operator !=(RiValuerNode x, RiValuerNode y)
        {
            if (x is null && y is null) return false;
            if (x is null && y.ID == -1) return false;
            if (y is null && x.ID == -1) return false;
            return !x.Equals(y);
        }
    }
}
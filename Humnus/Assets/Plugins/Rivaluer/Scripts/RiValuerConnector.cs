using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace NerScript.RiValuer
{
    public class RiValuerConnector : MonoBehaviour
    {
        public List<NodeGroup> nodes = new List<NodeGroup>();
        public List<NodeGroup> providers = new List<NodeGroup>();

        public ConnectionWindowStatus windowStatus = new ConnectionWindowStatus();


        public NodeGroup FindGroup(RiValuerNode node)
        {
            return nodes.Find(g => g.Self == node);
        }

        private void Awake()
        {
            NodeReference();
        }

        [ContextMenu("NodeReference")]
        public void NodeReference()
        {
            ProviderNodesReference();
            NodesReference();
            NodeIDSync();
        }
        private void NodesReference()
        {
            foreach (var node in nodes)
            {
                node.NodeReference(nodes);
            }
        }
        private void ProviderNodesReference()
        {
            for (int i = 0; i < providers.Count; i++)
            {
                NodeGroup pvn = nodes[providers[i].ID];
                providers[i] = pvn;
                pvn.NodeReference(nodes);
                if(pvn.Self.Provider == null) continue;
                pvn.Self.Provider.providerInfo.ProviderNodeGroup = pvn;
            }
        }
        public void NodeIDSync()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].ID = i;
            }
        }

        private void Update()
        {
            foreach (var pvn in providers)
            {
                if (!pvn.Self.Provider.providerInfo.UpdateFlow) return;
                pvn.Flow();
            }
        }
    }

    [Serializable]
    public class ConnectionWindowStatus
    {
        public Vector2 pivot = Vector2.zero;
        public float zoom = 1;
        public bool showFlag = false;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Enemies.BehaviourPattern.Nodes
{
    [Serializable]
    public class NodeSO : ScriptableObject
    {
        public string NodeType { get; protected set; }
        public string Name { get; protected set; }
        [field:SerializeField] public Vector2 NodePosition { get; set; }
        [field:SerializeField] public string Guid { get; set; }
        public PortDeclarationProperties InputPortProperties { get; protected set; }
        public PortDeclarationProperties OutputPortProperties { get; protected set; }
        [field:SerializeField] public NodeSO Parent { get; set; }
        [field:SerializeField] public List<NodeSO> Children { get; protected set; }
    }
}
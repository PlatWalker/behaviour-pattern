using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace jbzd.Enemies.BehaviourPattern.Nodes
{
    [Serializable]
    public class OutputPortsNode : NodeSO
    {
        public void OnEnable()
        {
            Children = new List<NodeSO>();
            NodeType = GetType().ToString();
            Name = "Output Port Node";
            InputPortProperties = new PortDeclarationProperties
            {
                Orientation = Orientation.Horizontal,
                Direction = Direction.Input,
                Capacity = Port.Capacity.Single,
                Type = typeof(bool)
            };
            OutputPortProperties = new PortDeclarationProperties
            {
                Orientation = Orientation.Horizontal,
                Direction = Direction.Output,
                Capacity = Port.Capacity.Multi,
                Type = typeof(bool)
            };
        }
    }
}
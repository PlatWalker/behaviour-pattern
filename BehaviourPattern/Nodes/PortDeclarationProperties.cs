using System;
using UnityEditor.Experimental.GraphView;

namespace jbzd.Enemies.BehaviourPattern.Nodes
{
    public class PortDeclarationProperties
    {
        public Orientation Orientation { get; set; }
        public Direction Direction { get; set; }
        public Port.Capacity Capacity { get; set; }
        public Type Type { get; set; }
    }
}
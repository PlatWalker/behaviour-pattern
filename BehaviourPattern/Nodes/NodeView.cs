using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Enemies.BehaviourPattern.Nodes
{
    public class NodeView : Node
    {
        public NodeSO Node { get; }
        public Port InputPort { get; set; }
        public Port OutputPort { get; set; }
        public NodeView(NodeSO node)
        {
            Node = node;
            title = node.Name;
            viewDataKey = node.Guid;

            style.left = node.NodePosition.x;
            style.top = node.NodePosition.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            var properties = Node.InputPortProperties;

            if (properties is null) return;
            
            InputPort = InstantiatePort(
                properties.Orientation,
                properties.Direction,
                properties.Capacity,
                properties.Type
                );

            if (InputPort == null) return;
            
            InputPort.portName = "";
            inputContainer.Add(InputPort);
        }

        private void CreateOutputPorts()
        {
            var properties = Node.OutputPortProperties;

            if (properties is null) return;
            
            OutputPort = InstantiatePort(
                properties.Orientation,
                properties.Direction,
                properties.Capacity,
                properties.Type
            );
            
            if (OutputPort == null) return;
            
            OutputPort.portName = "";
            outputContainer.Add(OutputPort);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            var newNodePosition = new Vector2
            {
                x = newPos.xMin,
                y = newPos.yMin
            };

            Node.NodePosition = newNodePosition;
            
        }
    }
}
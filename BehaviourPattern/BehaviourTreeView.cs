using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Enemies.BehaviourPattern.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.Enemies.BehaviourPattern
{
    public class BehaviourTreeView : GraphView
    {
        private const string USS_PATH = "Assets/Scripts/Enemies/BehaviourPattern/BehaviourTreeEditor.uss";

        private BehaviourTreeSO _behaviourTreeSo;
        
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> {}
        
        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(USS_PATH);
            styleSheets.Add(styleSheet);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            
            var types = GetType()
                .Assembly.GetTypes()
                .Where(type => type.BaseType == typeof(NodeSO) 
                               && !type.IsAbstract
                               && type.IsClass)
                .ToList();

            var nodesTypes = types
                .Select(ScriptableObject.CreateInstance)
                .Select(so => so as NodeSO)
                .ToList();

            if (!nodesTypes.Any()) return;
            
            //UnityBug: evt.localMousePosition zwraca zawsze zero w wyrazeniu lambda.
            //https://forum.unity.com/threads/how-to-get-mouse-click-position-when-got-contextualmenupopulateevent.614032/
            var mousePosition = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);
            
            foreach (var node in nodesTypes)
            {
                evt.menu.AppendAction($"[{node.NodeType}] {node.Name}", _ =>
                {
                    node.NodePosition = mousePosition;
                    
                    _behaviourTreeSo.AddNode(node);
                    CreateNodeView(node);
                    AssetDatabase.SaveAssets();
                    EditorUtility.SetDirty(node);
                    EditorUtility.SetDirty(_behaviourTreeSo);
                });
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node).ToList();
        }

        private NodeView FindNodeView(string id) => nodes.FirstOrDefault(node => node.viewDataKey == id) as NodeView;
        
        public void PopulateTreeView(BehaviourTreeSO tree)
        {
            _behaviourTreeSo = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
            
            foreach (var node in tree.Nodes)
            {
                CreateNodeView(node);
            }
            
            foreach (var node in tree.Nodes)
            {
                node.Children.ForEach(nodeChild =>
                {
                    var nodeParentView = FindNodeView(node.Guid);
                    var nodeChildView = FindNodeView(nodeChild.Guid);

                    var edge = nodeParentView.OutputPort.ConnectTo(nodeChildView.InputPort);
                    AddElement(edge);
                });
            }
            
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            graphViewChange.elementsToRemove?.ForEach(elem =>
            {
                switch (elem)
                {
                    case NodeView nodeView:
                        _behaviourTreeSo.DeleteNode(nodeView.Node);
                        break;
                    case Edge edge:

                        var parentView = edge.output.node as NodeView;
                        var childView = edge.input.node as NodeView;
                
                        Debug.Assert(parentView is not null && childView is not null);

                        _behaviourTreeSo.DeleteChildren(parentView.Node, childView.Node);
                        
                        break;
                }
            });

            graphViewChange.edgesToCreate?.ForEach(edge =>
            {
                var parentView = edge.output.node as NodeView;
                var childView = edge.input.node as NodeView;
                
                Debug.Assert(parentView is not null && childView is not null);
                
                _behaviourTreeSo.AddChild(parentView.Node, childView.Node);
                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(parentView.Node);
                EditorUtility.SetDirty(childView.Node);
                EditorUtility.SetDirty(_behaviourTreeSo);
            });
            return graphViewChange;
        }

        private void CreateNodeView(NodeSO node)
        {
            var nodeView = new NodeView(node);
            AddElement(nodeView);
        }
    }
}

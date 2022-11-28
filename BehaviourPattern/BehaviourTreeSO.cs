using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Enemies.BehaviourPattern.Nodes;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Enemies.BehaviourPattern
{
    [CreateAssetMenu(menuName = "New AI behaviour tree")]
    public class BehaviourTreeSO : ScriptableObject
    {
        [SerializeField] private List<ScriptableObject> nodes = new();
        
        /*
         * SO będzie przechowywać strukturę drzewa i referencję do skryptów które mają się wykonać? Behaviour tree jako component
         * będzie posiadał potencjalne referencje do sceny. Action node'y w inspectorze od ui buildera będą wyświetlać
         * to co będą wykorzystywać z referencji od componentu behaviourtree nałożonego na AI.
         */

        public IReadOnlyList<NodeSO> Nodes => nodes.Cast<NodeSO>().ToList();

        [OnOpenAsset]
        public static bool OpenGameStateWindow(int instanceID, int line)
        {
            if (Selection.activeObject is not BehaviourTreeSO) return false;
            
            var windowIsOpen = EditorWindow.HasOpenInstances<BehaviourTreeEditor>();
            if (!windowIsOpen)
            {
                EditorWindow.CreateWindow<BehaviourTreeEditor>();
            }
            else
            {
                EditorWindow.FocusWindowIfItsOpen<BehaviourTreeEditor>();
            }
            
            return false;
        }
        
        public void AddNode(NodeSO node)
        {
            node.Guid = Guid.NewGuid().ToString();
            node.name = node.Name;
            nodes.Add(node);
            
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(node);
        }
        
        public bool DeleteNode(NodeSO node)
        {
            try
            {
                nodes.Remove(node);
                AssetDatabase.RemoveObjectFromAsset(node);
                AssetDatabase.SaveAssets();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public void AddChild(NodeSO parent, NodeSO child)
        {
            parent.Children.Add(child);
            child.Parent = parent;
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(parent);
            EditorUtility.SetDirty(child);
            EditorUtility.SetDirty(this);
        }

        public void DeleteChildren(NodeSO parent, NodeSO child)
        {
            parent.Children.Remove(child);
            child.Parent = null;
        }
    }
}
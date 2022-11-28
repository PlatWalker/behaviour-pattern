using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.Enemies.BehaviourPattern
{
    public class BehaviourTreeEditor : EditorWindow
    {
        //TODO reference this files from unity settings like kiwiCoder
        private const string UXML_PATH = "Assets/Scripts/Enemies/BehaviourPattern/BehaviourTreeEditor.uxml"; 
        private const string USS_PATH = "Assets/Scripts/Enemies/BehaviourPattern/BehaviourTreeEditor.uss";

        private BehaviourTreeView _behaviourTreeView;
        private InspectorView _inspectorView;
        
        [MenuItem("BehaviourTreeEditor/Editor ...")]
        public static void OpenWindow()
        {
            var wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXML_PATH);
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(USS_PATH);
            root.styleSheets.Add(styleSheet);

            _behaviourTreeView = root.Q<BehaviourTreeView>();
            _inspectorView = root.Q<InspectorView>();

            OnSelectionChange();
        }

        public void OnSelectionChange()
        {
            if (Selection.activeObject is BehaviourTreeSO)
            {
                _behaviourTreeView.PopulateTreeView(Selection.activeObject as BehaviourTreeSO);
            }
        }
    }
}
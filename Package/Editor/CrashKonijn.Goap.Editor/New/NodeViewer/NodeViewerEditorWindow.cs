using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Editor.Classes;
using CrashKonijn.Goap.Editor.New.NodeViewer.Drawers;
using CrashKonijn.Goap.Interfaces;
using LamosInteractive.Goap.Debug;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.New.NodeViewer
{
    public class NodeViewerEditorWindow : EditorWindow
    {
        private IGoapRunner runner;
        private IGoapSet set;
        private AgentBehaviour agent;
        private List<AgentBehaviour> agents;
        private VisualElement leftPanel;
        private VisualElement rightPanel;

        private float lastUpdate = 0f;
        
        
        [MenuItem("Tools/GOAP/Node Viewer")]
        private static void ShowWindow()
        {
            var window = GetWindow<NodeViewerEditorWindow>();
            window.titleContent = new GUIContent("Node Viewer (GOAP)");
            window.Show();
        }

        private void OnFocus()
        {
            if (this.lastUpdate > Time.realtimeSinceStartup)
                this.lastUpdate = 0f;
        }

        public void OnEnable()
        {
            this.CreateGUI();
        }

        private void Update()
        {
            this.Render();
        }

        public void CreateGUI()
        {
            this.Render();
        }

        public void Render()
        {
            if (this.leftPanel == null)
            {
                Debug.Log("Creating GUI");
                
                var (left, right) = this.CreatePanels();

                this.leftPanel = left;
                this.rightPanel = right;
                
                var root = this.rootVisualElement;
                root.name = "node-viewer";
            
                var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.crashkonijn.goap/Editor/CrashKonijn.Goap.Editor/New/NodeViewer/NodeViewer.uss");
                root.styleSheets.Add(styleSheet);
            }
            
            if (!Application.isPlaying)
                return;

            if (Time.timeSinceLevelLoad - this.lastUpdate <= 0.5f)
            {
                return;
            }

            this.lastUpdate = Time.timeSinceLevelLoad;
            
            this.runner = FindObjectOfType<GoapRunnerBehaviour>();
            this.set = FindObjectOfType<GoapSetBehaviour>().Set;
            this.agents = FindObjectsOfType<AgentBehaviour>().ToList();

            this.RenderAgents();
            
            if (this.agent == null)
                return;
            
            if (!this.runner.Knows(this.set))
                return;

            this.RenderGraph();
        }

        private (VisualElement RootElement, VisualElement Right) CreatePanels()
        {
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

            var leftView = new VisualElement();
            var rightView = new VisualElement();
            
            splitView.Add(leftView);
            splitView.Add(rightView);
            
            this.rootVisualElement.Add(splitView);

            return (leftView, rightView);
        }

        private VisualElement GetGraphElement()
        {
            var graph = this.runner.GetGraph(this.set).ToPublic();
            var rootNode = this.agent == null ? graph.RootNodes.Values.First() : graph.RootNodes.Values.First(x => x.Action == this.agent.CurrentGoal);
            
            var nodes = new DebugGraph(graph).GetGraph(rootNode);

            return new NodesDrawer(nodes, this.agent);
        }

        private void RenderAgents()
        {
            this.leftPanel.Clear();
            
            // this.agents.ForEach(agent =>
            // {
            //     this.leftPanel.Add(new AgentDrawer(agent));
            // });
            
            var list = new ListView();

            list.fixedItemHeight = 70;
            list.makeItem = () => new AgentDrawer();
            list.bindItem = (item, index) => { (item as AgentDrawer).Update(this.agents[index]); };
            list.itemsSource = this.agents;
            
            list.SetSelectionWithoutNotify(new []{ this.agents.IndexOf(this.agent) });

            list.selectionChanged += _ =>
            {
                this.agent = this.agents[list.selectedIndex];
                this.RenderGraph();
            };

            this.leftPanel.Add(list);
        }
        
        private void RenderGraph()
        {
            this.rightPanel.Clear();
            this.rightPanel.Add(this.GetGraphElement());
        }
    }
}
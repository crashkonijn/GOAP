using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Editor.Classes;
using CrashKonijn.Goap.Editor.NodeViewer.Drawers;
using CrashKonijn.Goap.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer
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
        private DragDrawer dragDrawer;
        private VisualElement nodesDrawer;
        private VisualElement floatData;

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
            this.Render();
        }

        private void Update()
        {
            this.Render();
        }

        private void Render()
        {
            this.Init();
            
            if (!Application.isPlaying)
                return;

            if (Time.timeSinceLevelLoad - this.lastUpdate <= 0.5f)
            {
                return;
            }

            this.lastUpdate = Time.timeSinceLevelLoad;
            
            this.runner = FindObjectOfType<GoapRunnerBehaviour>();
            // this.set = FindObjectOfType<GoapSetBehaviour>().Set;
            this.agents = FindObjectsOfType<AgentBehaviour>().ToList();

            this.RenderAgents();
            
            if (this.agent == null)
                return;
            
            if (!this.runner.Knows(this.set))
                return;

            this.RenderGraph();
        }

        private void Init()
        {
            if (this.leftPanel != null)
                return;
            
            var (left, right) = this.CreatePanels();
                
            this.leftPanel = left;

            var dragParent = new VisualElement
            {
                name = "drag-parent"
            };
                
            right.Add(dragParent);
            this.rightPanel = dragParent;
            
            
            var root = this.rootVisualElement;
            root.name = "node-viewer-editor";
            
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.crashkonijn.goap/Editor/CrashKonijn.Goap.Editor/Styles/Generic.uss"));
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.crashkonijn.goap/Editor/CrashKonijn.Goap.Editor/Styles/NodeViewer.uss"));
            
#if UNITY_2022_1_OR_NEWER
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.crashkonijn.goap/Editor/CrashKonijn.Goap.Editor/Styles/NodeViewer_2022.uss"));
            this.dragDrawer = new DragDrawer(right, (offset) =>
            {
                dragParent.transform.position = offset;
                
                var posX = right.style.backgroundPositionX;
                posX.value = new BackgroundPosition(BackgroundPositionKeyword.Left, offset.x);
                right.style.backgroundPositionX = posX;
                    
                var posY = right.style.backgroundPositionY;
                posY.value = new BackgroundPosition(BackgroundPositionKeyword.Top, offset.y);
                right.style.backgroundPositionY = posY;
            });
#endif

            this.floatData = new VisualElement()
            {
                name = "float-right"
            };
            
            root.Add(this.floatData);
        }

        private (VisualElement RootElement, VisualElement Right) CreatePanels()
        {
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

            var leftView = new VisualElement
            {
                name = "left-panel"
            };

            var rightView = new VisualElement
            {
                name = "right-panel"
            };

            splitView.Add(leftView);
            splitView.Add(rightView);
            
            this.rootVisualElement.Add(splitView);

            return (leftView, rightView);
        }

        private VisualElement GetGraphElement()
        {
            var graph = this.runner.GetGraph(this.set).ToPublic();

            var element = new VisualElement();
            var widthOffset = 0f;

            foreach (var graphRootNode in graph.RootNodes.Values)
            {
                var debugGraph = new DebugGraph(graph).GetGraph(graphRootNode);
                var drawer = new NodesDrawer(debugGraph, this.agent);
                
                drawer.transform.position = new Vector3(widthOffset, 0f);
                
                element.Add(drawer);

                widthOffset += (debugGraph.MaxWidth * 250);
            }

            return element;
        }

        private void RenderAgents()
        {
            this.leftPanel.Clear();

            var list = new ListView
            {
                fixedItemHeight = 70,
                makeItem = () => new AgentDrawer(),
                bindItem = (item, index) => { (item as AgentDrawer).Update(this.agents[index]); },
                itemsSource = this.agents
            };

            list.SetSelectionWithoutNotify(new []{ this.agents.IndexOf(this.agent) });

#if UNITY_2021_1 || UNITY_2021_2 || UNITY_2021_3
            list.schedule.Execute(() =>
            {
                var index = list.selectedIndex;
                
                if (index < 0)
                    return;

                var agent = this.agents[index];

                if (agent == null)
                    return;
                
                this.agent = agent;
                this.set = this.agent.GoapSet;
                this.RenderGraph();
            }).Every(33);
#elif UNITY_2022_1_OR_NEWER
            list.selectionChanged += _ =>
            {
                this.agent = this.agents[list.selectedIndex];
                this.set = this.agent.GoapSet;
                this.RenderGraph();
            };
#endif
            this.leftPanel.Add(list);
        }
        
        private void RenderGraph()
        {
            this.rightPanel.Clear();

            this.nodesDrawer = this.GetGraphElement();
            
            this.rightPanel.Add(this.nodesDrawer);

            this.floatData.Clear();

            this.floatData.Add(new WorldDataDrawer(this.agent.WorldData));
            this.floatData.Add(new AgentDataDrawer(this.agent));
        }
    }
}
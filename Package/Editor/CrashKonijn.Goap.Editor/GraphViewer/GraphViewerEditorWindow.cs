using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace CrashKonijn.Goap.Editor
{
    public class GraphViewerEditorWindow : EditorWindow
    {
        private IGraph graph;
        private EditorWindowValues values = new ();

        [MenuItem("Tools/GOAP/Graph Viewer")]
        private static void ShowWindow()
        {
            var window = GetWindow<GraphViewerEditorWindow>();
            window.titleContent = new GUIContent("Graph Viewer (GOAP)");
            window.Show();
        }
        
        private void OnFocus()
        {
            this.OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            var selectedObject = Selection.activeObject;
            
            if (selectedObject == null)
                return;

            if (this.values.SelectedObject == selectedObject)
                return;

            if (selectedObject is AgentTypeScriptable agentTypeScriptable)
            {
                var agentType = new AgentTypeFactory(GoapConfig.Default).Create(agentTypeScriptable.Create(), false);
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
                
                this.RenderGraph(graph, selectedObject);
                return;
            }

            if (selectedObject is CapabilityConfigScriptable capabilityConfigScriptable)
            {
                var agentType = new AgentTypeFactory(GoapConfig.Default).Create(capabilityConfigScriptable.Create(), false);
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
                
                this.RenderGraph(graph, selectedObject);
                return;
            }

            if (selectedObject is ScriptableCapabilityFactoryBase capabilityFactoryScriptable)
            {
                var agentType =
                    new AgentTypeFactory(GoapConfig.Default).Create(capabilityFactoryScriptable.Create(), false);
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());

                this.RenderGraph(graph, selectedObject);
                return;
            }

            if (selectedObject is not GameObject gameObject)
                return;
            
            var typeFactory = gameObject.GetComponent<AgentTypeFactoryBase>();
            if (typeFactory != null)
            {
                var agentType = new AgentTypeFactory(GoapConfig.Default).Create(typeFactory.Create(), false);
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
                
                this.RenderGraph(graph, selectedObject);
                return;
            }
            
            var agent = gameObject.GetComponent<GoapActionProvider>();
            if (agent != null)
            {
                var agentType = agent.AgentType ?? new AgentTypeFactory(GoapConfig.Default).Create(agent.AgentTypeBehaviour.Config.Create(), false);
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
            
                this.RenderGraph(graph, agent);
                return;
            }
        }

        private void RenderGraph(IGraph graph, Object selectedObject)
        {
            if (this.graph == graph)
                return;
            
            if (this.values.SelectedObject == selectedObject)
                return;
            
            this.graph = graph;
            this.values.SelectedObject = selectedObject;
            
            this.rootVisualElement.Clear();
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/GraphViewer.uss");
            this.rootVisualElement.styleSheets.Add(styleSheet);

            var bezierRoot = new VisualElement();
            bezierRoot.AddToClassList("bezier-root");
            this.rootVisualElement.Add(bezierRoot);

            this.values.RootElement = this.rootVisualElement;

            var toolbar = new ToolbarElement(this.values);
            this.rootVisualElement.Add(toolbar);
            
            var dragRoot = new VisualElement();
            dragRoot.AddToClassList("drag-root");
            this.rootVisualElement.Add(dragRoot);
            
            var nodeRoot = new VisualElement();
            nodeRoot.AddToClassList("node-root");
            dragRoot.Add(nodeRoot);

            nodeRoot.schedule.Execute(() =>
            {
                nodeRoot.transform.scale = new Vector3(this.values.Zoom / 100f, this.values.Zoom / 100f, 1);
            }).Every(33);
            
            this.values.DragDrawer = new DragDrawer(dragRoot, (offset) =>
            {
                nodeRoot.transform.position = offset;
                
                this.values.Update();
                
#if UNITY_2022_1_OR_NEWER
                var posX = nodeRoot.style.backgroundPositionX;
                posX.value = new BackgroundPosition(BackgroundPositionKeyword.Left, offset.x);
                nodeRoot.style.backgroundPositionX = posX;
                    
                var posY = nodeRoot.style.backgroundPositionY;
                posY.value = new BackgroundPosition(BackgroundPositionKeyword.Top, offset.y);
                nodeRoot.style.backgroundPositionY = posY;
#endif
            });
            
            dragRoot.RegisterCallback<WheelEvent>((evt) =>
            {
                this.values.UpdateZoom(2 * (int) evt.delta.y);
            });

            foreach (var rootNode in graph.RootNodes)
            {
                this.CreateNode(nodeRoot, bezierRoot, rootNode);
            }

            foreach (var rootNode in graph.UnconnectedNodes)
            {
                this.CreateNode(nodeRoot, bezierRoot, rootNode);
            }
        }

        private void CreateNode(VisualElement parent, VisualElement bezierRoot, INode graphNode)
        {
            var node = new NodeElement(graphNode, bezierRoot, this.values);
            
            parent.Add(node);
        }
    }
}
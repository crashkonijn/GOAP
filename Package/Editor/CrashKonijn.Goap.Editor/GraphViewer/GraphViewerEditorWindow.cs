using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Editor.Drawers;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace CrashKonijn.Goap.Editor.GraphViewer
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
                var agentType = new AgentTypeFactory(GoapConfig.Default).Create(agentTypeScriptable, false);
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
                
                this.RenderGraph(graph, selectedObject);
            }

            if (selectedObject is CapabilityConfigScriptable capabilityConfigScriptable)
            {
                var agentType = new AgentTypeFactory(GoapConfig.Default).Create(capabilityConfigScriptable.GetConfig(), false);
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
                
                this.RenderGraph(graph, selectedObject);
            }
            
            if (selectedObject is GameObject gameObject)
            {
                var agent = gameObject.GetComponent<AgentBehaviour>();

                if (agent == null)
                    return;
                
                var agentType = agent.AgentType ?? new AgentTypeFactory(GoapConfig.Default).Create(agent.agentTypeBehaviour.Config, false);
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
                
                this.RenderGraph(graph, agent);
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
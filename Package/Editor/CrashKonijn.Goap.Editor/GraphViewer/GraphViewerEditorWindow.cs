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
        private EditorWindowValues values = new();

        [MenuItem("Tools/GOAP/Graph Viewer %g")]
        private static void ShowWindow()
        {
            var window = GetWindow<GraphViewerEditorWindow>();
            window.titleContent = new GUIContent("Graph Viewer (GOAP)");
            window.Show();

            EditorApplication.playModeStateChanged -= window.OnPlayModeChange;
            EditorApplication.playModeStateChanged += window.OnPlayModeChange;
        }

        private void OnPlayModeChange(PlayModeStateChange obj)
        {
            this.OnSelectionChange();
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

            var (agentType, obj) = this.GetAgentType(selectedObject);
            if (agentType == null)
                return;

            var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
            this.RenderGraph(graph, obj);
        }

        private (IAgentType agentType, Object obj) GetAgentType(Object selectedObject)
        {
            if (selectedObject is AgentTypeScriptable agentTypeScriptable)
            {
                return (new AgentTypeFactory(GoapConfig.Default).Create(agentTypeScriptable.Create(), false), selectedObject);
            }

            if (selectedObject is CapabilityConfigScriptable capabilityConfigScriptable)
            {
                return (new AgentTypeFactory(GoapConfig.Default).Create(capabilityConfigScriptable.Create(), false), selectedObject);
            }

            if (selectedObject is ScriptableCapabilityFactoryBase capabilityFactoryScriptable)
            {
                return (new AgentTypeFactory(GoapConfig.Default).Create(capabilityFactoryScriptable.Create(), false), selectedObject);
            }

            if (selectedObject is not GameObject gameObject)
                return default;

            var typeFactory = gameObject.GetComponent<AgentTypeFactoryBase>();
            if (typeFactory != null)
            {
                return (new AgentTypeFactory(GoapConfig.Default).Create(typeFactory.Create(), false), selectedObject);
            }

            var agentTypeBehaviour = gameObject.GetComponent<AgentTypeBehaviour>();
            if (agentTypeBehaviour != null)
            {
                return (new AgentTypeFactory(GoapConfig.Default).Create(agentTypeBehaviour.Config.Create(), false), selectedObject);
            }

            var provider = gameObject.GetComponent<GoapActionProvider>();
            if (provider == null)
                return default;

            if (provider.AgentType != null)
                return (provider.AgentType, provider);

            if (provider.AgentTypeBehaviour == null)
            {
                Debug.Log("Unable to render graph; No AgentType or AgentTypeBehaviour found on the agent! Please assign one in the inspector or through code in the Awake method.");
                return default;
            }

            return (new AgentTypeFactory(GoapConfig.Default).Create(provider.AgentTypeBehaviour.Config.Create(), false), provider);
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
                this.values.UpdateZoom(2 * (int) -evt.delta.y);
            });

            foreach (var rootNode in graph.RootNodes)
            {
                this.CreateNode(nodeRoot, bezierRoot, rootNode);
            }

            foreach (var rootNode in graph.UnconnectedNodes)
            {
                this.CreateNode(nodeRoot, bezierRoot, rootNode);
            }

            nodeRoot.schedule.Execute(() =>
            {
                if (selectedObject != null)
                    return;

                this.rootVisualElement.Clear();
            }).Every(33);
        }

        private void CreateNode(VisualElement parent, VisualElement bezierRoot, INode graphNode)
        {
            var node = new NodeElement(graphNode, bezierRoot, this.values);

            parent.Add(node);
        }
    }
}

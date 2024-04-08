using System;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Editor.NodeViewer.Drawers;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace CrashKonijn.Goap.Editor.Test
{
    public class TestEditorWindow : EditorWindow
    {
        private IGraph graph;
        private SelectedObject selectedObject = new ();
        private DragDrawer dragDrawer;

        [MenuItem("Tools/GOAP/Test Viewer")]
        private static void ShowWindow()
        {
            var window = GetWindow<TestEditorWindow>();
            window.titleContent = new GUIContent("Test Viewer (GOAP)");
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

            if (this.selectedObject.Object == selectedObject)
                return;

            if (selectedObject is AgentTypeScriptable agentTypeScriptable)
            {
                Debug.Log(agentTypeScriptable.name);
                
                var agentType = new AgentTypeFactory(GoapConfig.Default).Create(agentTypeScriptable);
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
                
                this.RenderGraph(graph, selectedObject);
            }

            if (selectedObject is CapabilityConfigScriptable capabilityConfigScriptable)
            {
                Debug.Log(capabilityConfigScriptable.name);
                
                var agentType = new AgentTypeFactory(GoapConfig.Default).Create(capabilityConfigScriptable.GetConfig());
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
                
                this.RenderGraph(graph, selectedObject);
            }
            
            if (selectedObject is GameObject gameObject)
            {
                var agent = gameObject.GetComponent<AgentBehaviour>();

                if (agent == null)
                    return;
                
                var agentType = agent.AgentType ?? new AgentTypeFactory(GoapConfig.Default).Create(agent.agentTypeBehaviour.Config);
                var graph = new GraphBuilder(GoapConfig.Default.KeyResolver).Build(agentType.GetAllNodes().ToArray());
                
                this.RenderGraph(graph, agent);
            }
        }

        private void RenderGraph(IGraph graph, Object selectedObject)
        {
            if (this.graph == graph)
                return;
            
            if (this.selectedObject.Object == selectedObject)
                return;
            
            this.graph = graph;
            this.selectedObject.SetObject(selectedObject);
            
            this.rootVisualElement.Clear();
            
            Debug.Log("cleared?");
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Test.uss");
            this.rootVisualElement.styleSheets.Add(styleSheet);

            var bezierRoot = new VisualElement();
            bezierRoot.AddToClassList("bezier-root");
            this.rootVisualElement.Add(bezierRoot);
            
            var nodeRoot = new VisualElement();
            nodeRoot.AddToClassList("node-root");
            this.rootVisualElement.Add(nodeRoot);
            
            this.dragDrawer = new DragDrawer(nodeRoot, (offset) =>
            {
                nodeRoot.transform.position = offset;
                
#if UNITY_2022_1_OR_NEWER
                var posX = nodeRoot.style.backgroundPositionX;
                posX.value = new BackgroundPosition(BackgroundPositionKeyword.Left, offset.x);
                nodeRoot.style.backgroundPositionX = posX;
                    
                var posY = nodeRoot.style.backgroundPositionY;
                posY.value = new BackgroundPosition(BackgroundPositionKeyword.Top, offset.y);
                nodeRoot.style.backgroundPositionY = posY;
#endif
            });

            foreach (var rootNode in graph.RootNodes)
            {
                this.CreateNode(nodeRoot, bezierRoot, rootNode, this.selectedObject);
            }

            foreach (var rootNode in graph.UnconnectedNodes)
            {
                this.CreateNode(nodeRoot, bezierRoot, rootNode, this.selectedObject);
            }
        }

        private void CreateNode(VisualElement parent, VisualElement bezierRoot, INode graphNode, SelectedObject selectedObject)
        {
            var node = new NodeElement(graphNode, bezierRoot, selectedObject);
            
            parent.Add(node);
        }
    }

    public class SelectedObject
    {
        public Object Object { get; private set; }
        public IMonoAgent Agent { get; private set; }
        
        public void SetObject(Object obj)
        {
            this.Object = obj;
            this.Agent = null;
        }
        
        public void SetAgent(IMonoAgent agent)
        {
            this.Agent = agent;
        }
    }

    public class NodeElement : VisualElement
    {
        public INode GraphNode { get; }

        public NodeElement(INode graphNode, VisualElement bezierRoot, SelectedObject selectedObject)
        {
            this.GraphNode = graphNode;
            this.AddToClassList("wrapper");
            
            this.NodeWrapper = new VisualElement();
            this.NodeWrapper.AddToClassList("node-wrapper");
            
            this.Node = new VisualElement();
            this.Node.AddToClassList("node");
            
            this.Content = new VisualElement();
            this.Content.AddToClassList("content");
            this.Content.Add(new Label($"{graphNode.Action.GetType().GetGenericTypeName()}"));
            
            this.Conditions = new VisualElement();
            this.Conditions.AddToClassList("conditions");
            this.Conditions.Add(new Label("Conditions"));

            this.Node.Add(this.Content);
            this.Node.Add(this.Conditions);

            this.Node.RegisterCallback<ClickEvent>(evt =>
            {
                this.Node.ToggleInClassList("collapsed");
            });

            this.NodeWrapper.Add(this.Node);
            
            this.ChildWrapper = new VisualElement();
            this.ChildWrapper.AddToClassList("child-wrapper");

            this.Add(this.ChildWrapper);
            this.Add(this.NodeWrapper);

            foreach (var condition in graphNode.Conditions)
            {
                var conditionElement = new ConditionElement(condition, selectedObject);
                this.Conditions.Add(conditionElement);
                
                foreach (var connection in condition.Connections)
                {
                    var connectionNode = new NodeElement(connection, bezierRoot, selectedObject);
                    this.ChildWrapper.Add(connectionNode);
                    
                    bezierRoot.Add(new ConnectionElement(conditionElement, connectionNode, selectedObject));
                }
            }
            
            if (!Application.isPlaying)
            {
                this.Effects = new VisualElement();
                this.Effects.AddToClassList("effects");
                this.Effects.Add(new Label("Effects"));
                this.Node.Add(this.Effects);
            
                foreach (var effect in graphNode.Effects)
                {
                    var effectElement = new EffectElement(effect, selectedObject);
                    this.Effects.Add(effectElement);
                }
            }
            
            this.schedule.Execute(() =>
            {
                this.Node.RemoveFromClassList("active");
                this.Node.RemoveFromClassList("path");

                if (selectedObject.Object is not IMonoAgent agent)
                    return;
                
                if (agent.CurrentGoal == this.GraphNode.Action)
                {
                    this.Node.AddToClassList("path");
                    return;
                }
                
                if (agent.CurrentAction == this.GraphNode.Action)
                {
                    this.Node.AddToClassList("active");
                    return;
                }
                
                if (agent.CurrentPlan.Contains(this.GraphNode.Action))
                {
                    this.Node.AddToClassList("path");
                    return;
                }
            }).Every(33);
        }

        public VisualElement Effects { get; set; }
        public VisualElement ChildWrapper { get; set; }
        public VisualElement Conditions { get; set; }
        public VisualElement Content { get; set; }
        public VisualElement Node { get; set; }
        public VisualElement NodeWrapper { get; private set; }
    }
    
    public class EffectElement : VisualElement
    {
        private readonly SelectedObject selectedObject;
        public INodeEffect GraphEffect { get; }

        public EffectElement(INodeEffect graphEffect, SelectedObject selectedObject)
        {
            this.selectedObject = selectedObject;
            this.GraphEffect = graphEffect;
            this.AddToClassList("effect");
            
            this.Label = new Label(this.GetText(graphEffect.Effect));
            this.Add(this.Label);
        }
        
        private string GetText(IEffect effect)
        {
            var suffix = effect.Increase ? "++" : "--";

            return $"{effect.WorldKey.Name}{suffix}";
        }

        public Label Label { get; set; }
    }

    public class ConditionElement : VisualElement
    {
        private readonly SelectedObject selectedObject;
        public INodeCondition GraphCondition { get; }

        public ConditionElement(INodeCondition graphCondition, SelectedObject selectedObject)
        {
            this.selectedObject = selectedObject;
            this.GraphCondition = graphCondition;
            this.AddToClassList("condition");
            
            this.Label = new Label(this.GetText(graphCondition.Condition, this.GetColor()));
            this.Add(this.Label);

            this.schedule.Execute(() =>
            {
                this.Label.text = this.GetText(this.GraphCondition.Condition, this.GetColor());
            }).Every(33);
        }

        private Color GetColor()
        {
            if (!Application.isPlaying)
                return Color.white;
            
            if (selectedObject.Object is not IMonoAgent agent)
                return Color.white;
            
            var conditionObserver = agent.AgentType.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(agent.WorldData);
            
            return conditionObserver.IsMet(GraphCondition.Condition) ? Color.green : Color.red;
        }
        
        private string GetText(ICondition condition, Color color)
        {
            var suffix = "";
            
            if (selectedObject.Object is IMonoAgent agent)
            {
                var (exists, value) = agent.WorldData.GetWorldValue(condition.WorldKey);

                suffix = "(" + (exists ? value.ToString() : "-") + ")";
            }
            
            return $"<color={this.GetHex(color)}>{condition.WorldKey.Name} {this.GetText(condition.Comparison)} {condition.Amount}</color> {suffix}";
        }

        private string GetHex(Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }
        
        private string GetText(Comparison comparison)
        {
            switch (comparison)
            {
                case Comparison.GreaterThan:
                    return ">";
                case Comparison.GreaterThanOrEqual:
                    return ">=";
                case Comparison.SmallerThan:
                    return "<";
                case Comparison.SmallerThanOrEqual:
                    return "<=";
            }
            
            return "";
        }

        public Label Label { get; set; }
    }
    
    public class ConnectionElement : VisualElement
    {
        private readonly ConditionElement condition;
        private readonly NodeElement node;
        private readonly SelectedObject selectedObject;

        public ConnectionElement(ConditionElement condition, NodeElement node, SelectedObject selectedObject)
        {
            this.condition = condition;
            this.node = node;
            this.selectedObject = selectedObject;
            
            this.Bezier = new BezierDrawer();
            this.Add(this.Bezier);
            
            this.schedule.Execute(() =>
            {
                var start = condition;
                var end = node.Node;
                
                var startPos = new Vector3(start.worldBound.position.x, start.worldBound.position.y - (start.worldBound.height / 4), 0);
                var endPos = new Vector3(end.worldBound.position.x + end.worldBound.width, end.worldBound.position.y, 0);
                    
                var startTan = startPos + (Vector3.left * 40);
                var endTan = endPos + (Vector3.right * 40);
            
                this.Bezier.Update(startPos, endPos, startTan, endTan, 2f, this.GetColor());
            }).Every(33);
        }

        private Color GetColor()
        {
            if (selectedObject.Object == null)
                return Color.black;
            
            if (selectedObject.Object is not IMonoAgent agent)
                return Color.black;

            var actions = agent.CurrentPlan;
            
            if (actions.Contains(this.node.GraphNode.Action))
                return new Color(0, 157, 100);
            
            return Color.black;
        }

        public BezierDrawer Bezier { get; set; }
    }
}
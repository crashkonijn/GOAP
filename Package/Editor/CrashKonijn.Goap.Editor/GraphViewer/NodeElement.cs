using System.Linq;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.GraphViewer
{
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

            this.Add(this.NodeWrapper);
            this.Add(this.ChildWrapper);

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
}
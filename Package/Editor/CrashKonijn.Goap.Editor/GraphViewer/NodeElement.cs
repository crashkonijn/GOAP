using System.Linq;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Editor.Elements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.GraphViewer
{
    public class NodeElement : VisualElement
    {
        public INode GraphNode { get; }

        public NodeElement(INode graphNode, VisualElement bezierRoot, EditorWindowValues values)
        {
            this.GraphNode = graphNode;
            this.AddToClassList("wrapper");
            
            this.NodeWrapper = new VisualElement();
            this.NodeWrapper.AddToClassList("node-wrapper");
            
            this.Node = new VisualElement();
            this.Node.AddToClassList("node");
            
            this.Content = new VisualElement();
            this.Content.AddToClassList("content");
            this.Title = new Label($"{graphNode.Action.GetType().GetGenericTypeName()}");
            this.Cost = new Label();
            
            this.Content.Add(new HorizontalSplitView(this.Title, this.Cost, 80));

            this.Target = new Label();
            this.Content.Add(this.Target);
            
            this.Conditions = new VisualElement();
            this.Conditions.AddToClassList("conditions");
            this.Conditions.Add(new Label("Conditions"));

            this.Node.Add(this.Content);
            this.Node.Add(this.Conditions);

            this.Node.RegisterCallback<ClickEvent>(evt =>
            {
                this.Node.ToggleInClassList("collapsed");
                
                values.Update();
            });

            this.NodeWrapper.Add(this.Node);
            
            this.ChildWrapper = new VisualElement();
            this.ChildWrapper.AddToClassList("child-wrapper");

            this.Add(this.NodeWrapper);
            this.Add(this.ChildWrapper);

            foreach (var condition in graphNode.Conditions)
            {
                var conditionElement = new ConditionElement(condition, values);
                this.Conditions.Add(conditionElement);
                
                foreach (var connection in condition.Connections)
                {
                    var connectionNode = new NodeElement(connection, bezierRoot, values);
                    this.ChildWrapper.Add(connectionNode);
                    
                    bezierRoot.Add(new ConnectionElement(this, conditionElement, connectionNode, values));
                }
            }
            
            if (!Application.isPlaying)
            {
                var config = (graphNode.Action as IGoapAction)?.Config;

                if (config != null)
                {
                    this.Target.text = $"Target: {config.Target.GetType().GetGenericTypeName()}";
                    this.Cost.text = $"Cost: {config.BaseCost}";
                }
                
                this.Effects = new VisualElement();
                this.Effects.AddToClassList("effects");
                this.Effects.Add(new Label("Effects"));
                this.Node.Add(this.Effects);
            
                foreach (var effect in graphNode.Effects)
                {
                    var effectElement = new EffectElement(effect);
                    this.Effects.Add(effectElement);
                }
            }
            
            this.schedule.Execute(() =>
            {
                this.Node.RemoveFromClassList("active");
                this.Node.RemoveFromClassList("disabled");
                this.Node.RemoveFromClassList("path");

                if (!Application.isPlaying)
                    return;

                if (values.SelectedObject is not IMonoGoapActionProvider agent)
                    return;
                
                if (agent.CurrentGoal == this.GraphNode.Action)
                {
                    this.Node.AddToClassList("path");
                    return;
                }
                
                if (agent.Agent.ActionState.Action == this.GraphNode.Action)
                {
                    this.Node.AddToClassList("active");
                    return;
                }
                
                if (agent.CurrentPlan.Contains(this.GraphNode.Action))
                {
                    this.Node.AddToClassList("path");
                    return;
                }

                if (graphNode.Action is not IAction action)
                    return;
                
                if (!action.IsEnabled(agent.Agent))
                {
                    this.Node.AddToClassList("disabled");
                    return;
                }

                this.Cost.text = $"Cost: {graphNode.GetCost(agent.Agent)}";
                var target = agent.WorldData.GetTarget(graphNode.Action as IGoapAction);
                var targetText = target != null ? target.Position.ToString() : "null";
                
                this.Target.text = $"Target: {targetText}";
            }).Every(33);
        }

        public Label Target { get; set; }

        public Label Cost { get; set; }

        public Label Title { get; set; }

        public VisualElement Effects { get; set; }
        public VisualElement ChildWrapper { get; set; }
        public VisualElement Conditions { get; set; }
        public VisualElement Content { get; set; }
        public VisualElement Node { get; set; }
        public VisualElement NodeWrapper { get; private set; }
    }
}
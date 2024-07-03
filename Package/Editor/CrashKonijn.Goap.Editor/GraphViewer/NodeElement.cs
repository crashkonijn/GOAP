using System;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class NodeElement : VisualElement
    {
        public INode GraphNode { get; }

        public NodeElement(INode graphNode, VisualElement bezierRoot, EditorWindowValues values, INode[] shownList = null)
        {
            var list = shownList ?? Array.Empty<INode>();
            
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
            
            var costWrapper = new VisualElement
            {
                style =
                {
                    alignItems = Align.FlexEnd,
                }
            };
            costWrapper.Add(this.Cost);
            
            this.Content.Add(new HorizontalSplitView(this.Title, costWrapper, 60));

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
                    if (list.Contains(connection))
                    {
                        Debug.Log($"Skipping connection {connection.Action?.GetType().GetGenericTypeName()} because it's already shown in the list. Recursive connection detected!");
                        continue;
                    }
                    
                    var connectionNode = new NodeElement(connection, bezierRoot, values, new []{ connection }.Concat(list).ToArray());
                    this.ChildWrapper.Add(connectionNode);
                    
                    bezierRoot.Add(new ConnectionElement(this, conditionElement, connectionNode, values));
                }
            }
            
            if (!Application.isPlaying)
            {
                if (graphNode.Action is IGoapAction goapAction)
                {
                    this.Target.text = $"Target: {goapAction.Config.Target?.GetType().GetGenericTypeName()}";
                    this.Cost.text = $"Cost: {goapAction.Config.BaseCost}";
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
                if (!Application.isPlaying)
                    return;

                if (values.SelectedObject is not IMonoGoapActionProvider provider)
                    return;
                
                this.UpdateClasses(graphNode, provider);
                
                this.Cost.text = $"Cost: {graphNode.GetCost(provider.Receiver):0.00}";

                if (graphNode.Action is IGoapAction action)
                {
                    var target = provider.WorldData.GetTarget(action);
                    var targetText = target != null ? target.Position.ToString() : "null";
                
                    this.Target.text = $"Target: {targetText}";
                }
                
            }).Every(33);
        }

        private void UpdateClasses(INode graphNode, IMonoGoapActionProvider provider)
        {
            this.Node.RemoveFromClassList("active");
            this.Node.RemoveFromClassList("disabled");
            this.Node.RemoveFromClassList("path");
                
            if (provider.CurrentPlan?.Goal == this.GraphNode.Action)
            {
                this.Node.AddToClassList("path");
                return;
            }
                
            if (provider.Receiver?.ActionState.Action == this.GraphNode.Action)
            {
                this.Node.AddToClassList("active");
                return;
            }
                
            if (provider.CurrentPlan != null && provider.CurrentPlan.Plan.Contains(this.GraphNode.Action))
            {
                this.Node.AddToClassList("path");
                return;
            }

            if (graphNode.Action is not IAction action)
                return;
                
            if (!action.IsEnabled(provider.Receiver))
            {
                this.Node.AddToClassList("disabled");
                return;
            }
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
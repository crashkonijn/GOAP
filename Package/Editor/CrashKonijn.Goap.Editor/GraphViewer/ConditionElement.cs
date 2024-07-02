using System.Linq;
using CrashKonijn.Goap.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class ConditionElement : VisualElement
    {
        private readonly EditorWindowValues values;
        public INodeCondition GraphCondition { get; }

        public Circle Circle { get; set; }

        public Label Label { get; set; }

        public ConditionElement(INodeCondition graphCondition, EditorWindowValues values)
        {
            this.values = values;
            this.GraphCondition = graphCondition;
            this.AddToClassList("condition");

            this.Circle = new Circle(this.GetCircleColor(graphCondition), 10f);
            this.Add(this.Circle);
            
            this.Label = new Label(this.GetText(graphCondition.Condition));
            this.Add(this.Label);

            this.schedule.Execute(() =>
            {
                this.Label.text = this.GetText(this.GraphCondition.Condition);
                this.Circle.SetColor(this.GetCircleColor(this.GraphCondition));
            }).Every(33);
        }

        private Color GetCircleColor(INodeCondition condition)
        {
            if (Application.isPlaying)
                return this.GetLiveColor();
            
            if (!condition.Connections.Any())
                return Color.red;
            
            return Color.green;
        }

        private Color GetLiveColor()
        {
            if (this.values.SelectedObject is not IMonoGoapActionProvider agent)
                return Color.white;
            
            if (agent.AgentType == null)
                return Color.white;
                
            var conditionObserver = agent.AgentType.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(agent.WorldData);
            
            return conditionObserver.IsMet(this.GraphCondition.Condition) ? Color.green : Color.red;
        }
        
        private string GetText(ICondition condition)
        {
            var suffix = this.GetSuffix(condition);
            
            return $"{condition.WorldKey.Name} {this.GetText(condition.Comparison)} {condition.Amount} {suffix}";
        }

        private string GetSuffix(ICondition condition)
        {
            if (!Application.isPlaying)
                return "";
            
            if (this.values.SelectedObject is not IMonoGoapActionProvider agent)
                return "";
            
            var (exists, value) = agent.WorldData.GetWorldValue(condition.WorldKey);
            
            return "(" + (exists ? value.ToString() : "-") + ")";
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
    }
}
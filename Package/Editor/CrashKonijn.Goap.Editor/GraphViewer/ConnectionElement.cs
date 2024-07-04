using System.Linq;
using CrashKonijn.Goap.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class ConnectionElement : VisualElement
    {
        private readonly NodeElement fromNode;
        private readonly NodeElement toNode;
        private readonly ConditionElement condition;
        private readonly EditorWindowValues values;

        public BezierDrawer Bezier { get; set; }
        public Label Cost { get; set; }

        public ConnectionElement(NodeElement fromNode, ConditionElement condition, NodeElement toNode, EditorWindowValues values)
        {
            this.fromNode = fromNode;
            this.condition = condition;
            this.toNode = toNode;
            this.values = values;
            
            this.Bezier = new BezierDrawer();
            this.Add(this.Bezier);
            
            this.Cost = new Label("")
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0,
                    left = 0,
                },
            };
            this.Cost.AddToClassList("distance-cost");
            this.Add(this.Cost);
            
            this.Update();
            
            this.values.OnUpdate += this.Update;
            
            this.schedule.Execute(this.Update).Every(100);
        }

        public void Update()
        {
            var magicOffset = 10f;
            
            var start = this.condition;
            var end = this.toNode.Node;
                
            var startPos = new Vector3(this.condition.parent.parent.worldBound.position.x + this.condition.parent.parent.worldBound.width, start.worldBound.position.y - magicOffset, 0);
            var endPos = new Vector3(end.worldBound.position.x, end.worldBound.position.y - magicOffset, 0);
                    
            var startTan = startPos + (Vector3.right * 40);
            var endTan = endPos + (Vector3.left * 40);
            
            this.Bezier.Update(startPos, endPos, startTan, endTan, 2f, this.GetColor());

            var center = this.Bezier.GetCenter();
            var cost = this.GetCost();
            var length = cost.Length * 5;
            
            this.Cost.style.left = center.x - length;
            this.Cost.style.top = center.y;
            this.Cost.text = cost;
            this.Cost.visible = !string.IsNullOrEmpty(cost);
        }

        private Color GetColor()
        {
            if (this.values.SelectedObject == null)
                return Color.black;
            
            if (this.values.SelectedObject is not IMonoGoapActionProvider agent)
                return Color.black;

            var actions = agent.CurrentPlan?.Plan;
            
            if (actions == null)
                return Color.black;
            
            if (actions.Contains(this.toNode.GraphNode.Action))
                return new Color(0, 157, 100);
            
            return Color.black;
        }

        private string GetCost()
        {
            if (this.values.SelectedObject is not IMonoGoapActionProvider agent)
                return "";
            
            if (!Application.isPlaying)
                return "";
            
            var fromAction = this.fromNode.GraphNode.Action as IGoapAction;
            var toAction = this.toNode.GraphNode.Action as IGoapAction;
            
            if (fromAction == null || toAction == null)
                return "";

            var startVector = agent.WorldData.GetTarget(fromAction);
            var endVector = agent.WorldData.GetTarget(toAction);
            
            if (startVector == null || endVector == null)
                return "";
            
            var distance = Vector3.Distance(startVector.Position, endVector.Position);
            var cost = agent.DistanceMultiplier * distance;
            
            return cost.ToString("F2");
        }
    }
}
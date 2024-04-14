using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Editor.Drawers;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.GraphViewer
{
    public class ConnectionElement : VisualElement
    {
        private readonly ConditionElement condition;
        private readonly NodeElement node;
        private readonly EditorWindowValues values;

        public BezierDrawer Bezier { get; set; }

        public ConnectionElement(ConditionElement condition, NodeElement node, EditorWindowValues values)
        {
            this.condition = condition;
            this.node = node;
            this.values = values;
            
            this.Bezier = new BezierDrawer();
            this.Add(this.Bezier);
            this.Update();
            
            this.values.OnUpdate += this.Update;
            
            this.schedule.Execute(this.Update).Every(100);
        }

        public void Update()
        {
            var magicOffset = 10f;
            
            var start = this.condition;
            var end = this.node.Node;
                
            var startPos = new Vector3(this.condition.parent.parent.worldBound.position.x + this.condition.parent.parent.worldBound.width, start.worldBound.position.y - magicOffset, 0);
            var endPos = new Vector3(end.worldBound.position.x, end.worldBound.position.y - magicOffset, 0);
                    
            var startTan = startPos + (Vector3.right * 40);
            var endTan = endPos + (Vector3.left * 40);
            
            this.Bezier.Update(startPos, endPos, startTan, endTan, 2f, this.GetColor());
        }

        private Color GetColor()
        {
            if (this.values.SelectedObject == null)
                return Color.black;
            
            if (this.values.SelectedObject is not IMonoAgent agent)
                return Color.black;

            var actions = agent.CurrentPlan;
            
            if (actions.Contains(this.node.GraphNode.Action))
                return new Color(0, 157, 100);
            
            return Color.black;
        }
    }
}
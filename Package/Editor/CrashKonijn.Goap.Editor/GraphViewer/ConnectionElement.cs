using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Editor.NodeViewer.Drawers;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.GraphViewer
{
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
            
            var magicOffset = 10f;
            
            this.schedule.Execute(() =>
            {
                var start = condition;
                var end = node.Node;
                
                var startPos = new Vector3(condition.parent.parent.worldBound.position.x + condition.parent.parent.worldBound.width, start.worldBound.position.y - magicOffset, 0);
                var endPos = new Vector3(end.worldBound.position.x, end.worldBound.position.y - magicOffset, 0);
                    
                var startTan = startPos + (Vector3.right * 40);
                var endTan = endPos + (Vector3.left * 40);
            
                this.Bezier.Update(startPos, endPos, startTan, endTan, 2f, this.GetColor());
            }).Every(33);
        }

        private Color GetColor()
        {
            if (this.selectedObject.Object == null)
                return Color.black;
            
            if (this.selectedObject.Object is not IMonoAgent agent)
                return Color.black;

            var actions = agent.CurrentPlan;
            
            if (actions.Contains(this.node.GraphNode.Action))
                return new Color(0, 157, 100);
            
            return Color.black;
        }

        public BezierDrawer Bezier { get; set; }
    }
}
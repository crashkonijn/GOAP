using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Editor.Classes;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer.Drawers
{
    public class NodesDrawer : VisualElement
    {
        // settings
        private const int Width = 200;
        private const int Height = 150;
        private const int MarginX = 50;
        private const int MarginY = 60;
        private const int Padding = 60;

        public NodesDrawer(Nodes nodes, IAgent agent)
        {
            this.name = "node-viewer";
            
            foreach (var (depth, renderNodes) in nodes.DepthNodes)
            {
                this.SetPositions(depth, renderNodes, nodes.MaxWidth);
            }

            var paths = new VisualElement();

            this.schedule.Execute(() =>
            {
                paths.Clear();

                foreach (var (key, start) in nodes.AllNodes)
                {
                    foreach (var guid in start.Node.Conditions.SelectMany(x => x.Connections))
                    {
                        var end = nodes.Get(guid);
                        var startPos = new Vector3(start.Rect.x + (start.Rect.width / 2f), start.Rect.y + start.Rect.height, 0);
                        var endPos = new Vector3(end.Rect.x + (start.Rect.width / 2f), end.Rect.y, 0);
                    
                        var startTan = startPos + (Vector3.up * 40);
                        var endTan = endPos + (Vector3.down * 40);
                    
                        paths.Add(new BezierDrawer(startPos, endPos, startTan, endTan, 2f, this.GetLineColor(end, agent)));
                    }
                }
            }).Every(500);
            
            this.Add(paths);
            
            foreach (var node in nodes.AllNodes.Values)
            {
                this.Add(new NodeDrawer(node, agent));
            }
        }

        private Color GetLineColor(RenderNode node, IAgent agent)
        {
            var color = Color.black;
            
            if (agent.CurrentActionPath.Contains(node.Node.Action))
                ColorUtility.TryParseHtmlString("#009dff", out color);
            
            return color;
        }
                
        private void SetPositions(int depth, List<RenderNode> renderNodes, int maxWidth)
        {
            for (var i = 0; i < renderNodes.Count; i++)
            {
                this.SetPosition(depth, renderNodes[i], maxWidth, i, renderNodes.Count);
            }
        }

        private void SetPosition(int depth, RenderNode renderNode, int maxWidth, int index, int rowCount)
        {
            var totalSize = (maxWidth * Width) + ((maxWidth - 1) * MarginX);
            var colSize = totalSize / rowCount;
            var colOffset = colSize / 2f;
            var sizeOffset = Width / 2f;

            renderNode.Position = new Vector2((colSize * index) + colOffset - sizeOffset, depth * (Height + MarginY)) + new Vector2(Padding, Padding);
        }
    }
}
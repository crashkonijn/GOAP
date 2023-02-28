using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Editor.Classes;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.New.NodeViewer.Drawers
{
    public class NodesDrawer : VisualElement
    {
        // settings
        private const int Width = 200;
        private const int Height = 150;
        private const int MarginX = 50;
        private const int MarginY = 50;
        private const int Padding = 50;

        public NodesDrawer(Nodes nodes, IAgent agent)
        {
            foreach (var (depth, renderNodes) in nodes.DepthNodes)
            {
                this.SetPositions(depth, renderNodes, nodes.MaxWidth);
            }
            
            foreach (var node in nodes.AllNodes.Values)
            {
                this.Add(new NodeDrawer(node, agent));
            }

            foreach (var (key, start) in nodes.AllNodes)
            {
                foreach (var guid in start.Node.Conditions.SelectMany(x => x.Connections))
                {
                    var end = nodes.Get(guid);
                    var startPos = new Vector3(start.Rect.x + (start.Rect.width / 2f), start.Rect.y + start.Rect.height, 0);
                    var endPos = new Vector3(end.Rect.x + (start.Rect.width / 2f), end.Rect.y, 0);
                    
                    this.Add(new LineDrawer(startPos, endPos, 2f));
                }
            }
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
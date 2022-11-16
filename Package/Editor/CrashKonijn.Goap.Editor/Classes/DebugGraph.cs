using System;
using System.Collections.Generic;
using System.Linq;
using LamosInteractive.Goap.Debug.Models;
using UnityEngine;

namespace CrashKonijn.Goap.Editor.Classes
{
    public class DebugGraph
    {
        private readonly Graph graph;

        public DebugGraph(Graph graph)
        {
            this.graph = graph;
        }

        public Nodes GetGraph(Node entryNode)
        {
            var nodes = new Nodes();
            
            this.StoreNode(0, entryNode, nodes);

            return nodes;
        }

        private void StoreNode(int depth, Node node, Nodes nodes)
        {
            nodes.Add(depth, node);

            foreach (var connection in node.Conditions.SelectMany(condition => condition.Connections))
            {
                this.StoreNode(depth + 1, this.GetNode(connection), nodes);
            }
        }

        private Node GetNode(Guid guid)
        {
            return this.graph.AllNodes.Values.First(x => x.Guid == guid);
            
            if (this.graph.AllNodes.TryGetValue(guid, out var value))
                return value;

            return null;
        }
    }

    public class Nodes
    {
        public Dictionary<int, List<RenderNode>> DepthNodes { get; private set; } = new ();
        public Dictionary<Guid, RenderNode> AllNodes { get; private set; } = new();
        public int MaxWidth { get; private set; }

        public RenderNode Get(Guid guid) => this.AllNodes[guid];
        
        private List<RenderNode> GetList(int depth)
        {
            if (this.DepthNodes.TryGetValue(depth, out var levels))
                return levels;

            levels = new List<RenderNode>();
            this.DepthNodes.Add(depth, levels);
            return levels;
        }
        
        public int GetMaxWidth()
        {
            var max = 0;

            foreach (var (key, value) in this.DepthNodes)
            {
                if (value.Count > max)
                    max = value.Count;
            }

            return max;
        }

        public void Add(int depth, Node node)
        {
            if (this.AllNodes.Values.Any(x => x.Node == node))
                return;

            var newNode = new RenderNode(node);
            this.GetList(depth).Add(newNode);
            this.AllNodes.Add(newNode.Node.Guid, newNode);

            this.MaxWidth = this.GetMaxWidth();
        }
    }

    public class RenderNode
    {
        private readonly Nodes nodes;
        public Node Node { get; }
        
        public Vector2 Position { get; set; }
        public Rect Rect => new Rect(this.Position.x, this.Position.y, 200, 150);

        public RenderNode(Node node)
        {
            this.Node = node;
        }
    }
}
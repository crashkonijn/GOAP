using UnityEngine;

namespace CrashKonijn.Goap.Editor
{
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
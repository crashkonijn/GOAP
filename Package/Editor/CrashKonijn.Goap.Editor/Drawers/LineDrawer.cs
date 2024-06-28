using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class LineDrawer : VisualElement
    {
        private readonly Vector3 startPos;
        private readonly Vector3 endPos;
        private readonly float thickness;
        private readonly Color strokeColor;

        public LineDrawer(Vector3 startPos, Vector3 endPos, float width, Color color)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            this.thickness = width;
            this.strokeColor = color;

            this.generateVisualContent += this.OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
#if UNITY_2022_1_OR_NEWER
            var paint = ctx.painter2D;
            
            paint.BeginPath();
            paint.MoveTo(this.startPos);
            paint.LineTo(this.endPos);
            paint.strokeColor = this.strokeColor;
            paint.lineWidth = this.thickness;
            paint.Stroke();
            paint.ClosePath();
            paint.Fill();
#else
            if (this.thickness <= 0f)
                return;

            var direction = (this.endPos - this.startPos).normalized;
            var perpDirection = new Vector3(-direction.y, direction.x, 0f);
            var halfThickness = this.thickness * 0.5f;
            perpDirection *= halfThickness;
            
            var p1 = this.startPos + perpDirection;
            var p2 = this.startPos - perpDirection;
            var p3 = this.endPos + perpDirection;
            var p4 = this.endPos - perpDirection;

            var writeData = ctx.Allocate(4, 6);
            if (writeData.vertexCount == 0)
                return;

            writeData.SetNextVertex(new Vertex() { position = new Vector3(p1.x, p1.y, Vertex.nearZ), tint = this.strokeColor });
            writeData.SetNextVertex(new Vertex() { position = new Vector3(p2.x, p2.y, Vertex.nearZ), tint = this.strokeColor });
            writeData.SetNextVertex(new Vertex() { position = new Vector3(p3.x, p3.y, Vertex.nearZ), tint = this.strokeColor });
            writeData.SetNextVertex(new Vertex() { position = new Vector3(p4.x, p4.y, Vertex.nearZ), tint = this.strokeColor });
            writeData.SetAllIndices(new ushort[] { 0, 1, 2, 2, 1, 3 });
#endif
        }
    }
}
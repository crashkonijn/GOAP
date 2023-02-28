using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.New.NodeViewer.Drawers
{
    public class LineDrawer : VisualElement
    {
        private Vector3 startPos, endPos;
        private float thickness;

        public LineDrawer(Vector3 pos1, Vector3 pos2, float width)
        {
            this.startPos = pos1;
            this.endPos = pos2;
            this.thickness = width;

            this.generateVisualContent += this.OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            var paint = ctx.painter2D;

            paint.BeginPath();
            paint.MoveTo(this.startPos);
            paint.LineTo(this.endPos);
            paint.strokeColor = Color.white;
            paint.lineWidth = this.thickness;
            paint.Stroke();
            paint.ClosePath();
            paint.Fill();
            // Debug.Log("ehm");
            // var angleDeg = Vector3.Angle(this.startPos, this.endPos);
            //
            // MeshWriteData mesh = ctx.Allocate(4, 6);
            // Vertex[] vertices = new Vertex[4];
            // vertices[0].position = this.startPos - new Vector3(0, this.thickness / 2, 0); //bottom left
            // vertices[1].position = this.startPos + new Vector3(0, this.thickness / 2, 0); //top left
            // vertices[2].position = this.endPos + new Vector3(0, this.thickness / 2, 0); //top right
            // vertices[3].position = this.endPos - new Vector3(0, this.thickness / 2, 0); //bottom right
            //
            // for (var index = 0; index < vertices.Length; index++)
            // {
            //     vertices[index].position += Vector3.forward * Vertex.nearZ;
            //     vertices[index].tint = Color.white;
            // }
            //
            // mesh.SetAllVertices(vertices);
            // mesh.SetAllIndices(new ushort[] { 0, 1, 3, 1, 2, 3 });

        }
    }
}
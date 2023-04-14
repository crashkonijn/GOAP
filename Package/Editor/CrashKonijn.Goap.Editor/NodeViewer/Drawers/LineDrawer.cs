using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer.Drawers
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
            var paint = ctx.painter2D;

            paint.BeginPath();
            paint.MoveTo(this.startPos);
            paint.LineTo(this.endPos);
            paint.strokeColor = this.strokeColor;
            paint.lineWidth = this.thickness;
            paint.Stroke();
            paint.ClosePath();
            paint.Fill();
        }
    }
}
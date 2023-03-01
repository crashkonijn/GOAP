using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.New.NodeViewer.Drawers
{
    public class BezierDrawer : VisualElement
    {
        private readonly Vector3 startPos;
        private readonly Vector3 endPos;
        private readonly Vector3 startTan;
        private readonly Vector3 endTan;
        private readonly float thickness;
        private readonly Color strokeColor;

        public BezierDrawer(Vector3 startPost, Vector3 endPos, Vector3 startTan, Vector3 endTan, float width, Color color)
        {
            this.startPos = startPost;
            this.endPos = endPos;
            this.startTan = startTan;
            this.endTan = endTan;
            this.thickness = width;
            this.strokeColor = color;
            
            this.generateVisualContent += this.OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            var paint = ctx.painter2D;

            paint.BeginPath();
            paint.MoveTo(this.startPos);

            const int count = 15;
            const float stepSize = 1f / count;

            for (var i = 0; i <= count; i++)
            {
                paint.LineTo(this.BezierPoint(stepSize * i));
            }
            
            paint.strokeColor = this.strokeColor;
            paint.lineWidth = this.thickness;
            paint.Stroke();
            paint.ClosePath();
        }

        private Vector3 BezierPoint(float t)
        {
            return this.BezierPoint(t, this.startPos, this.startTan, this.endTan, this.endPos);
        }
        
        private Vector3 BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var point = Vector3.zero;

            var u = 1.0f - t;
            var tt = t * t;
            var uu = u * u;
            var uuu = uu * u;
            var ttt = tt * t;

            point = uuu * p0;
            point += 3 * uu * t * p1;
            point += 3 * u * tt * p2;
            point += ttt * p3;

            return point;
        }
    }
}
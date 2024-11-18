using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_2021
using System.Collections.Generic;
#endif

namespace CrashKonijn.Goap.Editor
{
    public class BezierDrawer : VisualElement
    {
        private Vector3 startPos;
        private Vector3 endPos;
        private Vector3 startTan;
        private Vector3 endTan;
        private float thickness;
        private Color strokeColor;
        
        public BezierDrawer()
        {
            this.AddToClassList("bezier-drawer");
            
            this.generateVisualContent += this.OnGenerateVisualContent;

            // this.schedule.Execute(() =>
            // {
            //     // this.MarkDirtyRepaint();
            // }).Every(100);
        }

        public BezierDrawer(Vector3 startPost, Vector3 endPos, Vector3 startTan, Vector3 endTan, float width, Color color)
        {
            this.AddToClassList("bezier-drawer");
            
            this.startPos = startPost;
            this.endPos = endPos;
            this.startTan = startTan;
            this.endTan = endTan;
            this.thickness = width;
            this.strokeColor = color;
            
            this.generateVisualContent += this.OnGenerateVisualContent;
        }

        public void Update(Vector3 startPost, Vector3 endPos, Vector3 startTan, Vector3 endTan, float width, Color color)
        {
            this.startPos = startPost;
            this.endPos = endPos;
            this.startTan = startTan;
            this.endTan = endTan;
            this.thickness = width;
            this.strokeColor = color;
            
            this.MarkDirtyRepaint();
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
#if UNITY_2022_1_OR_NEWER
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
#else
            if (this.thickness <= 0f)
                return;

            var segments = Mathf.Max(1, Mathf.CeilToInt(Vector3.Distance(this.startPos, this.endPos) / 2f));
            var points = new List<Vector3>();

            for (var i = 0; i <= segments; i++)
            {
                var t = (float)i / (float)segments;
                points.Add(this.BezierPoint(t));
            }

            var vertexCount = segments * 4;
            var indexCount = segments * 6;

            var writeData = ctx.Allocate(vertexCount, indexCount);
            if (writeData.vertexCount == 0)
                return;

            for (var i = 0; i < segments; i++)
            {
                var direction = (points[i + 1] - points[i]).normalized;
                var perpDirection = new Vector3(-direction.y, direction.x, 0f);
                var halfThickness = this.thickness * 0.5f;
                perpDirection *= halfThickness;

                var p1 = points[i] + perpDirection;
                var p2 = points[i] - perpDirection;
                var p3 = points[i + 1] + perpDirection;
                var p4 = points[i + 1] - perpDirection;

                writeData.SetNextVertex(new Vertex() { position = new Vector3(p1.x, p1.y, Vertex.nearZ), tint = this.strokeColor });
                writeData.SetNextVertex(new Vertex() { position = new Vector3(p2.x, p2.y, Vertex.nearZ), tint = this.strokeColor });
                writeData.SetNextVertex(new Vertex() { position = new Vector3(p3.x, p3.y, Vertex.nearZ), tint = this.strokeColor });
                writeData.SetNextVertex(new Vertex() { position = new Vector3(p4.x, p4.y, Vertex.nearZ), tint = this.strokeColor });

                var indexStart = i * 4;
                writeData.SetNextIndex((ushort)indexStart);
                writeData.SetNextIndex((ushort)(indexStart + 1));
                writeData.SetNextIndex((ushort)(indexStart + 2));
                writeData.SetNextIndex((ushort)(indexStart + 2));
                writeData.SetNextIndex((ushort)(indexStart + 1));
                writeData.SetNextIndex((ushort)(indexStart + 3));
            }
#endif
        }
        
        public Vector3 GetCenter()
        {
            return this.BezierPoint(0.5f, this.startPos, this.startTan, this.endTan, this.endPos);
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
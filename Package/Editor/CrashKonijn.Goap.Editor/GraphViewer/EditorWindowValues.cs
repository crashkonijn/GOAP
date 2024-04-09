using System;
using CrashKonijn.Goap.Editor.NodeViewer.Drawers;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.GraphViewer
{
    public class EditorWindowValues
    {
        public int Zoom { get; set; } = 100;
        public VisualElement RootElement { get; set; }
        public DragDrawer DragDrawer { get; set; }
        
        public void UpdateZoom(int zoom)
        {
            if (zoom > 0)
            {
                this.Zoom = Math.Min(100, this.Zoom + zoom);
                return;
            }
            
            this.Zoom = Math.Max(50, this.Zoom + zoom);
        }
    }
}